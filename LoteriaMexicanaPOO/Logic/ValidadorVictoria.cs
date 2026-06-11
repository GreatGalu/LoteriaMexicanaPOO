using System.Collections.Generic;
using System.Linq;
using LoteriaMexicana.Models;

namespace LoteriaMexicana.Logic
{
    public class ValidadorVictoria
    {
        private readonly List<FiguraGanar> _figurasActivas;
        private readonly bool _tablaLlena;

        public ValidadorVictoria(List<FiguraGanar> figurasActivas, bool tablaLlena = false)
        {
            _figurasActivas = figurasActivas;
            _tablaLlena = tablaLlena;
        }

        public enum ResultadoValidacion { SinFigura, Victoria, Trampa }

        public class DetalleValidacion
        {
            public ResultadoValidacion Resultado { get; init; }
            public string Figura { get; init; }
            public List<int> CartasTrampa { get; init; } = new();
        }

        public DetalleValidacion EvaluarConValidacion(
            bool[,] tapas,
            int[,] casillas,
            List<int> cartasCantadas)
        {
            string figura = EvaluarTodo(tapas);

            if (figura == null)
                return new DetalleValidacion { Resultado = ResultadoValidacion.SinFigura };

            var trampa = DetectarTrampa(tapas, casillas, cartasCantadas);

            if (trampa.Count > 0)
                return new DetalleValidacion
                {
                    Resultado = ResultadoValidacion.Trampa,
                    Figura = figura,
                    CartasTrampa = trampa
                };

            return new DetalleValidacion { Resultado = ResultadoValidacion.Victoria, Figura = figura };
        }

        public string EvaluarTodo(bool[,] tapas)
        {
            if (_tablaLlena)
                return ValidarTablaLlena(tapas) ? "Tabla llena" : null;

            var encontradas = _figurasActivas
                .Where(f => ValidarFigura(tapas, f))
                .Select(f => f.Nombre)
                .ToList();

            return encontradas.Count > 0 ? string.Join(" + ", encontradas) : null;
        }

        private static bool ValidarTablaLlena(bool[,] tapas)
        {
            for (int f = 0; f < Tablero.FILAS; f++)
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                    if (!tapas[f, c]) return false;
            return true;
        }

        private static bool ValidarFigura(bool[,] tapas, FiguraGanar figura)
        {
            return figura.Casillas.All(pos => tapas[pos[0], pos[1]]);
        }

        private static List<int> DetectarTrampa(
            bool[,] tapas,
            int[,] casillas,
            List<int> cartasCantadas)
        {
            var trampa = new List<int>();

            var conteoCantadas = new Dictionary<int, int>();
            foreach (var id in cartasCantadas)
            {
                if (!conteoCantadas.ContainsKey(id)) conteoCantadas[id] = 0;
                conteoCantadas[id]++;
            }

            var conteoTapadas = new Dictionary<int, int>();
            for (int f = 0; f < Tablero.FILAS; f++)
            {
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    if (!tapas[f, c]) continue;
                    int id = casillas[f, c];

                    if (!conteoTapadas.ContainsKey(id)) conteoTapadas[id] = 0;
                    conteoTapadas[id]++;

                    if (!conteoCantadas.ContainsKey(id) || conteoTapadas[id] > conteoCantadas[id])
                        trampa.Add(id);
                }
            }

            return trampa;
        }
    }
}