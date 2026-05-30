using System;

namespace LoteriaMexicana.Logic
{
    public static class ValidadorVictoria
    {
        private const int FILAS    = 4;
        private const int COLUMNAS = 5;
        public static bool ValidarLineaHorizontal(bool[,] tapas)
        {
            ValidarDimension(tapas);

            for (int f = 0; f < FILAS; f++)
            {
                bool filaCompleta = true;
                for (int c = 0; c < COLUMNAS; c++)
                {
                    if (!tapas[f, c]) { filaCompleta = false; break; }
                }
                if (filaCompleta) return true;
            }
            return false;
        }
        public static bool ValidarLineaVertical(bool[,] tapas)
        {
            ValidarDimension(tapas);

            for (int c = 0; c < COLUMNAS; c++)
            {
                bool colCompleta = true;
                for (int f = 0; f < FILAS; f++)
                {
                    if (!tapas[f, c]) { colCompleta = false; break; }
                }
                if (colCompleta) return true;
            }
            return false;
        }
        public static bool ValidarDiagonal(bool[,] tapas)
        {
            ValidarDimension(tapas);

            bool diagPrincipal = true;
            for (int i = 0; i < FILAS; i++)
                if (!tapas[i, i]) { diagPrincipal = false; break; }

            if (diagPrincipal) return true;

            bool diagSecundaria = true;
            for (int i = 0; i < FILAS; i++)
                if (!tapas[i, COLUMNAS - 1 - i]) { diagSecundaria = false; break; }

            return diagSecundaria;
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

            const int FILA_CENTRAL = 1;   
            const int COL_CENTRAL  = 2;  

            for (int c = 0; c < COLUMNAS; c++)
                if (!tapas[FILA_CENTRAL, c]) return false;

            for (int f = 0; f < FILAS; f++)
                if (!tapas[f, COL_CENTRAL]) return false;

            return true;
        }
        public static string EvaluarTodo(bool[,] tapas)
        {
            if (ValidarLineaHorizontal(tapas)) return "Línea Horizontal";
            if (ValidarLineaVertical(tapas))   return "Línea Vertical";
            if (ValidarDiagonal(tapas))        return "Diagonal";
            if (ValidarEsquinas(tapas))        return "Esquinas";
            if (ValidarPoyaOCruz(tapas))       return "Poya / Cruz";
            return null;
        }
        private static void ValidarDimension(bool[,] tapas)
        {
            if (tapas == null)
                throw new ArgumentNullException(nameof(tapas),
                    "La matriz de tapas no puede ser null.");

            if (tapas.GetLength(0) != FILAS || tapas.GetLength(1) != COLUMNAS)
                throw new ArgumentException(
                    $"La matriz de tapas debe ser {FILAS}×{COLUMNAS}. " +
                    $"Se recibió {tapas.GetLength(0)}×{tapas.GetLength(1)}.",
                    nameof(tapas));
        }
    }
}
