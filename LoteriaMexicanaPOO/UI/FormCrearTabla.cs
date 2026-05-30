using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LoteriaMexicana.Core;
using LoteriaMexicana.Logic;
using LoteriaMexicana.Models;

namespace LoteriaMexicana.UI
{
    /// <summary>
    /// Formulario que permite al jugador armar su propia tabla de 4×5
    /// eligiendo 20 cartas del catálogo de 54, sin repetir ninguna.
    ///
    /// Resultado: si el usuario confirma, TableroResultante contiene
    /// el Tablero cargado con su selección personalizada.
    /// </summary>
    public class FormCrearTabla : Form
    {
        // ── Resultado que el llamante consulta ────────────────────
        public Tablero TableroResultante { get; private set; }

        // ── Controles ─────────────────────────────────────────────
        private FlowLayoutPanel _panelCatalogo;
        private Panel           _panelGridSeleccion;
        private Button          _btnConfirmar;
        private Button          _btnLimpiar;
        private Label           _lblContador;
        private PictureBox      _picPreview;

        // ── Estado ────────────────────────────────────────────────
        private readonly int[] _seleccionados = new int[Tablero.TOTAL_CASILLAS];
        private int _cursor = 0;   // Siguiente posición libre en _seleccionados

        // PictureBox de las 20 posiciones de la tabla en construcción
        private readonly PictureBox[,] _celdas = new PictureBox[Tablero.FILAS, Tablero.COLUMNAS];

        // ── Constructor ───────────────────────────────────────────

        public FormCrearTabla()
        {
            Text            = "Crear tabla personalizada";
            Size            = new Size(980, 640);
            StartPosition   = FormStartPosition.CenterParent;
            BackColor       = Color.FromArgb(40, 40, 42);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;

            ConstruirUI();
            CargarCatalogo();
        }

        // ── Construcción de la UI ─────────────────────────────────

        private void ConstruirUI()
        {
            // ── Panel derecho: catálogo de cartas ─────────────────
            var panelDerecho = new Panel
            {
                Width     = 570,
                Dock      = DockStyle.Right,
                BackColor = Color.FromArgb(35, 35, 37),
                Padding   = new Padding(8)
            };

            var lblCatalogo = new Label
            {
                Text      = "CATÁLOGO — haz clic para agregar",
                ForeColor = Color.FromArgb(180, 180, 180),
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                Dock      = DockStyle.Top,
                Height    = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(4, 0, 0, 0)
            };

            _panelCatalogo = new FlowLayoutPanel
            {
                Dock        = DockStyle.Fill,
                AutoScroll  = true,
                BackColor   = Color.FromArgb(35, 35, 37),
                Padding     = new Padding(4)
            };

            panelDerecho.Controls.Add(_panelCatalogo);
            panelDerecho.Controls.Add(lblCatalogo);

            // ── Panel izquierdo: tabla en construcción ────────────
            var panelIzquierdo = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = Color.FromArgb(40, 40, 42),
                Padding   = new Padding(12)
            };

            var lblTitulo = new Label
            {
                Text      = "TU TABLA (selecciona 20 cartas en orden)",
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock      = DockStyle.Top,
                Height    = 28
            };

            _lblContador = new Label
            {
                Text      = "0 / 20 seleccionadas",
                ForeColor = Color.FromArgb(120, 200, 120),
                Font      = new Font("Segoe UI", 9),
                Dock      = DockStyle.Top,
                Height    = 22
            };

            _panelGridSeleccion = new Panel
            {
                Height    = 460,
                Dock      = DockStyle.Top,
                BackColor = Color.FromArgb(38, 38, 40)
            };
            ConstruirGridSeleccion();

            // Botones inferiores
            var panelBotones = new FlowLayoutPanel
            {
                Dock          = DockStyle.Bottom,
                Height        = 44,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor     = Color.FromArgb(30, 30, 32),
                Padding       = new Padding(6, 6, 6, 6)
            };

            _btnConfirmar = new Button
            {
                Text      = "✔  Confirmar tabla",
                Width     = 160,
                Height    = 30,
                BackColor = Color.FromArgb(40, 130, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                Enabled   = false
            };
            _btnConfirmar.Click += BtnConfirmar_Click;

            _btnLimpiar = new Button
            {
                Text      = "✖  Limpiar",
                Width     = 110,
                Height    = 30,
                BackColor = Color.FromArgb(120, 40, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9)
            };
            _btnLimpiar.Click += BtnLimpiar_Click;

            panelBotones.Controls.AddRange(new Control[] { _btnConfirmar, _btnLimpiar });

            panelIzquierdo.Controls.Add(panelBotones);
            panelIzquierdo.Controls.Add(_panelGridSeleccion);
            panelIzquierdo.Controls.Add(_lblContador);
            panelIzquierdo.Controls.Add(lblTitulo);

            Controls.Add(panelDerecho);
            Controls.Add(panelIzquierdo);
        }

        private void ConstruirGridSeleccion()
        {
            const int ancho = 72, alto = 95, espacio = 3, padX = 10, padY = 8;

            for (int f = 0; f < Tablero.FILAS; f++)
            {
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    var pic = new PictureBox
                    {
                        Width       = ancho,
                        Height      = alto,
                        Left        = padX + c * (ancho + espacio),
                        Top         = padY + f * (alto  + espacio),
                        SizeMode    = PictureBoxSizeMode.Zoom,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor   = Color.FromArgb(55, 55, 58),
                        Cursor      = Cursors.Hand,
                        Tag         = -1   // -1 = vacía
                    };

                    // Etiqueta de número en la esquina superior izquierda
                    var lbl = new Label
                    {
                        Text      = $"{f * Tablero.COLUMNAS + c + 1}",
                        ForeColor = Color.FromArgb(100, 100, 100),
                        Font      = new Font("Segoe UI", 7),
                        AutoSize  = true,
                        Location  = new Point(2, 1),
                        BackColor = Color.Transparent
                    };
                    pic.Controls.Add(lbl);

                    int fi = f, co = c;
                    pic.Click += (s, e) => QuitarCelda(fi, co);

                    _celdas[f, c] = pic;
                    _panelGridSeleccion.Controls.Add(pic);
                }
            }
        }

        // ── Carga del catálogo ────────────────────────────────────

        private void CargarCatalogo()
        {
            foreach (var carta in CatalogoCartas.Todas)
            {
                var pic = new PictureBox
                {
                    Size        = new Size(60, 80),
                    SizeMode    = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor   = Color.FromArgb(55, 55, 58),
                    Image       = GestorArchivos.CargarImagen(carta.Id) ?? GenerarImagenFallback(carta),
                    Cursor      = Cursors.Hand,
                    Margin      = new Padding(3),
                    Tag         = carta.Id
                };

                var tooltip = new ToolTip();
                tooltip.SetToolTip(pic, $"[{carta.Id:D2}] {carta.Nombre}");

                pic.Click += CartaCatalogo_Click;
                _panelCatalogo.Controls.Add(pic);
            }
        }

        // ── Eventos de interacción ────────────────────────────────

        private void CartaCatalogo_Click(object sender, EventArgs e)
        {
            if (_cursor >= Tablero.TOTAL_CASILLAS) return;

            var pic = (PictureBox)sender;
            int id  = (int)pic.Tag;

            // Validación: no repetir
            if (_seleccionados.Take(_cursor).Contains(id))
            {
                MessageBox.Show($"La carta [{id:D2}] ya está en tu tabla.", "Repetida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Colocar en la siguiente celda libre
            int fila = _cursor / Tablero.COLUMNAS;
            int col  = _cursor % Tablero.COLUMNAS;

            _seleccionados[_cursor] = id;
            _cursor++;

            // Actualizar celda visual
            _celdas[fila, col].Image   = GestorArchivos.CargarImagen(id)
                                         ?? GenerarImagenFallback(CatalogoCartas.BuscarPorId(id));
            _celdas[fila, col].Tag     = id;
            _celdas[fila, col].BackColor = Color.FromArgb(45, 45, 48);

            ActualizarContador();
        }

        private void QuitarCelda(int fila, int col)
        {
            int pos = fila * Tablero.COLUMNAS + col;
            if (pos >= _cursor) return;  // Celda vacía

            // Compactar: mover todas las cartas posteriores una posición atrás
            for (int i = pos; i < _cursor - 1; i++)
                _seleccionados[i] = _seleccionados[i + 1];

            _seleccionados[_cursor - 1] = 0;
            _cursor--;

            RefrescarGridSeleccion();
            ActualizarContador();
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (_cursor < Tablero.TOTAL_CASILLAS) return;

            var tablero = new Tablero();
            tablero.CargarDesdeIds(_seleccionados.Take(Tablero.TOTAL_CASILLAS).ToArray());

            TableroResultante = tablero;
            DialogResult      = DialogResult.OK;
            Close();
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            Array.Clear(_seleccionados, 0, _seleccionados.Length);
            _cursor = 0;
            RefrescarGridSeleccion();
            ActualizarContador();
        }

        // ── Helpers de UI ─────────────────────────────────────────

        private void RefrescarGridSeleccion()
        {
            for (int f = 0; f < Tablero.FILAS; f++)
            {
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    int pos = f * Tablero.COLUMNAS + c;
                    if (pos < _cursor)
                    {
                        int id = _seleccionados[pos];
                        _celdas[f, c].Image    = GestorArchivos.CargarImagen(id)
                                                 ?? GenerarImagenFallback(CatalogoCartas.BuscarPorId(id));
                        _celdas[f, c].Tag      = id;
                        _celdas[f, c].BackColor = Color.FromArgb(45, 45, 48);
                    }
                    else
                    {
                        _celdas[f, c].Image    = null;
                        _celdas[f, c].Tag      = -1;
                        _celdas[f, c].BackColor = Color.FromArgb(55, 55, 58);
                    }
                }
            }
        }

        private void ActualizarContador()
        {
            _lblContador.Text     = $"{_cursor} / {Tablero.TOTAL_CASILLAS} seleccionadas";
            _btnConfirmar.Enabled = (_cursor == Tablero.TOTAL_CASILLAS);
        }

        private static Image GenerarImagenFallback(Carta carta)
        {
            if (carta == null) return null;
            var bmp = new Bitmap(60, 80);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(55, 55, 58));
            using var f = new Font("Segoe UI", 8, FontStyle.Bold);
            g.DrawString(carta.Id.ToString("D2"), f, Brushes.LightGray, 18, 28);
            return bmp;
        }
    }
}
