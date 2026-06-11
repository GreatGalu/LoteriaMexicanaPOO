using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LoteriaMexicana.Models
{
    public class FiguraGanar
    {
        public string Nombre { get; set; }
        public List<int[]> Casillas { get; set; } = new List<int[]>();

        public FiguraGanar() { }

        public FiguraGanar(string nombre, List<int[]> casillas)
        {
            Nombre = nombre;
            Casillas = casillas;
        }
    }
}
