using System;
using System.Drawing;
using System.Windows.Forms;
using LoteriaMexicana.Logic;
using LoteriaMexicana.Core;

namespace LoteriaMexicana.Controllers
{
    public class ControladorTablero
    {
        private const int ANCHO_CELDA  = 80;
        private const int ALTO_CELDA   = 105;
        private const int ESPACIO      = 4;
        private const int PADDING_IZQUIERDO = 16;
        private const int PADDING_SUPERIOR  = 28;

        private readonly Tablero      _tablero;
        private readonly PictureBox[,] _celdas;
        private readonly int          _indice; 

        public event Action<int, int, int> OnTapaAlternada;

        public ControladorTablero(Tablero tablero, int indice)
        {
            _tablero = tablero;
            _indice  = indice;
            _celdas  = new PictureBox[Tablero.FILAS, Tablero.COLUMNAS];
        }
        public GroupBox ConstruirGrupBox()
        {
            int anchoTotal = PADDING_IZQUIERDO * 2 + (ANCHO_CELDA * Tablero.COLUMNAS) + (ESPACIO * (Tablero.COLUMNAS - 1));
            int altoTotal  = PADDING_SUPERIOR  + 10 + (ALTO_CELDA  * Tablero.FILAS)   + (ESPACIO * (Tablero.FILAS - 1));

            var grupo = new GroupBox
            {
                Text      = $"  TABLA {_indice + 1}  ",
                ForeColor = Color.FromArgb(200, 200, 200),
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                Width     = anchoTotal,
                Height    = altoTotal,
                Margin    = new Padding(18, 10, 18, 10),
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

        public void MarcarCartaCantada(int fila, int col)
        {
            if (!CeldaValida(fila, col)) return;
            _celdas[fila, col].Tag = "cantada";
            AplicarEstiloEstado(_celdas[fila, col], tapada: _tablero.Tapas[fila, col], cantada: true);
        }
        public void RefrescarVisual()
        {
            for (int f = 0; f < Tablero.FILAS; f++)
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                    AplicarEstiloEstado(_celdas[f, c],
                        tapada:  _tablero.Tapas[f, c],
                        cantada: _tablero.CartasCantadas.Contains(_tablero.Casillas[f, c]));
        }
        private PictureBox CrearCelda(int idCarta, int fila, int col)
        {
            var pic = new PictureBox
            {
                Width   = ANCHO_CELDA,
                Height  = ALTO_CELDA,
                Left    = PADDING_IZQUIERDO + col * (ANCHO_CELDA + ESPACIO),
                Top     = PADDING_SUPERIOR  + fila * (ALTO_CELDA  + ESPACIO),
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
        private PictureBox CrearCeldaTextoFallback(int id, int fila, int col, PictureBox base_pic)
        {
            var bmp = new Bitmap(ANCHO_CELDA, ALTO_CELDA);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(55, 55, 58));
            using var fuente = new Font("Segoe UI", 11, FontStyle.Bold);
            var texto  = id.ToString("D2");
            var medida = g.MeasureString(texto, fuente);
            g.DrawString(texto, fuente, Brushes.LightGray,
                (ANCHO_CELDA - medida.Width) / 2f,
                (ALTO_CELDA  - medida.Height) / 2f);

            base_pic.Image = bmp;
            return base_pic;
        }
        private void AlternarTapa(int fila, int col)
        {
            bool jugadaLegal = _tablero.AlternarTapa(fila, col);
            if (!jugadaLegal) return;

            bool tapada  = _tablero.Tapas[fila, col];
            bool cantada = _tablero.CartasCantadas.Contains(_tablero.Casillas[fila, col]);
            AplicarEstiloEstado(_celdas[fila, col], tapada, cantada);

            OnTapaAlternada?.Invoke(_indice, fila, col);
        }
        private static void AplicarEstiloEstado(PictureBox pic, bool tapada, bool cantada)
        {
            if (tapada)
            {
                pic.BackColor   = Color.FromArgb(200, 120, 40);
                pic.BorderStyle = BorderStyle.Fixed3D;
            }
            else if (cantada)
            {
                pic.BackColor   = Color.FromArgb(50, 50, 52);
                pic.BorderStyle = BorderStyle.Fixed3D;            }
            else
            {
                pic.BackColor   = Color.FromArgb(50, 50, 52);
                pic.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private bool CeldaValida(int f, int c) =>
            f >= 0 && f < Tablero.FILAS && c >= 0 && c < Tablero.COLUMNAS;
    }
}
