using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LoteriaMexicana.Core;
using LoteriaMexicana.Logic;
using LoteriaMexicana.Network;
using LoteriaMexicana.UI.UserControls;

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
                _cliente.Conectar("127.0.0.1");

                _ucJuego.AsignarServidor(_servidor, _mazo);

                string codigo = IpACodigo(ObtenerIpLocal());
                _ucJuego.MostrarCodigoSala(codigo);
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
                _cliente.Conectar(ip);
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
            return uc;
        }
        private List<Tablero> RecolectarTableros(int cantidad)
        {
            var lista = new List<Tablero>();
            for (int i = 0; i < cantidad; i++)
            {
                var resp = MessageBox.Show(
                    $"Tabla {i + 1}: ¿Crearla de forma personalizada?\n\nSí = elegir cartas\nNo = aleatoria",
                    $"Tabla {i + 1}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                lista.Add(resp == DialogResult.Yes
                    ? CrearTableroConEditor(i + 1)
                    : CrearTableroAleatorio());
            }
            return lista;
        }

        private static Tablero CrearTableroAleatorio()
        {
            var t = new Tablero();
            t.GenerarAleatorio();
            return t;
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