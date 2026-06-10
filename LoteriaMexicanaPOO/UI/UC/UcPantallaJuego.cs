using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LoteriaMexicana.Controllers;
using LoteriaMexicana.Core;
using LoteriaMexicana.Logic;
using LoteriaMexicana.Network;

namespace LoteriaMexicana.UI.UserControls
{
    public partial class UcPantallaJuego : UserControl
    {
        public event Action OnSolicitarSalida;
        public event Action<string, string> OnPartidaTerminada;
        public event Action<string, string, List<int>, List<int>> OnEmpateDetectado;
        public event Action OnNuevaPartidaRecibida;
        private ClienteLoteria   _cliente;
        private ServidorLoteria  _servidor;
        private Mazo             _mazo;
        private System.Windows.Forms.Timer _timerCantor;
        private readonly GestorVoz _voz = new GestorVoz();
        private readonly List<Tablero> _tableros = new List<Tablero>();
        private readonly List<ControladorTablero> _ctrlTablas = new List<ControladorTablero>();
        private string _nombre = "Jugador";
        private bool   _esAnfitrion = false;
        private bool   _partidaTerminada = false;
        private bool   _descalificado = false;
        private List<string> _jugadoresEnDesempate = null;
        private int _advertenciasLoteria = 0;
        private const int MAX_ADVERTENCIAS = 3;
        private int    _rondaActual = 0;
        private int    _intervaloTimer = 5_000; // ms
        private bool   _timerPausado = false;
        private int?   _cartaDobleActual = null;
        private readonly Dictionary<int, string> _nombresCartas = new Dictionary<int, string>();
        public UcPantallaJuego()
        {
            InitializeComponent();
            PrecargarNombresCartas();
            btnPausarTimer.Click       += btnPausarTimer_Click;
            btnAumentarVelocidad.Click += btnAumentarVelocidad_Click;
            chkHorizontal.CheckedChanged += (s, e) => EnviarReglas();
            chkVertical.CheckedChanged   += (s, e) => EnviarReglas();
            chkDiagonal.CheckedChanged   += (s, e) => EnviarReglas();
            chkEsquinas.CheckedChanged   += (s, e) => EnviarReglas();
            chkPoyaCruz.CheckedChanged   += (s, e) => EnviarReglas();

            chkAutoCantar.CheckedChanged += (s, e) =>
            {
                if (chkAutoCantar.Checked && !_timerPausado && !_partidaTerminada && _mazo != null && !_mazo.EstaAgotado)
                {
                    IniciarTimerCantor();
                    _timerCantor.Start();
                }
                else
                {
                    _timerCantor?.Stop();
                }
            };
        }
        public void Configurar(string nombre, List<Tablero> tableros)
        {
            _nombre = nombre;
            _tableros.Clear();
            _tableros.AddRange(tableros);
            ConstruirTablas();
        }

        public void AsignarCliente(ClienteLoteria cliente)
        {
            _cliente = cliente;
            _cliente.OnMensajeRecibido  += ProcesarMensaje;
            _cliente.OnConectadoOk      += () => MostrarEnHistorial("[Sistema] Conectado correctamente.");
            _cliente.OnConectadoRechazado += motivo =>
            {
                if (IsDisposed) return;
                if (InvokeRequired) { BeginInvoke(new Action(() => ManejarRechazo(motivo))); return; }
                ManejarRechazo(motivo);
            };
            _cliente.OnError += msg => MostrarEnHistorial($"[ERROR RED] {msg}");
            _cliente.OnDesconectado += () => MostrarEnHistorial("Desconectado del servidor.");
        }

        private void ManejarRechazo(string motivo)
        {
            MessageBox.Show($"Conexión rechazada:\n{motivo}",
                "Nombre en uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            OnSolicitarSalida?.Invoke();
        }

        public void AsignarServidor(ServidorLoteria servidor, Mazo mazo)
        {
            _servidor    = servidor;
            _mazo        = mazo;
            _esAnfitrion = true;

            btnAccionRed.Text    = "Iniciar";
            btnAccionRed.Enabled = true;
            btnAccionRed.Click  -= btnAccionRed_ClickCrear;
            btnAccionRed.Click  += BtnIniciar_Click;

            chkAutoCantar.Visible       = true;
            panelAdminControles.Visible = true;
            HabilitarControlesReglas(true);
        }
        public int? ObtenerCartaDobleSiAplica()
        {
            if (chkCartasDobles.Checked) return new Random().Next(1, 55);
            return null;
        }

        private void BtnIniciar_Click(object sender, EventArgs e)
        {
            btnAccionRed.Text    = "Siguiente ▶";
            btnAccionRed.Click  -= BtnIniciar_Click;
            btnAccionRed.Click  += (s, ev) => CantarSiguienteCarta();
            
            if (_esAnfitrion)
            {
                int? idDoble = ObtenerCartaDobleSiAplica();
                if (idDoble.HasValue)
                {
                    _servidor.Transmitir($"MODO_DOBLE|{idDoble.Value}");
                }
                else
                {
                    _servidor.Transmitir($"MODO_DOBLE|0");
                }
                _cartaDobleActual = idDoble;
                _mazo.Reiniciar(idDoble);
                _mazo.Barajar();
                foreach (var t in _tableros) t.GenerarAleatorio(idDoble);
                ConstruirTablas();
            }

            MostrarEnHistorial("=== Partida iniciada ===");
            CantarSiguienteCarta();
        }

        public void MostrarCodigoSala(string codigo)
        {
            lblEstado.Text = $"SALA: {codigo}  |  Esperando jugadores...";
            txtSala.Text   = codigo;
            btnAccionRed.Text = "Siguiente";
        }

        public void MostrarConectado(string sala)
        {
            lblEstado.Text      = $"Conectado a la sala: {sala}";
            btnAccionRed.Enabled = false;
            HabilitarControlesReglas(false);
        }
        public void ReiniciarPartida(Mazo mazoNuevo)
        {
            _mazo            = mazoNuevo;
            _partidaTerminada = false;
            _descalificado   = false;
            _intervaloTimer  = 5_000;
            _timerPausado    = false;
            _advertenciasLoteria = 0;
            btnGritarLoteria.Enabled = true;
            btnEnviar.Enabled        = true;
            txtChatInput.Enabled     = true;
            _jugadoresEnDesempate = null;
            var miniaturas = new List<PictureBox>();
            foreach (Control c in panelHistorialCartas.Controls)
                if (c is PictureBox pb) miniaturas.Add(pb);
            foreach (var pb in miniaturas)
            {
                var img = pb.Image;
                pb.Image = null;
                panelHistorialCartas.Controls.Remove(pb);
                img?.Dispose();
                pb.Dispose();
            }

            var imgGrande = picCartaActual.Image;
            picCartaActual.Image = null;
            imgGrande?.Dispose();

            ConstruirTablas();

            if (_esAnfitrion)
            {
                btnAccionRed.Text    = "Iniciar";
                btnAccionRed.Enabled = true;
                btnAccionRed.Click  -= BtnIniciar_Click;
                btnAccionRed.Click  -= btnAccionRed_ClickCrear;
                btnAccionRed.Click  += BtnIniciar_Click;
                chkAutoCantar.Checked = false;
                _timerCantor?.Stop();
                ActualizarLabelVelocidad();
            }
            MostrarEnHistorial("=== Nueva partida iniciada ===");
        }

        public void SolicitarSalida() => btnSalir_Click(null, null);
        private void btnAccionRed_ClickCrear(object sender, EventArgs e) { }
        private void btnGritarLoteria_Click(object sender, EventArgs e)
        {
            if (_jugadoresEnDesempate != null && !_jugadoresEnDesempate.Contains(_nombre))
            {
                MessageBox.Show("Estás fuera de la ronda de desempate. Solo pueden ganar los jugadores empatados.",
                    "No puedes ganar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_partidaTerminada || _descalificado || _cliente == null) return;
            btnGritarLoteria.Enabled = false;

            for (int i = 0; i < _tableros.Count; i++)
            {
                var detalle = ValidadorVictoria.EvaluarConValidacion(
                    _tableros[i].Tapas,
                    _tableros[i].Casillas,
                    _tableros[i].CartasCantadas);

                switch (detalle.Resultado)
                {
                    case ValidadorVictoria.ResultadoValidacion.Victoria:
                        var idsGanadores = ObtenerIdsTapados(_tableros[i]);
                        var todosIds = string.Join(",", _tableros[i].ObtenerIdsEnOrden());
                        var idsGanStr = string.Join(",", idsGanadores);
                        _cliente.Enviar($"RECLAMO_LOTERIA|{_nombre}|{detalle.Figura} (Tabla {i + 1})|{i}|{idsGanStr}|{todosIds}");
                        return;

                    case ValidadorVictoria.ResultadoValidacion.Trampa:
                        string ids = string.Join(", #", detalle.CartasTrampa);
                        _advertenciasLoteria++;
                        int restantes = MAX_ADVERTENCIAS - _advertenciasLoteria;

                        if (_advertenciasLoteria >= MAX_ADVERTENCIAS)
                        {
                            MostrarEnHistorial($"Trampa detectada (#{ids}) — DESCALIFICADO.");
                            _cliente.Enviar($"PERDEDOR|{_nombre}|Marcó cartas no cantadas: #{ids}");
                            Descalificar("Marcaste casillas cuyas cartas aún no han salido tres veces.");
                        }
                        else
                        {
                            MostrarEnHistorial($"Advertencia {_advertenciasLoteria}/{MAX_ADVERTENCIAS}: casillas no cantadas tapadas #{ids}");
                            MessageBox.Show(
                                $" Advertencia {_advertenciasLoteria} de {MAX_ADVERTENCIAS}\n\nTienes casillas marcadas cuyas cartas aún no han salido.\nSi llegas a {MAX_ADVERTENCIAS} advertencias serás descalificado.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            btnGritarLoteria.Enabled = true;
                        }
                        return;
                }
            }
            _advertenciasLoteria++;
            int advsRestantes = MAX_ADVERTENCIAS - _advertenciasLoteria;

            if (_advertenciasLoteria >= MAX_ADVERTENCIAS)
            {
                _cliente.Enviar($"PERDEDOR|{_nombre}|Cantó Lotería sin completar ninguna figura (3 veces).");
                Descalificar("Cantaste Lotería sin figura tres veces.");
            }
            else
            {
                MostrarEnHistorial($"⚠ Advertencia {_advertenciasLoteria}/{MAX_ADVERTENCIAS}: Lotería cantada sin figura.");
                MessageBox.Show(
                    $"⚠ Advertencia {_advertenciasLoteria} de {MAX_ADVERTENCIAS}\n\nNo tienes ninguna figura completa.\nSi llegas a {MAX_ADVERTENCIAS} advertencias serás descalificado.",
                    "Sin figura", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnGritarLoteria.Enabled = true;
            }
        }
        private void Descalificar(string motivo)
        {
            _descalificado = true;
            btnGritarLoteria.Enabled  = false;
            btnGritarLoteria.Text     = "❌ DESCALIFICADO";
            btnGritarLoteria.BackColor = Color.FromArgb(80, 30, 30);
            MessageBox.Show($"¡Has sido descalificado!\n\n{motivo}\n\nPuedes seguir mirando la partida.",
                "Descalificado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private static List<int> ObtenerIdsTapados(Tablero t)
        {
            var ids = new List<int>();
            for (int f = 0; f < Tablero.FILAS; f++)
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                    if (t.Tapas[f, c]) ids.Add(t.Casillas[f, c]);
            return ids;
        }
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string txt = txtChatInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(txt) || _cliente == null) return;
            _cliente.Enviar($"CHAT|{_nombre}|{txt}");
            txtChatInput.Clear();
            txtChatInput.Focus();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show("¿Deseas salir de la partida?", "Salir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
                OnSolicitarSalida?.Invoke();
        }
        private void btnPausarTimer_Click(object sender, EventArgs e)
        {
            if (!_esAnfitrion) return;
            if (_timerPausado)
            {
                _timerCantor?.Start();
                _timerPausado = false;
                btnPausarTimer.Text      = "⏸ Pausar";
                btnPausarTimer.BackColor = Color.FromArgb(55, 85, 130);
                MostrarEnHistorial("[Admin] Gritón reanudado.");
            }
            else
            {
                _timerCantor?.Stop();
                _timerPausado = true;
                btnPausarTimer.Text      = "▶ Reanudar";
                btnPausarTimer.BackColor = Color.FromArgb(40, 120, 60);
                MostrarEnHistorial("[Admin] Gritón pausado.");
            }
        }

        private void btnAumentarVelocidad_Click(object sender, EventArgs e)
        {
            if (!_esAnfitrion) return;
            _intervaloTimer -= 1_000;
            if (_intervaloTimer < 1_000) _intervaloTimer = 5_000;
            ActualizarLabelVelocidad();
            if (_timerCantor != null && !_timerPausado)
            {
                _timerCantor.Stop();
                _timerCantor.Interval = _intervaloTimer;
                _timerCantor.Start();
            }
            MostrarEnHistorial($"[Admin] Velocidad ajustada: {_intervaloTimer / 1000}s entre cartas.");
        }

        private void ActualizarLabelVelocidad() =>
            lblVelocidadActual.Text = $"Vel: {_intervaloTimer / 1000}s";
        private void EnviarReglas()
        {
            if (!_esAnfitrion || _cliente == null) return;
            string msg = $"REGLAS|{B(chkHorizontal.Checked)}|{B(chkVertical.Checked)}" +
                         $"|{B(chkDiagonal.Checked)}|{B(chkEsquinas.Checked)}|{B(chkPoyaCruz.Checked)}";
            _cliente.Enviar(msg);
        }

        private static string B(bool v) => v ? "1" : "0";

        private void AplicarReglas(string[] p)
        {
            if (p.Length < 6) return;
            ValidadorVictoria.ReglaHorizontal = p[1] == "1";
            ValidadorVictoria.ReglaVertical   = p[2] == "1";
            ValidadorVictoria.ReglaDiagonal   = p[3] == "1";
            ValidadorVictoria.ReglaEsquinas   = p[4] == "1";
            ValidadorVictoria.ReglaPoyaCruz   = p[5] == "1";

            HabilitarControlesReglas(false);
            chkHorizontal.CheckedChanged -= (s, e) => EnviarReglas();
            chkVertical.CheckedChanged   -= (s, e) => EnviarReglas();
            chkDiagonal.CheckedChanged   -= (s, e) => EnviarReglas();
            chkEsquinas.CheckedChanged   -= (s, e) => EnviarReglas();
            chkPoyaCruz.CheckedChanged   -= (s, e) => EnviarReglas();

            chkHorizontal.Checked = ValidadorVictoria.ReglaHorizontal;
            chkVertical.Checked   = ValidadorVictoria.ReglaVertical;
            chkDiagonal.Checked   = ValidadorVictoria.ReglaDiagonal;
            chkEsquinas.Checked   = ValidadorVictoria.ReglaEsquinas;
            chkPoyaCruz.Checked   = ValidadorVictoria.ReglaPoyaCruz;

            if (_esAnfitrion) HabilitarControlesReglas(true);
        }

        private void HabilitarControlesReglas(bool habilitado)
        {
            chkHorizontal.Enabled = habilitado;
            chkVertical.Enabled   = habilitado;
            chkDiagonal.Enabled   = habilitado;
            chkEsquinas.Enabled   = habilitado;
            chkPoyaCruz.Enabled   = habilitado;
        }
        private void ProcesarMensaje(string mensaje)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                try { BeginInvoke(new Action<string>(ProcesarMensaje), mensaje); }
                catch { }
                return;
            }

            string[] p = mensaje.Split('|');
            switch (p[0])
            {
                case "MODO_DOBLE":    ProcesarModoDoble(p);      break;
                case "CARTA":         ProcesarCarta(p);         break;
                case "CHAT":          ProcesarChat(p);           break;
                case "GANADOR":       ProcesarGanador(p);        break;
                case "NUEVA_PARTIDA": ProcesarNuevaPartida();    break;
                case "REGLAS":        AplicarReglas(p);          break;
                case "JUGADORES":     ProcesarJugadores(p);      break;
                case "PERDEDOR":      ProcesarPerdedor(p);       break;
                case "TIE":           ProcesarEmpate(p);         break;
                case "RONDA_DESEMPATE": ProcesarRondaDesempate(p); break;
            }
        }

        private void ProcesarModoDoble(string[] p)
        {
            if (p.Length < 2 || !int.TryParse(p[1], out int id)) return;
            if (id == 0) _cartaDobleActual = null;
            else _cartaDobleActual = id;
            if (_cartaDobleActual.HasValue)
            {
                string nombre = _nombresCartas.TryGetValue(_cartaDobleActual.Value, out string n) ? n : $"carta {_cartaDobleActual.Value}";
                MostrarEnHistorial($"⭐ MODO CARTAS DOBLES ACTIVADO ⭐");
                MostrarEnHistorial($"Carta duplicada en todos los tableros: {nombre}");
            }
        }

        private void ProcesarCarta(string[] p)
        {
            if (p.Length < 2 || !int.TryParse(p[1], out int id)) return;

            string nombre = _nombresCartas.TryGetValue(id, out string n) ? n : $"carta {id}";
            _voz.AnunciarCarta(nombre);

            Image imgGrande    = GestorArchivos.CargarImagen(id);
            Image imgMiniatura = GestorArchivos.CargarImagen(id);

            Image anterior = picCartaActual.Image;
            picCartaActual.Image = imgGrande;
            anterior?.Dispose();

            AgregarMiniaturaHistorial(imgMiniatura);
            MostrarEnHistorial($"#{id} — {nombre}");

            foreach (var tablero in _tableros)
            {
                tablero.CartasCantadas.Add(id);
            }
        }

        private void ProcesarChat(string[] p)
        {
            if (p.Length < 3) return;
            MostrarEnHistorial($"[{p[1]}]: {p[2]}");
        }

        private void ProcesarGanador(string[] p)
        {
            if (p.Length < 3) return;
            string ganador = p[1];
            string figura  = p[2];

            _partidaTerminada = true;
            CongelarJuego();
            MostrarEnHistorial($" {ganador} ganó con {figura}.");
            _voz.AnunciarCarta($"¡Lotería! {ganador} ganó");

            _rondaActual++;
            listScorecard.Items.Insert(0, $"Ronda {_rondaActual}: {ganador} ({figura})");

            var timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += (s, e) =>
            {
                timer.Stop(); timer.Dispose();
                if (!IsDisposed)
                    OnPartidaTerminada?.Invoke(ganador, figura);
            };
            timer.Start();
        }

        private void ProcesarNuevaPartida()
        {
            MostrarEnHistorial("=== El anfitrión inició una nueva partida ===");
            _partidaTerminada  = false;
            _descalificado     = false;
            _advertenciasLoteria = 0;
            btnGritarLoteria.Enabled  = true;
            btnGritarLoteria.Text     = "¡ L O T E R Í A !";
            btnGritarLoteria.BackColor = Color.FromArgb(160, 30, 30);
            btnEnviar.Enabled         = true;
            txtChatInput.Enabled      = true;
            foreach (var tablero in _tableros) tablero.GenerarAleatorio(_cartaDobleActual);
            ConstruirTablas();
            OnNuevaPartidaRecibida?.Invoke();
            _jugadoresEnDesempate = null;
        }

        private void ProcesarJugadores(string[] p)
        {
            if (p.Length < 2) return;
            listJugadores.Items.Clear();
            if (!string.IsNullOrWhiteSpace(p[1]))
                foreach (var nombre in p[1].Split(','))
                    listJugadores.Items.Add(nombre);
        }

        private void ProcesarPerdedor(string[] p)
        {
            if (p.Length < 3) return;
            MostrarEnHistorial($" {p[1]} fue descalificado: {p[2]}");
        }

        private void ProcesarEmpate(string[] p)
        {
            _partidaTerminada = true;
            CongelarJuego();
            MostrarEnHistorial(" ¡EMPATE detectado!");
            _voz.AnunciarCarta("Empate detectado.");

            if (p.Length < 2) return;

            var candidatos = p[1].Split(';');
            var nombresEmp = candidatos.Select(c => c.Split('~')[0]).ToList();
            var figurasEmp = candidatos.Select(c => c.Split('~').Length > 1 ? c.Split('~')[1] : "?").ToList();
            var idsGanPorCandidato = candidatos.Select(c =>
            {
                var partes = c.Split('~');
                if (partes.Length < 4) return new List<int>();
                return partes[3].Split(',')
                    .Select(s => int.TryParse(s, out int v) ? v : -1)
                    .Where(v => v > 0)
                    .ToList();
            }).ToList();

            if (_esAnfitrion)
            {
                MostrarEnHistorial($" Empataron: {string.Join(" vs ", nombresEmp)}");
                MostrarDialogoDesempate(nombresEmp, figurasEmp, idsGanPorCandidato, candidatos);
            }
            else
            {
                MostrarEnHistorial($" Empataron: {string.Join(" vs ", nombresEmp)}. El anfitrión decidirá el desempate.");
            }
        }
        private void MostrarDialogoDesempate(
         List<string> nombres,
         List<string> figuras,
         List<List<int>> idsGanPorCandidato,
              string[] candidatosRaw)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("¿Cómo deseas resolver el empate?\n");
            for (int i = 0; i < nombres.Count; i++)
                sb.AppendLine($"  • {nombres[i]} — {figuras[i]}");
            sb.AppendLine();
            sb.AppendLine("[ Sí ]     → Carta mayor  (gana quien tenga la carta con ID más alto)");
            sb.AppendLine("[ No ]     → Ronda extra  (solo los empatados juegan otra ronda)");
            sb.AppendLine("[ Cancel ] → Cancelar (volver a elegir)");

            var resp = MessageBox.Show(sb.ToString(), "Desempate — Elige método",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (resp == DialogResult.Cancel) return;

            if (resp == DialogResult.Yes)
            {
                ResolverPorCartaMayor(nombres, figuras, idsGanPorCandidato);
            }
            else
            {
                IniciarRondaDesempate(nombres, candidatosRaw);
            }
        }

        private void ResolverPorCartaMayor(
    List<string> nombres,
    List<string> figuras,
    List<List<int>> idsGanPorCandidato)
        {
            int ganadorIdx = 0;
            for (int i = 1; i < idsGanPorCandidato.Count; i++)
            {
                var listaA = idsGanPorCandidato[ganadorIdx].OrderByDescending(x => x).ToList();
                var listaB = idsGanPorCandidato[i].OrderByDescending(x => x).ToList();

                int cmp = 0;
                for (int k = 0; k < Math.Min(listaA.Count, listaB.Count); k++)
                {
                    if (listaA[k] > listaB[k]) { cmp = -1; break; }
                    if (listaA[k] < listaB[k]) { cmp = 1; break; }
                }
                if (cmp == 0 && listaB.Count > listaA.Count) cmp = 1;
                if (cmp > 0) ganadorIdx = i;
            }

            string ganador = nombres[ganadorIdx];
            string figura = figuras[ganadorIdx];
            MostrarEnHistorial($" Desempate por carta mayor: {ganador} gana con {figura}.");
            _servidor.Transmitir($"GANADOR|{ganador}|{figura} (carta mayor)");
        }
        private void IniciarTimerCantor()
        {
            _timerCantor?.Stop();
            _timerCantor?.Dispose();
            _timerCantor = new System.Windows.Forms.Timer { Interval = _intervaloTimer };
            _timerCantor.Tick += (s, e) => CantarSiguienteCarta();
        }

        private void CantarSiguienteCarta()
        {
            if (IsDisposed) return;
            if (_partidaTerminada || _mazo == null || _mazo.EstaAgotado)
            {
                _timerCantor?.Stop();
                MostrarEnHistorial("El mazo se agotó. Fin de la partida.");
                return;
            }
            _servidor.Transmitir($"CARTA|{_mazo.SacarCarta().Id}");
            if (chkAutoCantar.Checked && !_timerPausado)
            {
                IniciarTimerCantor();
                _timerCantor.Start();
            }
        }
        private void ConstruirTablas()
        {
            foreach (Control c in panelTablas.Controls)
            {
                if (c is GroupBox gb)
                    foreach (Control inner in gb.Controls)
                        if (inner is PictureBox pb) { pb.Image = null; }
            }
            panelTablas.Controls.Clear();
            _ctrlTablas.Clear();

            int total = Math.Min(_tableros.Count, 6); 
            panelTablas.FlowDirection = FlowDirection.LeftToRight;
            panelTablas.WrapContents = total > 2;

            for (int i = 0; i < total; i++)
            {
                var ctrl = new ControladorTablero(_tableros[i], i, total);
                _ctrlTablas.Add(ctrl);
                var gb = ctrl.ConstruirGrupBox();
                if (total >= 3)
                {
                    int porFila = (int)Math.Ceiling(total / 2.0);
                    if (i > 0 && i % porFila == 0)
                    {
                        var spacer = new Panel
                        {
                            Width = panelTablas.ClientSize.Width,
                            Height = 0,
                            BackColor = Color.Transparent
                        };
                        panelTablas.Controls.Add(spacer);
                    }
                }

                panelTablas.Controls.Add(gb);
            }
        }
        private void IniciarRondaDesempate(List<string> nombresEmpatados, string[] candidatosRaw)
        {
            _jugadoresEnDesempate = nombresEmpatados;
            string nombresStr = string.Join(",", nombresEmpatados);
            _servidor.Transmitir($"RONDA_DESEMPATE|{nombresStr}");
            _partidaTerminada = false;
            _descalificado = false;
            _advertenciasLoteria = 0;
            btnGritarLoteria.Text = "¡ L O T E R Í A !";
            btnGritarLoteria.BackColor = Color.FromArgb(160, 30, 30);
            if (_mazo != null && !_mazo.EstaAgotado)
            {
                btnAccionRed.Enabled = true;
                btnEnviar.Enabled = true;
                txtChatInput.Enabled = true;
            }
            btnGritarLoteria.Enabled = nombresEmpatados.Contains(_nombre);
            foreach (var t in _tableros) t.ReiniciarTapas();
            foreach (var ctrl in _ctrlTablas) ctrl.RefrescarVisual();

            MostrarEnHistorial($"🔁 RONDA DE DESEMPATE — Solo pueden ganar: {string.Join(", ", nombresEmpatados)}");
        }
        private void ProcesarRondaDesempate(string[] p)
        {
            if (p.Length < 2) return;
            var empatados = p[1].Split(',').ToList();
            _jugadoresEnDesempate = empatados;

            _partidaTerminada = false;
            _descalificado = false;
            _advertenciasLoteria = 0;
            btnGritarLoteria.Text = "¡ L O T E R Í A !";
            btnGritarLoteria.BackColor = Color.FromArgb(160, 30, 30);
            btnEnviar.Enabled = true;
            txtChatInput.Enabled = true;
            bool puedeJugar = empatados.Contains(_nombre);
            btnGritarLoteria.Enabled = puedeJugar;

            foreach (var t in _tableros) t.ReiniciarTapas();
            foreach (var ctrl in _ctrlTablas) ctrl.RefrescarVisual();

            string msg = puedeJugar
                ? " RONDA DE DESEMPATE — ¡Tú estás en el desempate! Sigue marcando cartas."
                : " RONDA DE DESEMPATE — Solo pueden ganar: " + string.Join(", ", empatados);
            MostrarEnHistorial(msg);
        }
        private void MostrarEnHistorial(string linea)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                try { BeginInvoke(new Action<string>(MostrarEnHistorial), linea); }
                catch { }
                return;
            }
            txtHistorialChat.AppendText($"[{DateTime.Now:HH:mm:ss}] {linea}{Environment.NewLine}");
        }

        private void CongelarJuego()
        {
            btnGritarLoteria.Enabled = false;
            btnEnviar.Enabled        = false;
            txtChatInput.Enabled     = false;
            _timerCantor?.Stop();
        }

        private void AgregarMiniaturaHistorial(Image img)
        {
            if (img == null) return;
            var pic = new PictureBox
            {
                Size      = new Size(58, 80),
                SizeMode  = PictureBoxSizeMode.Zoom,
                Image     = img,
                Margin    = new Padding(3, 2, 3, 2),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(32, 32, 36)
            };
            panelHistorialCartas.Controls.Add(pic);
            panelHistorialCartas.ScrollControlIntoView(pic);
        }

        private void PrecargarNombresCartas()
        {
            try
            {
                foreach (var carta in CatalogoCartas.Todas)
                    _nombresCartas[carta.Id] = carta.Nombre;
            }
            catch { }
        }

        public void LiberarRecursos()
        {
            _timerCantor?.Stop();
            _timerCantor?.Dispose();
            _voz.Dispose();
            try { _cliente?.Desconectar(); } catch { }
            try { _servidor?.Detener(); } catch { }
            foreach (Control c in panelHistorialCartas.Controls)
                if (c is PictureBox p) p.Image?.Dispose();
        }
    }
}