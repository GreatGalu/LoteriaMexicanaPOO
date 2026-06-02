using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LoteriaMexicana.Core;
using LoteriaMexicana.Logic;
using LoteriaMexicana.Models;

namespace LoteriaMexicana.UI
{
    public class FormCrearTabla : Form
    {
        public Tablero TableroResultante { get; private set; }

        private FlowLayoutPanel _panelCatalogo;
        private Panel _panelGrid;
        private Button _btnConfirmar;
        private Button _btnLimpiar;
        private Label _lblContador;
        private readonly int[] _ids = new int[Tablero.TOTAL_CASILLAS];
        private int _cursor = 0;
        private readonly PictureBox[,] _celdas = new PictureBox[Tablero.FILAS, Tablero.COLUMNAS];
        public FormCrearTabla()
        {
            Text = "Crear tabla personalizada";
            Size = new Size(1000, 660);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(40, 40, 42);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            ConstruirUI();
            CargarCatalogo();
        }
        private void ConstruirUI()
        {
            var panelDerecho = new Panel
            {
                Width = 580,
                Dock = DockStyle.Right,
                BackColor = Color.FromArgb(35, 35, 37),
                Padding = new Padding(6)
            };

            var lblCatalogo = new Label
            {
                Text = "CATALOGO — haz clic en una carta para agregarla",
                ForeColor = Color.FromArgb(180, 180, 180),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 26,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(4, 0, 0, 0)
            };

            _panelCatalogo = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(35, 35, 37),
                Padding = new Padding(4)
            };

            panelDerecho.Controls.Add(_panelCatalogo);
            panelDerecho.Controls.Add(lblCatalogo);

            var panelIzquierdo = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(40, 40, 42),
                Padding = new Padding(10)
            };

            var lblTitulo = new Label
            {
                Text = "TU TABLA — selecciona 20 cartas en orden",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 28
            };

            _lblContador = new Label
            {
                Text = "0 / 20 seleccionadas",
                ForeColor = Color.FromArgb(120, 200, 120),
                Font = new Font("Segoe UI", 9),
                Dock = DockStyle.Top,
                Height = 22
            };

            // Grid 4x5 para la tabla en construccion
            _panelGrid = new Panel
            {
                Dock = DockStyle.Top,
                Height = 460,
                BackColor = Color.FromArgb(38, 38, 40)
            };
            ConstruirGrid();

            var lblTip = new Label
            {
                Text = "Clic en una casilla ocupada → la quita y compacta el resto",
                ForeColor = Color.FromArgb(130, 130, 130),
                Font = new Font("Segoe UI", 8),
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 46,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.FromArgb(30, 30, 32),
                Padding = new Padding(6)
            };

            _btnConfirmar = new Button
            {
                Text = "  Confirmar tabla",
                Width = 160,
                Height = 32,
                BackColor = Color.FromArgb(40, 130, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Enabled = false
            };
            _btnConfirmar.Click += BtnConfirmar_Click;

            _btnLimpiar = new Button
            {
                Text = "  Limpiar todo",
                Width = 120,
                Height = 32,
                BackColor = Color.FromArgb(120, 40, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            _btnLimpiar.Click += BtnLimpiar_Click;

            panelBotones.Controls.AddRange(new Control[] { _btnConfirmar, _btnLimpiar });

            panelIzquierdo.Controls.Add(panelBotones);
            panelIzquierdo.Controls.Add(lblTip);       
            panelIzquierdo.Controls.Add(_panelGrid);      
            panelIzquierdo.Controls.Add(_lblContador);    
            panelIzquierdo.Controls.Add(lblTitulo);       

            Controls.Add(panelDerecho);
            Controls.Add(panelIzquierdo);
        }

        private void ConstruirGrid()
        {
            const int ancho = 74, alto = 97, espacio = 3, padX = 10, padY = 6;

            for (int f = 0; f < Tablero.FILAS; f++)
            {
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    var pic = new PictureBox
                    {
                        Width = ancho,
                        Height = alto,
                        Left = padX + c * (ancho + espacio),
                        Top = padY + f * (alto + espacio),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.FromArgb(55, 55, 58),
                        Cursor = Cursors.Hand,
                        Tag = -1  
                    };

                    var lbl = new Label
                    {
                        Text = $"{f * Tablero.COLUMNAS + c + 1}",
                        ForeColor = Color.FromArgb(100, 100, 100),
                        Font = new Font("Segoe UI", 7),
                        AutoSize = true,
                        Location = new Point(2, 1),
                        BackColor = Color.Transparent
                    };
                    pic.Controls.Add(lbl);

                    int fi = f, co = c;
                    pic.Click += (s, e) => QuitarDesdeCelda(fi, co);

                    _celdas[f, c] = pic;
                    _panelGrid.Controls.Add(pic);
                }
            }
        }
        private void CargarCatalogo()
        {
            foreach (var carta in CatalogoCartas.Todas)
            {
                Image img = GestorArchivos.CargarImagen(carta.Id)
                            ?? GenerarFallback(carta.Id);

                var pic = new PictureBox
                {
                    Size = new Size(62, 82),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.FromArgb(55, 55, 58),
                    Image = img,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(3),
                    Tag = carta.Id
                };

                var tooltip = new ToolTip();
                tooltip.SetToolTip(pic, $"[{carta.Id}] {carta.Nombre}");

                pic.Click += CartaCatalogo_Click;
                _panelCatalogo.Controls.Add(pic);
            }
        }
        private void CartaCatalogo_Click(object sender, EventArgs e)
        {
            if (_cursor >= Tablero.TOTAL_CASILLAS) return;

            int id = (int)((PictureBox)sender).Tag;

            if (_ids.Take(_cursor).Contains(id))
            {
                MessageBox.Show($"La carta [{id}] ya esta en tu tabla.",
                    "Repetida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _ids[_cursor] = id;
            _cursor++;

            RefrescarGrid();
            ActualizarContador();
        }

        private void QuitarDesdeCelda(int fila, int col)
        {
            int pos = fila * Tablero.COLUMNAS + col;
            if (pos >= _cursor) return; 

            for (int i = pos; i < _cursor - 1; i++)
                _ids[i] = _ids[i + 1];

            _ids[_cursor - 1] = 0;
            _cursor--;

            RefrescarGrid();
            ActualizarContador();
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (_cursor < Tablero.TOTAL_CASILLAS) return;

            try
            {
                int[] ids = _ids.Take(Tablero.TOTAL_CASILLAS).ToArray();

                var tablero = new Tablero();
                tablero.CargarDesdeIds(ids);     

                TableroResultante = tablero;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la tabla:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            Array.Clear(_ids, 0, _ids.Length);
            _cursor = 0;
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
                        celda.Image?.Dispose();
                        celda.Image = GestorArchivos.CargarImagen(id) ?? GenerarFallback(id);
                        celda.Tag = id;
                        celda.BackColor = Color.FromArgb(45, 45, 48);
                    }
                    else
                    {
                        celda.Image?.Dispose();
                        celda.Image = null;
                        celda.Tag = -1;
                        celda.BackColor = Color.FromArgb(55, 55, 58);
                    }
                }
            }
        }

        private void ActualizarContador()
        {
            _lblContador.Text = $"{_cursor} / {Tablero.TOTAL_CASILLAS} seleccionadas";
            _btnConfirmar.Enabled = (_cursor == Tablero.TOTAL_CASILLAS);
        }

        private static Image GenerarFallback(int id)
        {
            var bmp = new Bitmap(62, 82);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(55, 55, 58));
            using var f = new Font("Segoe UI", 11, FontStyle.Bold);
            string txt = id.ToString();
            SizeF medida = g.MeasureString(txt, f);
            g.DrawString(txt, f, Brushes.LightGray,
                (62f - medida.Width) / 2f,
                (82f - medida.Height) / 2f);
            return bmp;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            foreach (var pic in _celdas)
                pic?.Image?.Dispose();

            base.OnFormClosing(e);
        }
    }
}