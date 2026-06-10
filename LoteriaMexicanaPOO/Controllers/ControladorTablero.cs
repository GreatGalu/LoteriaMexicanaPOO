using System;
using System.Drawing;
using System.Windows.Forms;
using LoteriaMexicana.Logic;
using LoteriaMexicana.Core;

namespace LoteriaMexicana.Controllers
{
    public class ControladorTablero
    {
        private const int ESPACIO           = 4;
        private const int PADDING_IZQUIERDO = 14;
        private const int PADDING_SUPERIOR  = 26;

        // ── Dimensiones adaptadas al número de tablas activas ───────────────
        private readonly int _anchoCelda;
        private readonly int _altoCelda;

        private readonly Tablero       _tablero;
        private readonly PictureBox[,] _celdas;
        private readonly int           _indice;

        public event Action<int, int, int> OnTapaAlternada;

        /// <param name="totalTablas">Número total de tablas para escalar visualmente.</param>
        public ControladorTablero(Tablero tablero, int indice, int totalTablas = 1)
        {
            _tablero = tablero;
            _indice  = indice;
            _celdas  = new PictureBox[Tablero.FILAS, Tablero.COLUMNAS];

            // Tamaño dinámico de celda según cuántas tablas coexisten
            if (totalTablas <= 2)      { _anchoCelda = 88; _altoCelda = 118; }
            else if (totalTablas <= 4) { _anchoCelda = 74; _altoCelda = 98;  }
            else                       { _anchoCelda = 60; _altoCelda = 80;  }
        }

        public GroupBox ConstruirGrupBox()
        {
            int anchoTotal = PADDING_IZQUIERDO * 2
                             + (_anchoCelda * Tablero.COLUMNAS)
                             + (ESPACIO * (Tablero.COLUMNAS - 1));
            int altoTotal  = PADDING_SUPERIOR + 10
                             + (_altoCelda  * Tablero.FILAS)
                             + (ESPACIO * (Tablero.FILAS - 1));

            var grupo = new GroupBox
            {
                Text      = $"  TABLA {_indice + 1}  ",
                ForeColor = Color.FromArgb(200, 200, 200),
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                Width     = anchoTotal,
                Height    = altoTotal,
                Margin    = new Padding(14, 10, 14, 10),
                BackColor = Color.FromArgb(38, 38, 40),
                FlatStyle = FlatStyle.Flat
            };

            for (int f = 0; f < Tablero.FILAS; f++)
            {
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    int idCarta = _tablero.Casillas[f, c];
                    var pic     = CrearCelda(idCarta, f, c);
                    _celdas[f, c] = pic;
                    grupo.Controls.Add(pic);
                }
            }

            return grupo;
        }

        /// <summary>
        /// Se llama cuando el gritón anuncia una carta; ya no cambia el estilo visual.
        /// Solo registra internamente (el estado ya está en CartasCantadas del Tablero).
        /// </summary>
        public void MarcarCartaCantada(int fila, int col)
        {
            // Comportamiento visual suprimido según "Ocultar Carta Cantada".
            // No se aplica ningún resalte especial para cartas anunciadas.
        }

        public void RefrescarVisual()
        {
            for (int f = 0; f < Tablero.FILAS; f++)
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                    AplicarEstiloEstado(_celdas[f, c], tapada: _tablero.Tapas[f, c]);
        }

        private PictureBox CrearCelda(int idCarta, int fila, int col)
        {
            var pic = new PictureBox
            {
                Width       = _anchoCelda,
                Height      = _altoCelda,
                Left        = PADDING_IZQUIERDO + col * (_anchoCelda + ESPACIO),
                Top         = PADDING_SUPERIOR  + fila * (_altoCelda  + ESPACIO),
                SizeMode    = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor      = Cursors.Hand,
                BackColor   = Color.FromArgb(50, 50, 52),
                Image       = GestorArchivos.CargarImagen(idCarta),
                Tag         = "normal"
            };

            if (pic.Image == null)
                pic = CrearCeldaTextoFallback(idCarta, fila, col, pic);

            int fi = fila, co = col;
            pic.Click += (s, e) => AlternarTapa(fi, co);

            return pic;
        }

        private PictureBox CrearCeldaTextoFallback(int id, int fila, int col, PictureBox basePic)
        {
            var bmp = new Bitmap(_anchoCelda, _altoCelda);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(55, 55, 58));
            using var fuente = new Font("Segoe UI", 10, FontStyle.Bold);
            var texto  = id.ToString("D2");
            var medida = g.MeasureString(texto, fuente);
            g.DrawString(texto, fuente, Brushes.LightGray,
                (_anchoCelda - medida.Width) / 2f,
                (_altoCelda  - medida.Height) / 2f);
            basePic.Image = bmp;
            return basePic;
        }

        private void AlternarTapa(int fila, int col)
        {
            // Marcado libre: siempre permitido
            _tablero.AlternarTapa(fila, col);
            AplicarEstiloEstado(_celdas[fila, col], tapada: _tablero.Tapas[fila, col]);
            OnTapaAlternada?.Invoke(_indice, fila, col);
        }

        private static void AplicarEstiloEstado(PictureBox pic, bool tapada)
        {
            if (tapada)
            {
                pic.BackColor   = Color.FromArgb(200, 120, 40);
                pic.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                // Sin resalte especial para cartas cantadas: aspecto neutro
                pic.BackColor   = Color.FromArgb(50, 50, 52);
                pic.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private bool CeldaValida(int f, int c) =>
            f >= 0 && f < Tablero.FILAS && c >= 0 && c < Tablero.COLUMNAS;
    }
}
