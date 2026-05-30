using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoteriaMexicana.Logic
{
    /// <summary>
    /// Administra la tabla de juego personal de un jugador (4 filas x 5 columnas).
    /// </summary>
    public class Tablero
    {
        public const int FILAS          = 4;
        public const int COLUMNAS       = 5;
        public const int TOTAL_CASILLAS = FILAS * COLUMNAS;
        private const int ID_MIN = 1;
        private const int ID_MAX = 54;

        public int[,]  Casillas      { get; private set; }
        public bool[,] Tapas         { get; private set; }
        public List<int> CartasCantadas { get; private set; } = new List<int>();

        public Tablero()
        {
            Casillas = new int[FILAS, COLUMNAS];
            Tapas    = new bool[FILAS, COLUMNAS];
        }

        // --- Carga ---

        public void GenerarAleatorio()
        {
            var pool = Enumerable.Range(ID_MIN, ID_MAX).ToList();
            var rng  = new Random();
            for (int i = pool.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (pool[i], pool[j]) = (pool[j], pool[i]);
            }
            LlenarMatriz(pool.Take(TOTAL_CASILLAS).ToArray());
            ReiniciarTapas();
        }

        /// <summary>Carga desde un arreglo de 20 IDs ya validados (FormCrearTabla).</summary>
        public void CargarDesdeIds(int[] ids)
        {
            if (ids == null || ids.Length != TOTAL_CASILLAS)
                throw new ArgumentException($"Se requieren exactamente {TOTAL_CASILLAS} IDs.");
            LlenarMatriz(ids);
            ReiniciarTapas();
        }

        public bool CargarDesdeArchivo(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo)) return false;
            string contenido;
            try   { contenido = File.ReadAllText(rutaArchivo).Trim(); }
            catch { return false; }
            string[] partes = contenido.Split(',');
            if (partes.Length != TOTAL_CASILLAS) return false;
            var ids = new int[TOTAL_CASILLAS];
            for (int i = 0; i < partes.Length; i++)
            {
                if (!int.TryParse(partes[i].Trim(), out int id)) return false;
                if (id < ID_MIN || id > ID_MAX) return false;
                ids[i] = id;
            }
            if (ids.Distinct().Count() != TOTAL_CASILLAS) return false;
            LlenarMatriz(ids);
            ReiniciarTapas();
            return true;
        }

        // --- Juego ---

        /// <summary>Alterna tapa solo si la carta fue cantada. Devuelve true si fue legal.</summary>
        public bool AlternarTapa(int fila, int col)
        {
            int idCarta = Casillas[fila, col];
            if (!CartasCantadas.Contains(idCarta)) return false;
            Tapas[fila, col] = !Tapas[fila, col];
            return true;
        }

        public void PonerTapa(int fila, int col, bool estado)
        {
            if (fila < 0 || fila >= FILAS)   throw new ArgumentOutOfRangeException(nameof(fila));
            if (col  < 0 || col  >= COLUMNAS) throw new ArgumentOutOfRangeException(nameof(col));
            Tapas[fila, col] = estado;
        }

        public (int fila, int col) BuscarId(int id)
        {
            for (int f = 0; f < FILAS; f++)
                for (int c = 0; c < COLUMNAS; c++)
                    if (Casillas[f, c] == id) return (f, c);
            return (-1, -1);
        }

        // --- Helpers privados ---

        private void LlenarMatriz(int[] ids)
        {
            int idx = 0;
            for (int f = 0; f < FILAS; f++)
                for (int c = 0; c < COLUMNAS; c++)
                    Casillas[f, c] = ids[idx++];
        }

        private void ReiniciarTapas()
        {
            CartasCantadas.Clear();
            for (int f = 0; f < FILAS; f++)
                for (int c = 0; c < COLUMNAS; c++)
                    Tapas[f, c] = false;
        }
    }
}
