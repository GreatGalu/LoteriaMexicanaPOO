using System;
using System.Collections.Generic;
using System.Linq;

namespace LoteriaMexicana.Logic
{
    public static class ValidadorVictoria
    {
        private const int FILAS    = Tablero.FILAS;   
        private const int COLUMNAS = Tablero.COLUMNAS;
        public static bool ReglaHorizontal { get; set; } = true;
        public static bool ReglaVertical   { get; set; } = true;
        public static bool ReglaDiagonal   { get; set; } = true;
        public static bool ReglaEsquinas   { get; set; } = true;
        public static bool ReglaPoyaCruz   { get; set; } = true;
        public enum ResultadoValidacion
        {
            SinFigura,
            Victoria,
            Trampa
        }

        public class DetalleValidacion
        {
            public ResultadoValidacion Resultado   { get; init; }
            public string              Figura      { get; init; }
            public List<int>           CartasTrampa { get; init; } = new();
        }
        public static DetalleValidacion EvaluarConValidacion(
            bool[,] tapas,
            int[,]  casillas,
            List<int> cartasCantadas)
        {
            ValidarDimension(tapas);

            string figura = EvaluarTodo(tapas);

            if (figura == null)
                return new DetalleValidacion { Resultado = ResultadoValidacion.SinFigura };

            var trampa = new List<int>();
            var conteoCantadas = new Dictionary<int, int>();
            foreach (var id in cartasCantadas)
            {
                if (!conteoCantadas.ContainsKey(id)) conteoCantadas[id] = 0;
                conteoCantadas[id]++;
            }

            var conteoTapadas = new Dictionary<int, int>();
            for (int f = 0; f < FILAS; f++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    if (!tapas[f, c]) continue;
                    int idCarta = casillas[f, c];

                    if (!conteoTapadas.ContainsKey(idCarta)) conteoTapadas[idCarta] = 0;
                    conteoTapadas[idCarta]++;

                    if (!conteoCantadas.ContainsKey(idCarta) || conteoTapadas[idCarta] > conteoCantadas[idCarta])
                    {
                        trampa.Add(idCarta);
                    }
                }
            }

            if (trampa.Count > 0)
                return new DetalleValidacion
                {
                    Resultado    = ResultadoValidacion.Trampa,
                    Figura       = figura,
                    CartasTrampa = trampa
                };

            return new DetalleValidacion
            {
                Resultado = ResultadoValidacion.Victoria,
                Figura    = figura
            };
        }
        public static string EvaluarTodo(bool[,] tapas)
        {
            var figuras = new List<string>();
            if (ReglaHorizontal && ValidarLineaHorizontal(tapas)) figuras.Add("Línea Horizontal");
            if (ReglaVertical   && ValidarLineaVertical(tapas))   figuras.Add("Línea Vertical");
            if (ReglaDiagonal   && ValidarDiagonal(tapas))        figuras.Add("Diagonal");
            if (ReglaEsquinas   && ValidarEsquinas(tapas))        figuras.Add("Esquinas");
            if (ReglaPoyaCruz   && ValidarPoyaOCruz(tapas))       figuras.Add("Poya / Cruz");
            return figuras.Count > 0 ? string.Join(" + ", figuras) : null;
        }
        public static bool ValidarLineaHorizontal(bool[,] tapas)
        {
            ValidarDimension(tapas);
            for (int f = 0; f < FILAS; f++)
            {
                bool completa = true;
                for (int c = 0; c < COLUMNAS; c++)
                    if (!tapas[f, c]) { completa = false; break; }
                if (completa) return true;
            }
            return false;
        }

        public static bool ValidarLineaVertical(bool[,] tapas)
        {
            ValidarDimension(tapas);
            for (int c = 0; c < COLUMNAS; c++)
            {
                bool completa = true;
                for (int f = 0; f < FILAS; f++)
                    if (!tapas[f, c]) { completa = false; break; }
                if (completa) return true;
            }
            return false;
        }

        public static bool ValidarDiagonal(bool[,] tapas)
        {
            ValidarDimension(tapas);
            int largo = Math.Min(FILAS, COLUMNAS);

            bool principal = true;
            for (int i = 0; i < largo; i++)
                if (!tapas[i, i]) { principal = false; break; }
            if (principal) return true;

            bool secundaria = true;
            for (int i = 0; i < largo; i++)
                if (!tapas[i, COLUMNAS - 1 - i]) { secundaria = false; break; }
            return secundaria;
        }

        public static bool ValidarEsquinas(bool[,] tapas)
        {
            ValidarDimension(tapas);
            return tapas[0, 0]
                && tapas[0, COLUMNAS - 1]
                && tapas[FILAS - 1, 0]
                && tapas[FILAS - 1, COLUMNAS - 1];
        }
        public static bool ValidarPoyaOCruz(bool[,] tapas)
        {
            ValidarDimension(tapas);
            if (FilaCompleta(tapas, 2) && ColumnaCompleta(tapas, 2)) return true;
            return false;
        }
        private static bool FilaCompleta(bool[,] tapas, int fila)
        {
            for (int c = 0; c < COLUMNAS; c++)
                if (!tapas[fila, c]) return false;
            return true;
        }

        private static bool ColumnaCompleta(bool[,] tapas, int col)
        {
            for (int f = 0; f < FILAS; f++)
                if (!tapas[f, col]) return false;
            return true;
        }

        private static void ValidarDimension(bool[,] tapas)
        {
            if (tapas == null)
                throw new ArgumentNullException(nameof(tapas));
            if (tapas.GetLength(0) != FILAS || tapas.GetLength(1) != COLUMNAS)
                throw new ArgumentException(
                    $"La matriz debe ser {FILAS}x{COLUMNAS}. " +
                    $"Se recibio {tapas.GetLength(0)}x{tapas.GetLength(1)}.");
        }
    }
}