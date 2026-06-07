using System;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace LoteriaMexicana.Core
{
    public class GestorVoz : IDisposable
    {
        private readonly SpeechSynthesizer _synth;
        private bool _disposed = false;

        public GestorVoz()
        {
            _synth = new SpeechSynthesizer();
            _synth.SetOutputToDefaultAudioDevice();

            _synth.Rate = -2;
            _synth.Volume = 100;

            foreach (var voz in _synth.GetInstalledVoices())
            {
                var info = voz.VoiceInfo;
                if (info.Culture.Name.StartsWith("es", StringComparison.OrdinalIgnoreCase))
                {
                    _synth.SelectVoice(info.Name);
                    break;
                }
            }
        }
        public void AnunciarCarta(string nombreCarta)
        {
            if (_disposed || string.IsNullOrWhiteSpace(nombreCarta)) return;

            Task.Run(() =>
            {
                try
                {
                    _synth.SpeakAsyncCancelAll();   
                    _synth.SpeakAsync(nombreCarta);
                }
                catch { }
            });
        }

        public void Detener()
        {
            try { _synth?.SpeakAsyncCancelAll(); } catch { }
        }
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            try { _synth.SpeakAsyncCancelAll(); } catch { }
            _synth?.Dispose();
        }
    }
}