using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LoteriaMexicana.Network
{
    public class ServidorLoteria
    {
        private const int PUERTO = 5000;
        private TcpListener _listener;
        private bool _activo = false;
        private readonly Dictionary<TcpClient, string> _nombresClientes
            = new Dictionary<TcpClient, string>();
        private readonly List<string> _reclamosActivos = new List<string>();
        private System.Threading.Timer _timerEmpate;
        public event Action<string>        OnClienteConectado;
        public event Action<string>        OnMensajeRecibido;
        public event Action<string>        OnError;
        public void Iniciar()
        {
            if (_activo) return;
            _activo = true;
            _listener = new TcpListener(IPAddress.Any, PUERTO);
            _listener.Start();
            Task.Run(() => BucleAceptacion());
        }

        public void Detener()
        {
            _activo = false;
            _listener?.Stop();
            lock (_nombresClientes)
            {
                foreach (var kv in _nombresClientes) kv.Key.Close();
                _nombresClientes.Clear();
            }
        }
        public void Transmitir(string mensaje)
        {
            var desconectados = new List<TcpClient>();
            lock (_nombresClientes)
            {
                foreach (var kv in _nombresClientes)
                {
                    if (!EscribirACliente(kv.Key, mensaje))
                        desconectados.Add(kv.Key);
                }
                foreach (var caido in desconectados)
                {
                    caido.Close();
                    _nombresClientes.Remove(caido);
                }
            }
        }
        private async Task BucleAceptacion()
        {
            while (_activo)
            {
                try
                {
                    TcpClient nuevoCliente = await _listener.AcceptTcpClientAsync();
                    lock (_nombresClientes)
                        _nombresClientes[nuevoCliente] = string.Empty;

                    string ip = ((IPEndPoint)nuevoCliente.Client.RemoteEndPoint).Address.ToString();
                    OnClienteConectado?.Invoke(ip);
                    Task.Run(() => EscucharCliente(nuevoCliente));
                }
                catch (Exception ex) when (_activo)
                {
                    OnError?.Invoke($"[Servidor] Error aceptando cliente: {ex.Message}");
                }
            }
        }
        private void EscucharCliente(TcpClient cliente)
        {
            try
            {
                using var reader = new StreamReader(cliente.GetStream());
                while (_activo && cliente.Connected)
                {
                    string linea = reader.ReadLine();
                    if (linea == null) break;

                    if (linea.StartsWith("CONNECT|"))
                    {
                        ProcesarConexion(cliente, linea);
                    }
                    else if (linea.StartsWith("RECLAMO_LOTERIA|"))
                    {
                        ProcesarReclamo(linea);
                    }
                    else
                    {
                        OnMensajeRecibido?.Invoke(linea);
                        Transmitir(linea);
                    }
                }
            }
            catch (Exception ex) when (_activo)
            {
                OnError?.Invoke($"[Servidor] Cliente desconectado: {ex.Message}");
            }
            finally
            {
                string nombre = string.Empty;
                lock (_nombresClientes)
                {
                    _nombresClientes.TryGetValue(cliente, out nombre);
                    _nombresClientes.Remove(cliente);
                    cliente.Close();
                }
                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    TransmitirListaJugadores();
                    Transmitir($"CHAT|Sistema|{nombre} se desconectó.");
                }
            }
        }
        private void ProcesarConexion(TcpClient cliente, string linea)
        {
            string nombre = linea.Length > 8 ? linea.Substring(8).Trim() : string.Empty;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                EscribirACliente(cliente, "CONNECT_REJECT|Nombre inválido.");
                lock (_nombresClientes) _nombresClientes.Remove(cliente);
                cliente.Close();
                return;
            }

            bool nombreEnUso;
            lock (_nombresClientes)
                nombreEnUso = _nombresClientes.Any(kv => kv.Key != cliente
                    && kv.Value.Equals(nombre, StringComparison.OrdinalIgnoreCase));

            if (nombreEnUso)
            {
                EscribirACliente(cliente, $"CONNECT_REJECT|El nombre '{nombre}' ya está en uso.");
                lock (_nombresClientes) _nombresClientes.Remove(cliente);
                cliente.Close();
                return;
            }

            lock (_nombresClientes)
                _nombresClientes[cliente] = nombre;

            EscribirACliente(cliente, "CONNECT_OK");
            TransmitirListaJugadores();
            Transmitir($"CHAT|Sistema|{nombre} se unió a la partida.");
        }
        private void TransmitirListaJugadores()
        {
            string[] nombres;
            lock (_nombresClientes)
                nombres = _nombresClientes.Values
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .ToArray();

            Transmitir("JUGADORES|" + string.Join(",", nombres));
        }
        private void ProcesarReclamo(string linea)
        {
            lock (_reclamosActivos)
            {
                _reclamosActivos.Add(linea);

                if (_reclamosActivos.Count == 1)
                {
                    _timerEmpate?.Dispose();
                    _timerEmpate = new System.Threading.Timer(_ => ResolverEmpate(), null, 2000, System.Threading.Timeout.Infinite);
                }
            }
        }

        private void ResolverEmpate()
        {
            List<string> reclamos;
            lock (_reclamosActivos)
            {
                reclamos = new List<string>(_reclamosActivos);
                _reclamosActivos.Clear();
            }
            if (reclamos.Count == 0) return;

            foreach (var r in reclamos)
                OnMensajeRecibido?.Invoke(r);

            if (reclamos.Count == 1)
            {
                var p = reclamos[0].Split('|');
                if (p.Length >= 3)
                    Transmitir($"GANADOR|{p[1]}|{p[2]}");
            }
            else
            {
                Transmitir("TIE|" + string.Join(";", reclamos.Select(r =>
                {
                    var p = r.Split('|');
                    return p.Length >= 5 ? $"{p[1]}~{p[2]}~{p[3]}~{p[4]}" : r;
                })));
            }
        }
        private bool EscribirACliente(TcpClient cliente, string mensaje)
        {
            try
            {
                var writer = new StreamWriter(cliente.GetStream()) { AutoFlush = true };
                writer.WriteLine(mensaje);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
