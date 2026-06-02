using System;
using System.Drawing;
using System.IO;

namespace LoteriaMexicana.Core
{
    public static class GestorArchivos
    {
        private static readonly string _raizDatos = ResolverRaizDatos();
        public static string RutaImagenes => Path.Combine(_raizDatos, "Imagenes");
        public static string RutaAudio => Path.Combine(_raizDatos, "Audios");
        public static string RutaJson => Path.Combine(_raizDatos, "cartas.json");

        public static string RutaImagen(int id) =>
            Path.Combine(RutaImagenes, $"{id}.png");
        public static string RutaAudioCarta(int id)
        {
            string mp3 = Path.Combine(RutaAudio, $"{id}.mp3");
            if (File.Exists(mp3)) return mp3;

            string wav = Path.Combine(RutaAudio, $"{id}.wav");
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
            string juntoBin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (Directory.Exists(juntoBin)) return juntoBin;
            string dirEjecutable = AppDomain.CurrentDomain.BaseDirectory
                                   .TrimEnd(Path.DirectorySeparatorChar);
            string dirProyecto = dirEjecutable;

            for (int niveles = 0; niveles < 3; niveles++)
            {
                string padre = Path.GetDirectoryName(dirProyecto);
                if (padre == null) break;
                dirProyecto = padre;
            }

            string candidatoProyecto = Path.Combine(dirProyecto, "Data");
            if (Directory.Exists(candidatoProyecto)) return candidatoProyecto;
            string dirActual = AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 0; i < 6; i++)
            {
                string padre = Path.GetDirectoryName(dirActual);
                if (padre == null) break;
                dirActual = padre;

                string candidato = Path.Combine(dirActual, "Data");
                if (Directory.Exists(candidato)) return candidato;
            }
            return juntoBin;
        }
    }
}