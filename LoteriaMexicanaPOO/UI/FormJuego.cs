using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LoteriaMexicana.Controllers;
using LoteriaMexicana.Core;
using LoteriaMexicana.Logic;
using LoteriaMexicana.Network;

namespace LoteriaMexicana.UI
{
    public partial class FormJuego : Form
    {
        private readonly GestorAudio _audio = new GestorAudio();
        private ClienteLoteria _cliente;
        private ServidorLoteria _servidor;
        private Mazo _mazo;
        private System.Windows.Forms.Timer _timerCantor;
        private readonly List<Tablero> _tableros = new List<Tablero>();
        private readonly List<ControladorTablero> _ctrlTablas = new List<ControladorTablero>();

        private string _nombre = "Jugador";
        private int _cantidadTablas = 1;
        private bool _partidaTerminada = false;

        private bool _cerrandoIntencional = false;
        public FormJuego()
        {
            PedirDatosIniciales();
            InitializeComponent();
            AplicarTema();
            ConstruirPanelTablas();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            KeyPreview = true;
        }
        private void PedirDatosIniciales()
        {
            string nom = Microsoft.VisualBasic.Interaction.InputBox(
                "Ingresa tu nombre de jugador:", "Loteria Mexicana", "Jugador1");
            if (!string.IsNullOrWhiteSpace(nom)) _nombre = nom.Trim();

            string sel = Microsoft.VisualBasic.Interaction.InputBox(
                "Con cuantas tablas deseas jugar? (1, 2 o 3):", "Tablas", "1");
            if (!int.TryParse(sel, out _cantidadTablas) || _cantidadTablas < 1 || _cantidadTablas > 3)
                _cantidadTablas = 1;

            for (int i = 0; i < _cantidadTablas; i++)
                _tableros.Add(PedirTablero(i + 1));
        }

        private Tablero PedirTablero(int numero)
        {
            var resp = MessageBox.Show(
                $"Deseas crear la Tabla {numero} de forma personalizada?\n\n" +
                "Si = Elige tus 20 cartas\nNo = Tabla aleatoria",
                $"Tabla {numero}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                using var frm = new FormCrearTabla();
                if (frm.ShowDialog() == DialogResult.OK && frm.TableroResultante != null)
                    return frm.TableroResultante;
            }

            var t = new Tablero();
            t.GenerarAleatorio();
            return t;
        }

        private void ConstruirPanelTablas()
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
        private void AplicarTema()
        {
            BackColor = Color.FromArgb(83, 143, 143);

            panelControl.BackColor = Color.FromArgb(56, 56, 56);
            panelTablas.BackColor = Color.FromArgb(68, 68, 68);
            panelHistorialCartas.BackColor = Color.FromArgb(40, 40, 40);

            lblEstado.ForeColor = Color.FromArgb(220, 220, 220);
            lblEstado.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            txtIpServidor.BackColor = Color.FromArgb(60, 60, 60);
            txtIpServidor.ForeColor = Color.White;

            EstilizarBoton(btnCrearPartida, Color.FromArgb(90, 90, 90));
            EstilizarBoton(btnUnirse, Color.FromArgb(90, 90, 90));
            EstilizarBoton(btnEnviar, Color.FromArgb(80, 80, 80));
            EstilizarBoton(btnGritarLoteria, Color.FromArgb(180, 40, 40));
            EstilizarBoton(btnSalir, Color.FromArgb(70, 70, 70));

            btnGritarLoteria.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            btnSalir.Font = new Font("Segoe UI", 8);

            picCartaActual.BackColor = Color.FromArgb(30, 30, 30);

            txtHistorialChat.BackColor = Color.FromArgb(44, 44, 44);
            txtHistorialChat.ForeColor = Color.FromArgb(202, 202, 202);
            txtHistorialChat.Font = new Font("Consolas", 9);
            txtChatInput.BackColor = Color.FromArgb(60, 60, 60);
            txtChatInput.ForeColor = Color.White;
        }

        private static void EstilizarBoton(Button b, Color fondo)
        {
            b.BackColor = fondo;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
            b.Cursor = Cursors.Hand;
        }
        private void btnCrearPartida_Click(object sender, EventArgs e)
        {
            try
            {
                _servidor = new ServidorLoteria();
                _servidor.OnError += msg => MostrarEnHistorial($"[ERROR SERVIDOR] {msg}");
                _servidor.OnClienteConectado += ip => MostrarEnHistorial($"Jugador conectado desde {ip}");
                _servidor.Iniciar();

                _mazo = new Mazo();
                _mazo.Barajar();
                IniciarTimerCantor();

                string ip = ObtenerIpLocal();
                string codigo = IpACodigo(ip);
                ConectarComoCliente("127.0.0.1");

                ActualizarEstado($"SALA: {codigo}  |  Esperando jugadores...");
                txtIpServidor.Text = codigo;

                btnCrearPartida.Text = "Siguiente";
                btnCrearPartida.Click -= btnCrearPartida_Click;
                btnCrearPartida.Click += (s, ev) => CantarSiguienteCarta();
                btnUnirse.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear partida:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUnirse_Click(object sender, EventArgs e)
        {
            string entrada = txtIpServidor.Text.Trim();
            if (string.IsNullOrWhiteSpace(entrada)) return;

            string ip = CodigoAIp(entrada);
            try
            {
                ConectarComoCliente(ip);
                ActualizarEstado($"Conectado a la sala: {entrada.ToUpper()}");
                btnCrearPartida.Enabled = false;
                btnUnirse.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo conectar:\n{ex.Message}",
                    "Error de conexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGritarLoteria_Click(object sender, EventArgs e)
        {
            if (_partidaTerminada || _cliente == null) return;

            for (int i = 0; i < _tableros.Count; i++)
            {
                string res = ValidadorVictoria.EvaluarTodo(_tableros[i].Tapas);
                if (res != null)
                {
                    _cliente.Enviar($"GANADOR|{_nombre}|{res} (Tabla {i + 1})");
                    return;
                }
            }
            MessageBox.Show(
                "Aun no completas ninguna figura valida.\n\n" +
                "Formas de ganar:\n" +
                "  Linea Horizontal — cualquier fila completa (5 tapas)\n" +
                "  Linea Vertical   — cualquier columna completa (4 tapas)\n" +
                "  Diagonal         — diagonal principal o secundaria (4 tapas)\n" +
                "  Esquinas         — las 4 esquinas\n" +
                "  Poya / Cruz      — fila central + columna central",
                "Loteria", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var r = MessageBox.Show(
                "Deseas salir de la partida?",
                "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
                CerrarLimpio();
        }
        private void ProcesarMensaje(string mensaje)
        {
            if (_cerrandoIntencional) return;

            if (InvokeRequired)
            {
                try { BeginInvoke(new Action<string>(ProcesarMensaje), mensaje); }
                catch {  }
                return;
            }

            if (_disposed) return;

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

            _audio.ReproducirCarta(id);

            Image imgGrande = GestorArchivos.CargarImagen(id);
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
            MessageBox.Show($"LOTERIA!\n\n{p[1]} gano con: {p[2]}",
                "Fin de la partida!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MostrarEnHistorial($"{p[1]} gano con {p[2]}.");
        }
        private void IniciarTimerCantor()
        {
            _timerCantor = new System.Windows.Forms.Timer { Interval = 10_000 };
            _timerCantor.Tick += (s, e) => CantarSiguienteCarta();
            _timerCantor.Start();
        }

        private void CantarSiguienteCarta()
        {
            if (_cerrandoIntencional) return;
            if (_partidaTerminada || _mazo == null || _mazo.EstaAgotado)
            {
                _timerCantor?.Stop();
                MostrarEnHistorial("El mazo se agoto. Fin de la partida.");
                return;
            }

            _timerCantor?.Stop();
            _timerCantor?.Start();

            var carta = _mazo.SacarCarta();
            _servidor.Transmitir($"CARTA|{carta.Id}");
        }
        private void MostrarEnHistorial(string linea) =>
            txtHistorialChat.AppendText($"[{DateTime.Now:HH:mm:ss}] {linea}{Environment.NewLine}");

        private void ActualizarEstado(string texto) => lblEstado.Text = texto;

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
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(40, 40, 40)
            };
            panelHistorialCartas.Controls.Add(pic);
            panelHistorialCartas.ScrollControlIntoView(pic);
        }
        private void ConectarComoCliente(string ip)
        {
            _cliente = new ClienteLoteria();
            _cliente.OnMensajeRecibido += ProcesarMensaje;
            _cliente.OnError += msg =>
            {
                if (!_cerrandoIntencional)
                    MostrarEnHistorial($"[ERROR RED] {msg}");
            };
            _cliente.OnDesconectado += () =>
            {
                if (!_cerrandoIntencional)
                    MostrarEnHistorial("Desconectado del servidor.");
            };
            _cliente.Conectar(ip);
        }
        private bool _disposed = false;

        private void CerrarLimpio()
        {
            if (_cerrandoIntencional) return;
            _cerrandoIntencional = true;

            _timerCantor?.Stop();
            _timerCantor?.Dispose();
            _timerCantor = null;

            _audio.Dispose();
            try { _cliente?.Desconectar(); } catch { }
            try { _servidor?.Detener(); } catch { }

            foreach (Control ctrl in panelHistorialCartas.Controls)
                if (ctrl is PictureBox pic) pic.Image?.Dispose();

            _disposed = true;
            Close();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
                btnSalir_Click(null, EventArgs.Empty);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_cerrandoIntencional)
            {
                base.OnFormClosing(e);
                return;
            }
            e.Cancel = true;
            btnSalir_Click(null, EventArgs.Empty);
        }
        private static string ObtenerIpLocal()
        {
            try
            {
                foreach (var ip in System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()))
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                        && !System.Net.IPAddress.IsLoopback(ip))
                        return ip.ToString();
            }
            catch { }
            return "127.0.0.1";
        }

        private static string IpACodigo(string ip)
        {
            try
            {
                var p = ip.Split('.');
                int s3 = int.Parse(p[2]), s4 = int.Parse(p[3]);
                return $"{(char)('A' + s3 / 26)}{(char)('A' + s3 % 26)}{(char)('A' + s4 / 26)}{(char)('A' + s4 % 26)}";
            }
            catch { return "SALA"; }
        }

        private static string CodigoAIp(string codigo)
        {
            try
            {
                string c = codigo.Trim().ToUpper();
                if (c.Length != 4) return codigo;
                int s3 = (c[0] - 'A') * 26 + (c[1] - 'A');
                int s4 = (c[2] - 'A') * 26 + (c[3] - 'A');
                var p = ObtenerIpLocal().Split('.');
                return $"{p[0]}.{p[1]}.{s3}.{s4}";
            }
            catch { return "127.0.0.1"; }
        }
    }
}