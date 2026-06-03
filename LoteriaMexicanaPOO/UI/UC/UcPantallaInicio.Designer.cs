using System.Drawing;
using System.Windows.Forms;

namespace LoteriaMexicana.UI.UserControls
{
    partial class UcPantallaInicio
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private Label     lblTitulo;
        private Label     lblSubtitulo;
        private Panel     panelTarjeta;
        private Label     lblNombre;
        private TextBox   txtNombre;
        private Label     lblTablas;
        private NumericUpDown nudTablas;
        private Label     lblSala;
        private TextBox   txtSala;
        private Button    btnCrear;
        private Button    btnUnirse;
        private Label     lblVersion;

        private void InitializeComponent()
        {
            components   = new System.ComponentModel.Container();

            lblTitulo    = new Label();
            lblSubtitulo = new Label();
            panelTarjeta = new Panel();
            lblNombre    = new Label();
            txtNombre    = new TextBox();
            lblTablas    = new Label();
            nudTablas    = new NumericUpDown();
            lblSala      = new Label();
            txtSala      = new TextBox();
            btnCrear     = new Button();
            btnUnirse    = new Button();
            lblVersion   = new Label();

            this.SuspendLayout();

            this.Dock      = DockStyle.Fill;
            this.BackColor = Color.FromArgb(24, 24, 26);

            lblTitulo.Text      = "LOTERÍA MEXICANA";
            lblTitulo.Font      = ResolverFuente(22, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(230, 180, 60);
            lblTitulo.AutoSize  = false;
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Dock      = DockStyle.Top;
            lblTitulo.Height    = 70;
            lblTitulo.Padding   = new Padding(0, 18, 0, 0);

            lblSubtitulo.Text      = "Multijugador Online / LAN";
            lblSubtitulo.Font      = ResolverFuente(10, FontStyle.Regular);
            lblSubtitulo.ForeColor = Color.FromArgb(130, 130, 135);
            lblSubtitulo.AutoSize  = false;
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitulo.Dock      = DockStyle.Top;
            lblSubtitulo.Height    = 28;

            panelTarjeta.BackColor  = Color.FromArgb(32, 32, 36);
            panelTarjeta.Width      = 400;
            panelTarjeta.Height     = 320;
            panelTarjeta.Anchor     = AnchorStyles.None;
            panelTarjeta.Padding    = new Padding(32, 28, 32, 24);

            this.Resize += (s, e) => CentrarTarjeta();

            AplicarEstiloLabel(lblNombre, "Nombre del jugador");
            AplicarEstiloInput(txtNombre);
            txtNombre.PlaceholderText = "Ej. Jugador1";
            txtNombre.TabIndex        = 0;

            AplicarEstiloLabel(lblTablas, "Cantidad de tablas (1-3)");
            nudTablas.Minimum      = 1;
            nudTablas.Maximum      = 3;
            nudTablas.Value        = 1;
            nudTablas.BackColor    = Color.FromArgb(44, 44, 50);
            nudTablas.ForeColor    = Color.White;
            nudTablas.Font         = ResolverFuente(10);
            nudTablas.BorderStyle  = BorderStyle.FixedSingle;
            nudTablas.TabIndex     = 1;

            AplicarEstiloLabel(lblSala, "Codigo de sala (para unirse)");
            AplicarEstiloInput(txtSala);
            txtSala.PlaceholderText = "Codigo 4 letras o IP";
            txtSala.TabIndex        = 2;
            txtSala.KeyDown        += txtSala_KeyDown;

            AplicarEstiloBoton(btnCrear,  "CREAR PARTIDA", Color.FromArgb(180, 120, 20));
            AplicarEstiloBoton(btnUnirse, "UNIRSE",        Color.FromArgb(40, 100, 160));
            btnCrear.TabIndex  = 3;
            btnUnirse.TabIndex = 4;
            btnCrear.Click  += btnCrear_Click;
            btnUnirse.Click += btnUnirse_Click;

            var tabla = new TableLayoutPanel
            {
                Dock        = DockStyle.Fill,
                ColumnCount = 2,
                RowCount    = 6,
                BackColor   = Color.Transparent,
                Padding     = new Padding(0)
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 16));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));

            tabla.Controls.Add(lblNombre, 0, 0); tabla.SetColumnSpan(lblNombre, 2);
            tabla.Controls.Add(txtNombre, 0, 1); tabla.SetColumnSpan(txtNombre, 2);
            tabla.Controls.Add(lblTablas, 0, 2);
            tabla.Controls.Add(lblSala,   1, 2);
            tabla.Controls.Add(nudTablas, 0, 3);
            tabla.Controls.Add(txtSala,   1, 3);
            tabla.Controls.Add(btnCrear,  0, 5);
            tabla.Controls.Add(btnUnirse, 1, 5);

            nudTablas.Dock = DockStyle.Fill;
            nudTablas.Margin = new Padding(0, 0, 8, 0);
            txtSala.Dock   = DockStyle.Fill;
            txtSala.Margin = new Padding(8, 0, 0, 0);

            panelTarjeta.Controls.Add(tabla);

            lblVersion.Text      = "v1.0 — Proyecto Final POO";
            lblVersion.Font      = ResolverFuente(8);
            lblVersion.ForeColor = Color.FromArgb(70, 70, 75);
            lblVersion.AutoSize  = false;
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            lblVersion.Dock      = DockStyle.Bottom;
            lblVersion.Height    = 24;

            this.Controls.Add(panelTarjeta);
            this.Controls.Add(lblVersion);
            this.Controls.Add(lblSubtitulo);
            this.Controls.Add(lblTitulo);

            this.ResumeLayout(false);
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

        private static void AplicarEstiloLabel(Label l, string texto)
        {
            l.Text      = texto;
            l.Font      = ResolverFuente(9);
            l.ForeColor = Color.FromArgb(160, 160, 165);
            l.Dock      = DockStyle.Fill;
            l.TextAlign = ContentAlignment.BottomLeft;
        }

        private static void AplicarEstiloInput(TextBox t)
        {
            t.BackColor  = Color.FromArgb(44, 44, 50);
            t.ForeColor  = Color.White;
            t.Font       = ResolverFuente(10);
            t.BorderStyle = BorderStyle.FixedSingle;
            t.Dock       = DockStyle.Fill;
        }

        private static void AplicarEstiloBoton(Button b, string texto, Color acento)
        {
            b.Text      = texto;
            b.Font      = ResolverFuente(10, FontStyle.Bold);
            b.BackColor = acento;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize  = 0;
            b.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(
                    Math.Min(acento.R + 30, 255),
                    Math.Min(acento.G + 30, 255),
                    Math.Min(acento.B + 30, 255));
            b.Cursor    = Cursors.Hand;
            b.Dock      = DockStyle.Fill;
            b.Margin    = new Padding(3, 4, 3, 0);
        }

        private void CentrarTarjeta()
        {
            panelTarjeta.Left = (this.Width  - panelTarjeta.Width)  / 2;
            panelTarjeta.Top  = (this.Height - panelTarjeta.Height) / 2 + 20;
        }
    }
}
