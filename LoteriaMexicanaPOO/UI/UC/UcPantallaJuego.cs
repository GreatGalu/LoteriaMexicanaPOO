using System;
using System.Collections.Generic;
using System.Drawing;
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
        private ClienteLoteria _cliente;
        private ServidorLoteria _servidor;
        private Mazo _mazo;
        private System.Windows.Forms.Timer _timerCantor;
        private readonly GestorVoz _voz = new GestorVoz();
        private readonly List<Tablero> _tableros = new List<Tablero>();
        private readonly List<ControladorTablero> _ctrlTablas = new List<ControladorTablero>();
        private string _nombre = "Jugador";
        private bool _esAnfitrion = false;
        private bool _partidaTerminada = false;

        private readonly Dictionary<int, string> _nombresCartas = new Dictionary<int, string>();
        public UcPantallaJuego()
        {
            InitializeComponent();
            PrecargarNombresCartas();
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
            _cliente.OnMensajeRecibido += ProcesarMensaje;
            _cliente.OnError += msg => MostrarEnHistorial($"[ERROR RED] {msg}");
            _cliente.OnDesconectado += () => MostrarEnHistorial("Desconectado del servidor.");
        }

        public void AsignarServidor(ServidorLoteria servidor, Mazo mazo)
        {
            _servidor = servidor;
            _mazo = mazo;
            _esAnfitrion = true;
            IniciarTimerCantor();

            btnAccionRed.Text = "Siguiente ▶";
            btnAccionRed.Click -= btnAccionRed_ClickCrear;
            btnAccionRed.Click += (s, e) => CantarSiguienteCarta();
            btnUnirse.Enabled = false;
        }

        public void MostrarCodigoSala(string codigo)
        {
            lblEstado.Text = $"SALA: {codigo}  |  Esperando jugadores...";
            txtSala.Text = codigo;
            btnAccionRed.Text = "Siguiente ▶";
        }

        public void MostrarConectado(string sala)
        {
            lblEstado.Text = $"Conectado a la sala: {sala}";
            btnAccionRed.Enabled = false;
            btnUnirse.Enabled = false;
        }
        public void ReiniciarPartida(Mazo mazoNuevo)
        {
            _mazo = mazoNuevo;
            _partidaTerminada = false;

            btnGritarLoteria.Enabled = true;
            btnEnviar.Enabled = true;
            txtChatInput.Enabled = true;
           var miniaturas = new System.Collections.Generic.List<PictureBox>();
            foreach (Control c in panelHistorialCartas.Controls)
                if (c is PictureBox pb) miniaturas.Add(pb);

            foreach (var pb in miniaturas)
            {
                pb.Image?.Dispose();
                panelHistorialCartas.Controls.Remove(pb);
                pb.Dispose();
            }

            picCartaActual.Image?.Dispose();
            picCartaActual.Image = null;

            ConstruirTablas();

            if (_esAnfitrion)
                IniciarTimerCantor();

            MostrarEnHistorial("=== Nueva partida iniciada ===");
        }

        public void SolicitarSalida() => btnSalir_Click(null, null);
        private void btnAccionRed_ClickCrear(object sender, EventArgs e) { }

        private void btnGritarLoteria_Click(object sender, EventArgs e)
        {
            if (_partidaTerminada || _cliente == null) return;

            for (int i = 0; i < _tableros.Count; i++)
            {
                var detalle = ValidadorVictoria.EvaluarConValidacion(
                    _tableros[i].Tapas,
                    _tableros[i].Casillas,
                    _tableros[i].CartasCantadas);

                switch (detalle.Resultado)
                {
                    case ValidadorVictoria.ResultadoValidacion.Victoria:
                        _cliente.Enviar($"GANADOR|{_nombre}|{detalle.Figura} (Tabla {i + 1})");
                        return;

                    case ValidadorVictoria.ResultadoValidacion.Trampa:
                        string ids = string.Join(", #", detalle.CartasTrampa);
                        MessageBox.Show(
                            $"Tienes fichas en casillas cuyas cartas aún no han salido.\n\n" +
                            $"Cartas no cantadas tapadas: #{ids}\n\n" +
                            "Quita esas fichas y espera a que sean cantadas.",
                            "Fichas inválidas",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }
            }

            MessageBox.Show(
                "Aún no completas ninguna figura.\n\n" +
                "Formas de ganar:\n" +
                "  · Línea horizontal  — cualquier fila completa (5 tapas)\n" +
                "  · Línea vertical    — cualquier columna completa (4 tapas)\n" +
                "  · Diagonal          — diagonal de 4 celdas\n" +
                "  · Esquinas          — las 4 esquinas\n" +
                "  · Poya / Cruz       — fila central + columna central\n\n" +
                "Recuerda: haz clic en la casilla para poner la ficha.",
                "¡Lotería!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                case "CARTA": ProcesarCarta(p); break;
                case "CHAT": ProcesarChat(p); break;
                case "GANADOR": ProcesarGanador(p); break;
            }
        }

        private void ProcesarCarta(string[] p)
        {
            if (p.Length < 2 || !int.TryParse(p[1], out int id)) return;

            string nombre = _nombresCartas.TryGetValue(id, out string n) ? n : $"carta {id}";
            _voz.AnunciarCarta(nombre);

            Image imgGrande = GestorArchivos.CargarImagen(id);
            Image imgMiniatura = GestorArchivos.CargarImagen(id);

            Image anterior = picCartaActual.Image;
            picCartaActual.Image = imgGrande;
            anterior?.Dispose();

            AgregarMiniaturaHistorial(imgMiniatura);
            MostrarEnHistorial($"#{id} — {nombre}");

            for (int t = 0; t < _tableros.Count; t++)
            {
                if (!_tableros[t].CartasCantadas.Contains(id))
                    _tableros[t].CartasCantadas.Add(id);

                var (fila, col) = _tableros[t].BuscarId(id);
                if (fila != -1)
                    _ctrlTablas[t].MarcarCartaCantada(fila, col);
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
            string figura = p[2];

            _partidaTerminada = true;
            CongelarJuego();
            MostrarEnHistorial($"🏆 {ganador} ganó con {figura}.");
            _voz.AnunciarCarta($"¡Lotería! {ganador} ganó");

            var timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                timer.Dispose();
                if (!IsDisposed)
                    OnPartidaTerminada?.Invoke(ganador, figura);
            };
            timer.Start();
        }
        private void IniciarTimerCantor()
        {
            _timerCantor?.Stop();
            _timerCantor?.Dispose();
            _timerCantor = new System.Windows.Forms.Timer { Interval = 10_000 };
            _timerCantor.Tick += (s, e) => CantarSiguienteCarta();
            _timerCantor.Start();
        }

        private void CantarSiguienteCarta()
        {
            if (IsDisposed) return;
            if (_partidaTerminada || _mazo == null || _mazo.EstaAgotado)
            {
                _timerCantor?.Stop();
                MostrarEnHistorial("El mazo se agotó.");
                return;
            }
            _timerCantor?.Stop();
            _timerCantor?.Start();
            _servidor.Transmitir($"CARTA|{_mazo.SacarCarta().Id}");
        }
        private void ConstruirTablas()
        {
            panelTablas.Controls.Clear();
            _ctrlTablas.Clear();
            for (int i = 0; i < _tableros.Count; i++)
            {
                var ctrl = new ControladorTablero(_tableros[i], i);
                _ctrlTablas.Add(ctrl);
                panelTablas.Controls.Add(ctrl.ConstruirGrupBox());
            }
        }

        private void MostrarEnHistorial(string linea) =>
            txtHistorialChat.AppendText($"[{DateTime.Now:HH:mm:ss}] {linea}{Environment.NewLine}");

        private void CongelarJuego()
        {
            btnGritarLoteria.Enabled = false;
            btnEnviar.Enabled = false;
            txtChatInput.Enabled = false;
            _timerCantor?.Stop();
        }

        private void AgregarMiniaturaHistorial(Image img)
        {
            if (img == null) return;
            var pic = new PictureBox
            {
                Size = new Size(58, 80),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = img,
                Margin = new Padding(3, 2, 3, 2),
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