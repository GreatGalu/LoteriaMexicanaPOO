using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LoteriaMexicana.Network
{
    public class ServidorLoteria
    {
        private const int PUERTO = 5000;
        private TcpListener _listener;
        private readonly List<TcpClient> _clientes = new List<TcpClient>();

        private bool _activo = false;
        public event Action<string> OnClienteConectado;

        public event Action<string> OnMensajeRecibido;

        public event Action<string> OnError;

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

            lock (_clientes)
            {
                foreach (var cliente in _clientes)
                    cliente.Close();
                _clientes.Clear();
            }
        }
        public void Transmitir(string mensaje)
        {
            var desconectados = new List<TcpClient>();

            lock (_clientes)
            {
                foreach (var cliente in _clientes)
                {
                    if (!EscribirACliente(cliente, mensaje))
                        desconectados.Add(cliente);
                }

                foreach (var caido in desconectados)
                {
                    caido.Close();
                    _clientes.Remove(caido);
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

                    lock (_clientes)
                        _clientes.Add(nuevoCliente);

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

                    OnMensajeRecibido?.Invoke(linea);

                    Transmitir(linea);
                }
            }
            catch (Exception ex) when (_activo)
            {
                OnError?.Invoke($"[Servidor] Cliente desconectado: {ex.Message}");
            }
            finally
            {
                lock (_clientes)
                {
                    _clientes.Remove(cliente);
                    cliente.Close();
                }
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
