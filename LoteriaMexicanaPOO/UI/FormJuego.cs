using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LoteriaMexicana.Core;
using LoteriaMexicana.Logic;
using LoteriaMexicana.Network;
using LoteriaMexicana.UI.UserControls;

namespace LoteriaMexicana.UI
{ 
    public partial class FormJuego : Form
    {
        private ClienteLoteria  _cliente;
        private ServidorLoteria _servidor;
        private UcPantallaInicio  _ucInicio;
        private UcPantallaJuego   _ucJuego;
        private bool _cerrandoIntencional = false;
        public FormJuego()
        {
            InitializeComponent();
            MontarPantallaInicio();
        }
        private void MontarPantallaInicio()
        {
            _ucInicio = new UcPantallaInicio();
            _ucInicio.OnCrearPartida += FlujoCrearPartida;
            _ucInicio.OnUnirse       += FlujoUnirse;

            contenedor.Controls.Clear();
            contenedor.Controls.Add(_ucInicio);
        }
        private async void FlujoCrearPartida(string nombre, int cantidadTablas)
        {
            var tableros = await PedirTablerosAsync(cantidadTablas);

            _ucJuego = ConstruirUcJuego(nombre, tableros);
            CambiarPantalla(_ucJuego);

            try
            {
                _servidor = new ServidorLoteria();
                _servidor.OnError            += msg => _ucJuego?.Invoke(new Action(() =>
                    MessageBox.Show(msg, "Error Servidor", MessageBoxButtons.OK, MessageBoxIcon.Warning)));
                _servidor.OnClienteConectado += ip => _ucJuego?.BeginInvoke(new Action(() =>
                    {  }));
                _servidor.Iniciar();

                _cliente = new ClienteLoteria();
                _ucJuego.AsignarCliente(_cliente);
                _cliente.Conectar("127.0.0.1");

                var mazo = new Mazo();
                mazo.Barajar();
                _ucJuego.AsignarServidor(_servidor, mazo);

                string ip     = ObtenerIpLocal();
                string codigo = IpACodigo(ip);
                _ucJuego.MostrarCodigoSala(codigo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear la partida:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MontarPantallaInicio();
            }
        }
        private async void FlujoUnirse(string nombre, int cantidadTablas, string entradaSala)
        {
            var tableros = await PedirTablerosAsync(cantidadTablas);
            _ucJuego     = ConstruirUcJuego(nombre, tableros);
            CambiarPantalla(_ucJuego);

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
                MessageBox.Show($"No se pudo conectar a {ip}:\n{ex.Message}",
                    "Error de conexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MontarPantallaInicio();
            }
        }
        private System.Threading.Tasks.Task<List<Tablero>> PedirTablerosAsync(int cantidad)
        {
            return System.Threading.Tasks.Task.FromResult(PedirTableros(cantidad));
        }

        private List<Tablero> PedirTableros(int cantidad)
        {
            var lista = new List<Tablero>();

            for (int i = 0; i < cantidad; i++)
            {
                var resp = MessageBox.Show(
                    $"Tabla {i + 1}: ¿Deseas crearla de forma personalizada?\n\n" +
                    "Sí  = Elegir mis 20 cartas\nNo = Tabla aleatoria",
                    $"Tabla {i + 1}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resp == DialogResult.Yes)
                {
                    Tablero t = PedirTableroConCreador(i + 1);
                    lista.Add(t);
                }
                else
                {
                    var t = new Tablero();
                    t.GenerarAleatorio();
                    lista.Add(t);
                }
            }
            return lista;
        }
        private Tablero PedirTableroConCreador(int numero)
        {
            Tablero resultado = null;
            bool terminado = false;

            var ucCreador = new UcCreadorTablas(numero);

            ucCreador.OnTablaConfirmada += t =>
            {
                resultado = t;
                terminado = true;
            };
            ucCreador.OnCancelado += () =>
            {
                terminado = true;
            };

            contenedor.Controls.Clear();
            contenedor.Controls.Add(ucCreador);
            while (!terminado)
                Application.DoEvents();

            if (resultado == null)
            {
                resultado = new Tablero();
                resultado.GenerarAleatorio();
            }
            return resultado;
        }
        private UcPantallaJuego ConstruirUcJuego(string nombre, List<Tablero> tableros)
        {
            var uc = new UcPantallaJuego();
            uc.Configurar(nombre, tableros);
            uc.OnSolicitarSalida += CerrarLimpio;
            return uc;
        }

        private void CambiarPantalla(UserControl siguiente)
        {
            contenedor.Controls.Clear();
            contenedor.Controls.Add(siguiente);
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
            CerrarLimpio();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape && _ucJuego != null)
            {
                _ucJuego.LiberarRecursos();
                CerrarLimpio();
            }
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
                var p  = ObtenerIpLocal().Split('.');
                return $"{p[0]}.{p[1]}.{s3}.{s4}";
            }
            catch { return "127.0.0.1"; }
        }
    }
}
