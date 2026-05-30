namespace LoteriaMexicana.Models
{
    public class Carta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ImagenPath { get; set; }
        public string AudioPath { get; set; }
        public Carta(int id, string nombre, string imagenPath, string audioPath)
        {
            Id         = id;
            Nombre     = nombre;
            ImagenPath = imagenPath;
            AudioPath  = audioPath;
        }
        public override string ToString() => $"[{Id:D2}] {Nombre}";
    }
}
