using System;
using System.Media;
using System.IO;
using System.Threading.Tasks;

namespace LoteriaMexicana.Core
{
    /// <summary>
    /// Reproduce el audio del cantor sin bloquear la UI.
    ///
    /// Estrategia:
    ///   - Archivos .wav  → SoundPlayer (incluido en .NET, sin dependencias).
    ///   - Archivos .mp3  → WMPLib (Windows Media Player COM, disponible en Windows).
    ///     Si WMPLib no está disponible, el audio .mp3 se omite silenciosamente.
    ///
    /// Para habilitar .mp3 en tu proyecto:
    ///   1. Referencias del proyecto → Agregar referencia COM
    ///   2. Buscar "Windows Media Player" → Activar → Aceptar
    /// </summary>
    public class GestorAudio : IDisposable
    {
        private SoundPlayer _soundPlayer;
        private bool _disposed = false;

        // ── API pública ───────────────────────────────────────────

        /// <summary>
        /// Reproduce el audio de la carta indicada de forma asíncrona.
        /// No lanza excepción si el archivo no existe: falla silenciosamente.
        /// </summary>
        public void ReproducirCarta(int id)
        {
            string ruta = GestorArchivos.RutaAudioCarta(id);
            if (ruta == null) return;          // Archivo no encontrado: sin audio

            Task.Run(() => Reproducir(ruta));  // Hilo secundario: no bloquea la UI
        }

        /// <summary>Detiene cualquier reproducción en curso.</summary>
        public void Detener()
        {
            try { _soundPlayer?.Stop(); } catch { /* ignorar */ }
        }

        // ── Lógica interna ────────────────────────────────────────

        private void Reproducir(string ruta)
        {
            try
            {
                string ext = Path.GetExtension(ruta).ToLower();

                if (ext == ".wav")
                    ReproducirWav(ruta);
                else if (ext == ".mp3")
                    ReproducirMp3(ruta);
            }
            catch
            {
                // Audio nunca debe romper el juego; cualquier error se ignora
            }
        }

        private void ReproducirWav(string ruta)
        {
            _soundPlayer?.Dispose();
            _soundPlayer = new SoundPlayer(ruta);
            _soundPlayer.PlaySync();   // Sync aquí está bien: ya estamos en hilo secundario
        }

        private void ReproducirMp3(string ruta)
        {
            // WMPLib.WindowsMediaPlayer está disponible si el proyecto referencia la COM
            // Si no está disponible, este bloque lanza excepción → se captura arriba y se ignora
            dynamic wmp = Activator.CreateInstance(Type.GetTypeFromProgID("WMPlayer.OCX"));
            wmp.URL = ruta;
            wmp.controls.play();

            // Esperar a que termine antes de liberar (máximo 30 segundos)
            int espera = 0;
            while (wmp.playState != 1 && espera < 300) // 1 = wmppsReady (stopped)
            {
                System.Threading.Thread.Sleep(100);
                espera++;
            }
            wmp.close();
        }

        // ── IDisposable ───────────────────────────────────────────

        public void Dispose()
        {
            if (_disposed) return;
            _soundPlayer?.Dispose();
            _disposed = true;
        }
    }
}
