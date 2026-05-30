using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LoteriaMexicana.Network
{
    public class ClienteLoteria
    {
        private const int PUERTO = 5000;
        private TcpClient  _cliente;
        private StreamWriter _writer;
        private bool _conectado = false;
        public event Action<string> OnMensajeRecibido;
        public event Action OnDesconectado;
        public event Action<string> OnError;
        public void Conectar(string ip)
        {
            if (_conectado) return;

            _cliente   = new TcpClient();
            _cliente.Connect(ip, PUERTO);
            _writer    = new StreamWriter(_cliente.GetStream()) { AutoFlush = true };
            _conectado = true;

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
                    OnMensajeRecibido?.Invoke(linea);
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
