using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LoteriaMexicana.Network
{
    public class ClienteLoteria
    {
        private const int PUERTO = 5000;
        private TcpClient    _cliente;
        private StreamWriter _writer;
        private bool         _conectado = false;

        public event Action<string> OnMensajeRecibido;
        public event Action         OnDesconectado;
        public event Action<string> OnError;
        /// <summary>
        /// Se dispara cuando el servidor rechaza el nombre elegido.
        /// El argumento es el motivo del rechazo.
        /// </summary>
        public event Action<string> OnConectadoRechazado;
        /// <summary>
        /// Se dispara cuando el servidor acepta la conexión.
        /// </summary>
        public event Action OnConectadoOk;

        public void Conectar(string ip, string nombre)
        {
            if (_conectado) return;

            _cliente   = new TcpClient();
            _cliente.Connect(ip, PUERTO);
            _writer    = new StreamWriter(_cliente.GetStream()) { AutoFlush = true };
            _conectado = true;

            // Presentarse con el nombre al servidor
            _writer.WriteLine($"CONNECT|{nombre}");

            Task.Run(() => BucleEscucha());
        }

        public void Enviar(string mensaje)
        {
            if (!_conectado || _writer == null) return;
            try
            {
                _writer.WriteLine(mensaje);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"[Cliente] Error al enviar: {ex.Message}");
            }
        }

        public void Desconectar()
        {
            _conectado = false;
            _writer?.Close();
            _cliente?.Close();
        }

        private void BucleEscucha()
        {
            try
            {
                using var reader = new StreamReader(_cliente.GetStream());
                while (_conectado)
                {
                    string linea = reader.ReadLine();
                    if (linea == null) break;

                    if (linea == "CONNECT_OK")
                    {
                        OnConectadoOk?.Invoke();
                    }
                    else if (linea.StartsWith("CONNECT_REJECT|"))
                    {
                        string motivo = linea.Substring(15);
                        _conectado = false;
                        OnConectadoRechazado?.Invoke(motivo);
                        break;
                    }
                    else
                    {
                        OnMensajeRecibido?.Invoke(linea);
                    }
                }
            }
            catch (Exception ex) when (_conectado)
            {
                OnError?.Invoke($"[Cliente] Error de escucha: {ex.Message}");
            }
            finally
            {
                _conectado = false;
                OnDesconectado?.Invoke();
            }
        }
    }
}
