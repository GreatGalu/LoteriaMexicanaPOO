using LoteriaMexicana.Core;
using LoteriaMexicana.Logic;
using LoteriaMexicana.Models;
using LoteriaMexicana.Network;
using LoteriaMexicana.UI.UserControls;
using LoteriaMexicanaPOO.UI.UC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LoteriaMexicana.UI
{
    public partial class FormJuego : Form
    {
        private ClienteLoteria _cliente;
        private ServidorLoteria _servidor;
        private Mazo _mazo;
        private bool _esAnfitrion = false;
        private bool _cerrandoIntencional = false;
        private UcPantallaJuego _ucJuego;
        private List<Tablero> _tablerosActivos;
        private UcPostPartida _overlayPostPartida;

        public FormJuego()
        {
            InitializeComponent();
            VolverAlMenu();
        }
        private void MontarControl(UserControl uc)
        {
            contenedor.Controls.Clear();
            contenedor.Controls.Add(uc);
        }

        private void VolverAlMenu()
        {
            QuitarOverlayPostPartida();

            var ucInicio = new UcPantallaInicio();
            ucInicio.OnCrearPartida += FlujoCrearPartida;
            ucInicio.OnUnirse += FlujoUnirse;
            MontarControl(ucInicio);
        }
        private void FlujoCrearPartida(string nombre, int cantidadTablas)
        {
            _esAnfitrion = true;
            _tablerosActivos = RecolectarTableros(cantidadTablas);
            _ucJuego = ConstruirUcJuego(nombre, _tablerosActivos);
            MontarControl(_ucJuego);

            try
            {
                _servidor = new ServidorLoteria();
                _servidor.Iniciar();
                _mazo = new Mazo();
                _mazo.Barajar();
                _cliente = new ClienteLoteria();
                _ucJuego.AsignarCliente(_cliente);
                _cliente.Conectar("127.0.0.1", nombre);
                _ucJuego.AsignarServidor(_servidor, _mazo);
                _ucJuego.MostrarCodigoSala(IpACodigo(ObtenerIpLocal()));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear la partida:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                VolverAlMenu();
            }
        }
        private void FlujoUnirse(string nombre, int cantidadTablas, string entradaSala)
        {
            _esAnfitrion = false;
            _tablerosActivos = RecolectarTableros(cantidadTablas);
            _ucJuego = ConstruirUcJuego(nombre, _tablerosActivos);
            MontarControl(_ucJuego);

            string ip = CodigoAIp(entradaSala);
            try
            {
                _cliente = new ClienteLoteria();
                _ucJuego.AsignarCliente(_cliente);
                _cliente.Conectar(ip, nombre);
                _ucJuego.MostrarConectado(entradaSala.ToUpper());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo conectar:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                VolverAlMenu();
            }
        }
        private UcPantallaJuego ConstruirUcJuego(string nombre, List<Tablero> tableros)
        {
            var uc = new UcPantallaJuego();
            uc.Configurar(nombre, tableros);
            uc.OnSolicitarSalida += CerrarLimpio;
            uc.OnPartidaTerminada += MostrarOverlayPostPartida;
            uc.OnCrearFiguraSolicitado += () => AbrirEditorFigura(uc);
            uc.OnNuevaPartidaRecibida += () =>
            {
                if (InvokeRequired) { BeginInvoke(new Action(() => QuitarOverlayPostPartida())); return; }
                QuitarOverlayPostPartida();
            };

            return uc;
        }
        private void MostrarOverlayPostPartida(string ganador, string figura)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => MostrarOverlayPostPartida(ganador, figura)));
                return;
            }
            QuitarOverlayPostPartida();
            _overlayPostPartida = new UcPostPartida(ganador, figura, _esAnfitrion);
            _overlayPostPartida.Size = this.ClientSize;
            _overlayPostPartida.Location = new Point(0, 0);

            _overlayPostPartida.OnNuevaPartida += () =>
            {
                QuitarOverlayPostPartida();
                bool cartaDoble = _ucJuego.CartaDobleActiva();

                _servidor?.Transmitir(cartaDoble ? "MODO_DOBLE|1" : "MODO_DOBLE|0");

                if (_mazo != null) { _mazo.Reiniciar(); _mazo.Barajar(); }
                foreach (var t in _tablerosActivos) t.GenerarAleatorio(cartaDoble);
                _ucJuego?.ReiniciarPartida(_mazo);
                _servidor?.Transmitir("NUEVA_PARTIDA");
            };

            _overlayPostPartida.OnSalirAlMenu += () =>
            {
                QuitarOverlayPostPartida();
                CerrarLimpio();
            };
            this.Controls.Add(_overlayPostPartida);
            _overlayPostPartida.BringToFront();
            _overlayPostPartida.CentrarTarjetaPublico();
        }

        private void QuitarOverlayPostPartida()
        {
            if (_overlayPostPartida == null) return;
            this.Controls.Remove(_overlayPostPartida);
            _overlayPostPartida.Dispose();
            _overlayPostPartida = null;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_overlayPostPartida != null && !_overlayPostPartida.IsDisposed)
            {
                _overlayPostPartida.Size = this.ClientSize;
                _overlayPostPartida.CentrarTarjetaPublico();
            }
        }
        private List<Tablero> RecolectarTableros(int cantidad)
        {
            var lista = new List<Tablero>();
            for (int i = 0; i < cantidad; i++)
            {
                bool tablaValida = false;
                while (!tablaValida)
                {
                    var resp = MessageBox.Show(
                        $"Tabla {i + 1}: ¿Cómo deseas generar esta tabla?\n\nSí = Crear en Editor\nNo = Cargar desde JSON\nCancelar = Aleatoria",
                        $"Tabla {i + 1}", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    Tablero nuevaTabla = null;

                    if (resp == DialogResult.Yes)
                    {
                        nuevaTabla = CrearTableroConEditor(i + 1);
                    }
                    else if (resp == DialogResult.No)
                    {
                        nuevaTabla = CargarTableroDesdeArchivo();
                        if (nuevaTabla == null) nuevaTabla = CrearTableroAleatorio();
                    }
                    else
                    {
                        nuevaTabla = CrearTableroAleatorio();
                    }
                    bool esDuplicada = false;
                    foreach (var t in lista)
                    {
                        if (t.EsIdenticoA(nuevaTabla))
                        {
                            esDuplicada = true;
                            break;
                        }
                    }

                    if (esDuplicada)
                    {
                        if (resp == DialogResult.Yes || resp == DialogResult.No)
                        {
                            MessageBox.Show("Esta tabla es idéntica a una que ya agregaste. Por favor crea/carga una diferente.",
                                            "Tabla Duplicada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        lista.Add(nuevaTabla);
                        tablaValida = true;
                    }
                }
            }
            return lista;
        }

        private static Tablero CrearTableroAleatorio()
        {
            var t = new Tablero();
            t.GenerarAleatorio();
            return t;
        }

        private Tablero CargarTableroDesdeArchivo()
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Archivos JSON (*.json)|*.json",
                Title = "Cargar Tabla Guardada"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var t = new Tablero();
                if (t.CargarDesdeArchivo(dialog.FileName))
                {
                    return t;
                }
                MessageBox.Show("El archivo no tiene el formato correcto o no contiene la cantidad de cartas necesaria.",
                                "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return null;
        }

        private void AbrirEditorFigura(UcPantallaJuego ucJuego)
        {
            bool terminado = false;
            FiguraGanar resultado = null;

            var ucEditor = new UcCreadorFigura();
            ucEditor.OnFiguraConfirmada += fig => { resultado = fig; terminado = true; };
            ucEditor.OnCancelado += () => terminado = true;

            MontarControl(ucEditor);
            while (!terminado) Application.DoEvents();
            MontarControl(ucJuego);

            ucJuego.RecibirFiguraCreada(resultado);
        }

        private Tablero CrearTableroConEditor(int numero)
        {
            Tablero resultado = null;
            bool terminado = false;
            var ucCreador = new UcCreadorTablas(numero);
            ucCreador.OnTablaConfirmada += t => { resultado = t; terminado = true; };
            ucCreador.OnCancelado += () => terminado = true;
            MontarControl(ucCreador);
            while (!terminado) Application.DoEvents();
            return resultado ?? CrearTableroAleatorio();
        }
        private void CerrarLimpio()
        {
            if (_cerrandoIntencional) return;
            _cerrandoIntencional = true;
            QuitarOverlayPostPartida();
            _ucJuego?.LiberarRecursos();
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_cerrandoIntencional) { base.OnFormClosing(e); return; }
            e.Cancel = true;
            var r = MessageBox.Show("¿Deseas salir del juego?", "Salir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes) CerrarLimpio();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape && _ucJuego != null && !_ucJuego.IsDisposed)
                _ucJuego.SolicitarSalida();
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