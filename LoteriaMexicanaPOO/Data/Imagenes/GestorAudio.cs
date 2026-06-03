using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoteriaMexicana.Core
{

    public class GestorAudio : IDisposable
    {
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciSendString(
            string lpstrCommand,
            StringBuilder lpstrReturnString,
            int uReturnLength,
            IntPtr hwndCallback);

        private const string ALIAS = "loteria_carta";
        private bool _disposed = false;

        private CancellationTokenSource _cts = new CancellationTokenSource();
        public void ReproducirCarta(int id)
        {
            string ruta = GestorArchivos.RutaAudioCarta(id);
            if (string.IsNullOrEmpty(ruta)) return;
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            Task.Run(() => Reproducir(ruta, token), token);
        }

        public void Detener()
        {
            try
            {
                _cts?.Cancel();
                Enviar($"stop {ALIAS}");
                Enviar($"close {ALIAS}");
            }
            catch { }
        }
        private void Reproducir(string ruta, CancellationToken token)
        {
            try
            {
                if (token.IsCancellationRequested) return;
                Enviar($"stop {ALIAS}");
                Enviar($"close {ALIAS}");

                if (token.IsCancellationRequested) return;
                int ret = mciSendString(
                    $"open \"{ruta}\" alias {ALIAS}",
                    null, 0, IntPtr.Zero);
                if (ret != 0) return;

                if (token.IsCancellationRequested)
                {
                    Enviar($"close {ALIAS}");
                    return;
                }

                Enviar($"play {ALIAS}");
                var sb = new StringBuilder(128);
                for (int i = 0; i < 150 && !token.IsCancellationRequested; i++)
                {
                    Thread.Sleep(200);
                    sb.Clear();
                    mciSendString($"status {ALIAS} mode", sb, 128, IntPtr.Zero);
                    if (sb.ToString().Trim() == "stopped") break;
                }

                Enviar($"stop {ALIAS}");
                Enviar($"close {ALIAS}");
            }
            catch
            {
            }
        }
        private static void Enviar(string cmd)
        {
            try { mciSendString(cmd, null, 0, IntPtr.Zero); }
            catch { }
        }
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _cts?.Cancel();
            _cts?.Dispose();
            Enviar($"stop {ALIAS}");
            Enviar($"close {ALIAS}");
        }
    }
}
