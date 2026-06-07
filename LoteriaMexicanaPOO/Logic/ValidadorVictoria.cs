using System;
using System.Collections.Generic;

namespace LoteriaMexicana.Logic
{
    public static class ValidadorVictoria
    {
        private const int FILAS = 4;
        private const int COLUMNAS = 5;
        public enum ResultadoValidacion
        {
            SinFigura,      
            Victoria,       
            Trampa          
        }

        public class DetalleValidacion
        {
            public ResultadoValidacion Resultado { get; init; }
            public string Figura { get; init; }  
            public List<int> CartasTrampa { get; init; } = new();
        }
        public static DetalleValidacion EvaluarConValidacion(
            bool[,] tapas,
            int[,] casillas,
            List<int> cartasCantadas)
        {
            ValidarDimension(tapas);

            string figura = EvaluarTodo(tapas);

            if (figura == null)
                return new DetalleValidacion { Resultado = ResultadoValidacion.SinFigura };

            var trampa = new List<int>();
            for (int f = 0; f < FILAS; f++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    if (!tapas[f, c]) continue;          

                    int idCarta = casillas[f, c];
                    if (!cartasCantadas.Contains(idCarta))
                        trampa.Add(idCarta);
                }
            }

            if (trampa.Count > 0)
                return new DetalleValidacion
                {
                    Resultado = ResultadoValidacion.Trampa,
                    Figura = figura,
                    CartasTrampa = trampa
                };

            return new DetalleValidacion
            {
                Resultado = ResultadoValidacion.Victoria,
                Figura = figura
            };
        }
        public static string EvaluarTodo(bool[,] tapas)
        {
            if (ValidarLineaHorizontal(tapas)) return "Linea Horizontal";
            if (ValidarLineaVertical(tapas)) return "Linea Vertical";
            if (ValidarDiagonal(tapas)) return "Diagonal";
            if (ValidarEsquinas(tapas)) return "Esquinas";
            if (ValidarPoyaOCruz(tapas)) return "Poya / Cruz";
            return null;
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
            bool principal = true;
            for (int i = 0; i < FILAS; i++)
                if (!tapas[i, i]) { principal = false; break; }
            if (principal) return true;

            bool secundaria = true;
            for (int i = 0; i < FILAS; i++)
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
            const int COL_CENTRAL = 2;
            if (FilaCompleta(tapas, 1) && ColumnaCompleta(tapas, COL_CENTRAL)) return true;
            if (FilaCompleta(tapas, 2) && ColumnaCompleta(tapas, COL_CENTRAL)) return true;
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