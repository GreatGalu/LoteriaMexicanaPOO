using LoteriaMexicana.Logic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LoteriaMexicana.UI.UserControls
{
    partial class UcCreadorTablas
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        internal Label lblTituloCreador;
        internal Label lblContador;
        internal Panel panelGrid;
        internal FlowLayoutPanel panelCatalogo;
        internal Button btnConfirmar;
        internal Button btnLimpiar;
        internal Button btnAleatorio;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTituloCreador = new Label();
            lblContador = new Label();
            panelGrid = new Panel();
            panelCatalogo = new FlowLayoutPanel();
            btnConfirmar = new Button();
            btnLimpiar = new Button();
            btnAleatorio = new Button();

            this.SuspendLayout();

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(24, 24, 26);

            lblTituloCreador.Text = $"TABLA — elige 20 cartas en orden";
            lblTituloCreador.Font = ResolverFuente(14, FontStyle.Bold);
            lblTituloCreador.ForeColor = Color.FromArgb(230, 180, 60);
            lblTituloCreador.Dock = DockStyle.Top;
            lblTituloCreador.Height = 42;
            lblTituloCreador.TextAlign = ContentAlignment.MiddleCenter;

            lblContador.Text = "0 / 20 seleccionadas";
            lblContador.Font = ResolverFuente(10);
            lblContador.ForeColor = Color.FromArgb(120, 200, 120);
            lblContador.Dock = DockStyle.Top;
            lblContador.Height = 22;
            lblContador.TextAlign = ContentAlignment.MiddleCenter;
            var split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.BackColor = Color.FromArgb(24, 24, 26);
            split.BorderStyle = BorderStyle.None;
            split.Panel1MinSize = 50;
            split.Panel2MinSize = 50;
            split.Panel1.BackColor = Color.FromArgb(32, 32, 36);
            split.Panel2.BackColor = Color.FromArgb(28, 28, 32);

            split.SizeChanged += (s, e) =>
            {
                if (split.Width <= split.Panel1MinSize + split.Panel2MinSize) return;
                int distancia = (int)(split.Width * 0.60);
                distancia = Math.Max(distancia, split.Panel1MinSize);
                distancia = Math.Min(distancia, split.Width - split.Panel2MinSize);
                try { split.SplitterDistance = distancia; } catch { }
            };

            panelGrid.Dock = DockStyle.Fill;
            panelGrid.BackColor = Color.FromArgb(32, 32, 36);
            panelGrid.Padding = new Padding(16, 10, 16, 10);
            ConstruirGrid();

            var panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.FromArgb(28, 28, 32),
                Padding = new Padding(8, 8, 8, 8)
            };

            AplicarEstiloBoton(btnConfirmar, "  Confirmar", Color.FromArgb(30, 120, 60));
            AplicarEstiloBoton(btnLimpiar, "  Limpiar", Color.FromArgb(120, 40, 40));
            AplicarEstiloBoton(btnAleatorio, "  Aleatorio", Color.FromArgb(60, 60, 80));
            btnConfirmar.Width = 140;
            btnLimpiar.Width = 110;
            btnAleatorio.Width = 120;
            btnConfirmar.Click += btnConfirmar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnAleatorio.Click += btnAleatorio_Click;

            panelBotones.Controls.AddRange(new Control[] { btnConfirmar, btnLimpiar, btnAleatorio });

            split.Panel1.Controls.Add(panelGrid);
            split.Panel1.Controls.Add(panelBotones);

            var lblCatalogoTitulo = new Label
            {
                Text = "CATÁLOGO — haz clic para agregar",
                Font = ResolverFuente(9, FontStyle.Bold),
                ForeColor = Color.FromArgb(160, 160, 165),
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panelCatalogo.Dock = DockStyle.Fill;
            panelCatalogo.AutoScroll = true;
            panelCatalogo.BackColor = Color.FromArgb(28, 28, 32);
            panelCatalogo.Padding = new Padding(6);

            split.Panel2.Controls.Add(panelCatalogo);
            split.Panel2.Controls.Add(lblCatalogoTitulo);

            this.Controls.Add(split);
            this.Controls.Add(lblContador);
            this.Controls.Add(lblTituloCreador);
            this.ResumeLayout(false);
        }

        private void ConstruirGrid()
        {
            const int ancho = 74, alto = 97, esp = 3, padX = 8, padY = 6;

            for (int f = 0; f < Tablero.FILAS; f++)
            {
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    var pic = new PictureBox
                    {
                        Width = ancho,
                        Height = alto,
                        Left = padX + c * (ancho + esp),
                        Top = padY + f * (alto + esp),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.FromArgb(36, 36, 40),
                        Cursor = Cursors.Hand,
                        Tag = -1
                    };

                    var lbl = new Label
                    {
                        Text = $"{f * Tablero.COLUMNAS + c + 1}",
                        ForeColor = Color.FromArgb(80, 80, 85),
                        Font = ResolverFuente(7),
                        AutoSize = true,
                        Location = new Point(2, 1),
                        BackColor = Color.Transparent
                    };
                    pic.Controls.Add(lbl);

                    int fi = f, co = c;
                    pic.Click += (s, e) => QuitarDesdeCelda(fi, co);

                    _celdas[f, c] = pic;
                    panelGrid.Controls.Add(pic);
                }
            }
        }
        private static Font ResolverFuente(float size, FontStyle style = FontStyle.Regular)
        {
            foreach (string nombre in new[] { "Helvetica", "Arial", "Segoe UI" })
            {
                try
                {
                    using var prueba = new Font(nombre, 1);
                    if (prueba.Name == nombre)
                        return new Font(nombre, size, style);
                }
                catch { }
            }
            return new Font("Segoe UI", size, style);
        }

        private static void AplicarEstiloBoton(Button b, string texto, Color acento)
        {
            b.Text = texto;
            b.Font = ResolverFuente(9, FontStyle.Bold);
            b.BackColor = acento;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Cursor = Cursors.Hand;
            b.Height = 32;
            b.Margin = new Padding(4, 0, 0, 0);
        }
    }
}