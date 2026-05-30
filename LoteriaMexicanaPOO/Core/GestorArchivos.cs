using System;
using System.Drawing;
using System.IO;

namespace LoteriaMexicana.Core
{
    public static class GestorArchivos
    {
        private static readonly string _raizDatos = ResolverRaizDatos();

        public static string RutaImagenes => Path.Combine(_raizDatos, "Images");
        public static string RutaAudio    => Path.Combine(_raizDatos, "Audio");
        public static string RutaJson     => Path.Combine(_raizDatos, "cartas.json");
        public static string RutaImagen(int id) =>
            Path.Combine(RutaImagenes, $"{id:D2}.png");
        public static string RutaAudioCarta(int id)
        {
            string mp3 = Path.Combine(RutaAudio, $"{id:D2}.mp3");
            if (File.Exists(mp3)) return mp3;

            string wav = Path.Combine(RutaAudio, $"{id:D2}.wav");
            if (File.Exists(wav)) return wav;

            return null;
        }
        public static Image CargarImagen(int id)
        {
            string ruta = RutaImagen(id);
            if (!File.Exists(ruta)) return null;

            using var ms = new MemoryStream(File.ReadAllBytes(ruta));
            return Image.FromStream(ms);
        }
        public static bool ExisteJsonCatalogo() => File.Exists(RutaJson);
        public static string LeerJsonCatalogo()
        {
            if (!ExisteJsonCatalogo()) return null;
            return File.ReadAllText(RutaJson, System.Text.Encoding.UTF8);
        }
        public static void AsegurarEstructura()
        {
            Directory.CreateDirectory(RutaImagenes);
            Directory.CreateDirectory(RutaAudio);
        }
        private static string ResolverRaizDatos()
        {
            string junto = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (Directory.Exists(junto)) return junto;
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 0; i < 6; i++)
            {
                dir = Path.GetDirectoryName(dir) ?? dir;
                string candidato = Path.Combine(dir, "Data");
                if (Directory.Exists(candidato)) return candidato;
            }
            return junto;
        }
    }
}
