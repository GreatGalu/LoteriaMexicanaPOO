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

        private ClienteLoteria                    _cliente;
        private ServidorLoteria                   _servidor;
        private Mazo                              _mazo;
        private System.Windows.Forms.Timer        _timerCantor;
        private readonly GestorAudio              _audio = new GestorAudio();

        private readonly List<Tablero>            _tableros   = new List<Tablero>();
        private readonly List<ControladorTablero> _ctrlTablas = new List<ControladorTablero>();

        private string _nombre           = "Jugador";
        private bool   _partidaTerminada = false;

        public UcPantallaJuego()
        {
            InitializeComponent();
        }



        public void Configurar(string nombre, List<Tablero> tableros)
        {
            _nombre = nombre;
            _tableros.Clear();
            _tableros.AddRange(tableros);
            ConstruirTablas();
        }

        /// <summary>Conecta el cliente de red y suscribe el procesador de mensajes.</summary>
        public void AsignarCliente(ClienteLoteria cliente)
        {
            _cliente = cliente;
            _cliente.OnMensajeRecibido += ProcesarMensaje;
            _cliente.OnError           += msg => MostrarEnHistorial($"[ERROR RED] {msg}");
            _cliente.OnDesconectado    += ()  => MostrarEnHistorial("Desconectado del servidor.");
        }

        /// <summary>
        /// Asigna el servidor y mazo (solo lo llama el Anfitri­on).
        /// Inicia el timer automatico de cartas.
        /// </summary>
        public void AsignarServidor(ServidorLoteria servidor, Mazo mazo)
        {
            _servidor = servidor;
            _mazo     = mazo;
            IniciarTimerCantor();

            // Cambiar boton "Crear" → "Siguiente carta"
            btnAccionRed.Text   = "Siguiente ▶";
            btnAccionRed.Click -= btnAccionRed_ClickCrear;
            btnAccionRed.Click += (s, e) => CantarSiguienteCarta();
            btnUnirse.Enabled   = false;
        }

        public void MostrarCodigoSala(string codigo)
        {
            lblEstado.Text     = $"SALA: {codigo}  |  Esperando jugadores...";
            txtSala.Text       = codigo;
            btnAccionRed.Text  = "Siguiente ▶";
        }

        public void MostrarConectado(string sala)
        {
            lblEstado.Text      = $"Conectado a la sala: {sala}";
            btnAccionRed.Enabled = false;
            btnUnirse.Enabled    = false;
        }

        private void btnAccionRed_ClickCrear(object sender, EventArgs e) { }
        private void btnGritarLoteria_Click(object sender, EventArgs e)
        {
            if (_partidaTerminada || _cliente == null) return;

            // ────────────────────────────────────────────────────────────
            // FIX DEL VALIDADOR:
            // Se accede a _tableros[i].Tapas que es la referencia DIRECTA
            // al mismo objeto Tablero que uso ControladorTablero.
            // AlternarTapa() ya actualizo Tapas[fila,col] = true cuando el
            // jugador hizo clic en la celda. EvaluarTodo() lee esa matriz.
            // ────────────────────────────────────────────────────────────
            for (int i = 0; i < _tableros.Count; i++)
            {
                bool[,] tapas = _tableros[i].Tapas;   // referencia directa, no copia
                string resultado = ValidadorVictoria.EvaluarTodo(tapas);

                if (resultado != null)
                {
                    _cliente.Enviar($"GANADOR|{_nombre}|{resultado} (Tabla {i + 1})");
                    return;
                }
            }

            // Ninguna figura valida: mostrar pista
            MessageBox.Show(
                "Aun no completas ninguna figura.\n\n" +
                "Recuerda:\n" +
                "• Haz CLIC en la casilla cuando la carta sea cantada para poner la tapa.\n\n" +
                "Formas de ganar:\n" +
                "  · Linea horizontal  — cualquier fila completa (5 tapas)\n" +
                "  · Linea vertical    — cualquier columna completa (4 tapas)\n" +
                "  · Diagonal          — diagonal principal o secundaria (4 tapas)\n" +
                "  · Esquinas          — las 4 esquinas\n" +
                "  · Poya / Cruz       — fila central + columna del centro",
                "¡Lotería!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // ====================================================================
        //  PROCESAMIENTO DE MENSAJES DE RED
        // ====================================================================

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
                case "CARTA":   ProcesarCarta(p);   break;
                case "CHAT":    ProcesarChat(p);     break;
                case "GANADOR": ProcesarGanador(p);  break;
            }
        }

        private void ProcesarCarta(string[] p)
        {
            if (p.Length < 2 || !int.TryParse(p[1], out int id)) return;

            _audio.ReproducirCarta(id);

            // Dos instancias independientes para evitar el Dispose cruzado
            Image imgGrande    = GestorArchivos.CargarImagen(id);
            Image imgMiniatura = GestorArchivos.CargarImagen(id);

            Image anterior = picCartaActual.Image;
            picCartaActual.Image = imgGrande;
            anterior?.Dispose();

            AgregarMiniaturaHistorial(imgMiniatura);
            MostrarEnHistorial($"Carta cantada: #{id}");

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
            _partidaTerminada = true;
            CongelarJuego();
            MessageBox.Show($"¡LOTERÍA!\n\n{p[1]} ganó con: {p[2]}",
                "¡Fin de la partida!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MostrarEnHistorial($"🏆 {p[1]} gano con {p[2]}.");
        }



        private void IniciarTimerCantor()
        {
            _timerCantor       = new System.Windows.Forms.Timer { Interval = 10_000 };
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
            btnEnviar.Enabled        = false;
            txtChatInput.Enabled     = false;
            _timerCantor?.Stop();
        }

        private void AgregarMiniaturaHistorial(Image img)
        {
            if (img == null) return;
            var pic = new PictureBox
            {
                Size        = new Size(58, 80),
                SizeMode    = PictureBoxSizeMode.Zoom,
                Image       = img,
                Margin      = new Padding(3, 2, 3, 2),
                BorderStyle = BorderStyle.None,
                BackColor   = Color.FromArgb(32, 32, 36)
            };
            panelHistorialCartas.Controls.Add(pic);
            panelHistorialCartas.ScrollControlIntoView(pic);
        }

        // ====================================================================
        //  LIMPIEZA DE RECURSOS
        // ====================================================================

        public void LiberarRecursos()
        {
            _timerCantor?.Stop();
            _timerCantor?.Dispose();
            _audio.Dispose();
            try { _cliente?.Desconectar(); } catch { }
            try { _servidor?.Detener();    } catch { }
            foreach (Control c in panelHistorialCartas.Controls)
                if (c is PictureBox p) p.Image?.Dispose();
        }
    }
}
