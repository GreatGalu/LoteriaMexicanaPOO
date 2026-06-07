using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LoteriaMexicana.Core;
using LoteriaMexicana.Logic;
using LoteriaMexicana.Models;

namespace LoteriaMexicana.UI.UserControls
{
    public partial class UcCreadorTablas : UserControl
    {
        public event Action<Tablero> OnTablaConfirmada;
        public event Action OnCancelado;

        private readonly int[] _ids = new int[Tablero.TOTAL_CASILLAS];
        private int _cursor = 0;
        private readonly PictureBox[,] _celdas = new PictureBox[Tablero.FILAS, Tablero.COLUMNAS];
        private readonly int _numeroTabla;

        public UcCreadorTablas(int numeroTabla = 1)
        {
            _numeroTabla = numeroTabla;
            InitializeComponent();

            lblTituloCreador.Text = $"TABLA {_numeroTabla} — Elige 20 cartas en orden";
            btnConfirmar.Click += btnConfirmar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnAleatorio.Click += btnAleatorio_Click;

            ConstruirGrid();
            CargarCatalogo();
            ActualizarContador();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (_cursor < Tablero.TOTAL_CASILLAS)
            {
                MessageBox.Show("Debes seleccionar las 20 cartas antes de confirmar.",
                    "Tabla Incompleta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                var tablero = new Tablero();
                tablero.CargarDesdeIds(_ids.Take(Tablero.TOTAL_CASILLAS).ToArray());
                OnTablaConfirmada?.Invoke(tablero);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la tabla:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Array.Clear(_ids, 0, _ids.Length);
            _cursor = 0;
            RefrescarGrid();
            ActualizarContador();
        }

        private void btnAleatorio_Click(object sender, EventArgs e)
        {
            var rand = new Random();
            int[] todosLosIds = CatalogoCartas.Todas.Select(c => c.Id).ToArray();
            int[] seleccionados = todosLosIds.OrderBy(x => rand.Next()).Take(Tablero.TOTAL_CASILLAS).ToArray();

            Array.Copy(seleccionados, _ids, Tablero.TOTAL_CASILLAS);
            _cursor = Tablero.TOTAL_CASILLAS;

            RefrescarGrid();
            ActualizarContador();
        }

        private void CartaCatalogo_Click(object sender, EventArgs e)
        {
            if (_cursor >= Tablero.TOTAL_CASILLAS) return;
            int id = (int)((PictureBox)sender).Tag;

            if (_ids.Take(_cursor).Contains(id))
            {
                MessageBox.Show($"La carta [{id}] ya está en tu tabla.",
                    "Repetida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _ids[_cursor++] = id;
            RefrescarGrid();
            ActualizarContador();
        }

        private void QuitarDesdeCelda(int fila, int col)
        {
            int pos = fila * Tablero.COLUMNAS + col;
            if (pos >= _cursor) return;

            for (int i = pos; i < _cursor - 1; i++)
                _ids[i] = _ids[i + 1];

            _ids[--_cursor] = 0;

            RefrescarGrid();
            ActualizarContador();
        }

        private void RefrescarGrid()
        {
            for (int f = 0; f < Tablero.FILAS; f++)
            {
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    int pos = f * Tablero.COLUMNAS + c;
                    var celda = _celdas[f, c];

                    if (pos < _cursor)
                    {
                        int id = _ids[pos];
                        celda.Image = GestorArchivos.CargarImagen(id) ?? GenerarFallback(id);
                        celda.Tag = id;
                        celda.BackColor = Color.FromArgb(45, 45, 50);
                    }
                    else
                    {
                        celda.Image = null;
                        celda.Tag = -1;
                        celda.BackColor = Color.FromArgb(36, 36, 40);
                    }
                }
            }
        }

        private void ActualizarContador()
        {
            lblContador.Text = $"{_cursor} / {Tablero.TOTAL_CASILLAS} seleccionadas";
            btnConfirmar.Enabled = (_cursor == Tablero.TOTAL_CASILLAS);
        }

        private void CargarCatalogo()
        {
            panelCatalogo.SuspendLayout();
            panelCatalogo.Controls.Clear();

            foreach (var carta in CatalogoCartas.Todas)
            {
                Image img = GestorArchivos.CargarImagen(carta.Id) ?? GenerarFallback(carta.Id);
                var pic = new PictureBox
                {
                    Size = new Size(62, 82),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.FromArgb(36, 36, 40),
                    Image = img,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(4),
                    Tag = carta.Id
                };

                new ToolTip().SetToolTip(pic, $"[{carta.Id}] {carta.Nombre}");
                pic.Click += CartaCatalogo_Click;
                panelCatalogo.Controls.Add(pic);
            }
            panelCatalogo.ResumeLayout();
        }

        private static Image GenerarFallback(int id)
        {
            var bmp = new Bitmap(62, 82);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(42, 42, 48));
            using var f = new Font("Segoe UI", 12, FontStyle.Bold);
            string txt = id.ToString();
            SizeF medida = g.MeasureString(txt, f);
            g.DrawString(txt, f, new SolidBrush(Color.FromArgb(160, 160, 165)),
                (62f - medida.Width) / 2f, (82f - medida.Height) / 2f);
            return bmp;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            foreach (var pic in _celdas) pic?.Image?.Dispose();
            base.OnHandleDestroyed(e);
        }
    }
}