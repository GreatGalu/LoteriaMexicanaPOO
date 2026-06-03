using System.Drawing;
using System.Windows.Forms;

namespace LoteriaMexicana.UI.UserControls
{
    partial class UcPantallaJuego
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        internal Panel            panelIzquierdo;
        internal Label            lblEstado;
        internal TextBox          txtSala;
        internal Button           btnAccionRed;
        internal Button           btnUnirse;
        internal Button           btnSalir;
        internal PictureBox       picCartaActual;
        internal Button           btnGritarLoteria;
        internal TextBox          txtHistorialChat;
        internal TextBox          txtChatInput;
        internal Button           btnEnviar;
        internal FlowLayoutPanel  panelTablas;
        internal FlowLayoutPanel  panelHistorialCartas;
        private  Label            lblHistorialTitulo;

        private void InitializeComponent()
        {
            components           = new System.ComponentModel.Container();
            panelIzquierdo       = new Panel();
            lblEstado            = new Label();
            txtSala              = new TextBox();
            btnAccionRed         = new Button();
            btnUnirse            = new Button();
            btnSalir             = new Button();
            picCartaActual       = new PictureBox();
            btnGritarLoteria     = new Button();
            txtHistorialChat     = new TextBox();
            txtChatInput         = new TextBox();
            btnEnviar            = new Button();
            panelTablas          = new FlowLayoutPanel();
            panelHistorialCartas = new FlowLayoutPanel();
            lblHistorialTitulo   = new Label();

            this.SuspendLayout();

            this.Dock      = DockStyle.Fill;
            this.BackColor = Color.FromArgb(24, 24, 26);

            panelIzquierdo.Dock      = DockStyle.Left;
            panelIzquierdo.Width     = 295;
            panelIzquierdo.BackColor = Color.FromArgb(32, 32, 36);
            panelIzquierdo.Padding   = new Padding(10);

            lblEstado.Text      = "Sin conexion";
            lblEstado.Font      = ResolverFuente(9, FontStyle.Bold);
            lblEstado.ForeColor = Color.FromArgb(200, 200, 205);
            lblEstado.Dock      = DockStyle.Top;
            lblEstado.Height    = 22;

            var panelConexion = new FlowLayoutPanel
            {
                Dock          = DockStyle.Top,
                Height        = 34,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents  = false,
                BackColor     = Color.Transparent,
                Padding       = new Padding(0, 2, 0, 2)
            };

            AplicarEstiloInput(txtSala, "Sala");
            txtSala.Width  = 76;
            txtSala.Margin = new Padding(0, 3, 3, 0);

            AplicarEstiloBotonPequeno(btnAccionRed, "Crear",   Color.FromArgb(55, 100, 55));
            AplicarEstiloBotonPequeno(btnUnirse,    "Unirse",  Color.FromArgb(45, 80, 130));
            AplicarEstiloBotonPequeno(btnSalir,     "Salir",   Color.FromArgb(100, 40, 40));

            btnAccionRed.Click += btnAccionRed_ClickCrear;
            btnUnirse.Click    += (s, e) => { };
            btnSalir.Click     += btnSalir_Click;

            panelConexion.Controls.AddRange(new Control[]
                { txtSala, btnAccionRed, btnUnirse, btnSalir });

            picCartaActual.Dock        = DockStyle.Top;
            picCartaActual.Height      = 226;
            picCartaActual.SizeMode    = PictureBoxSizeMode.Zoom;
            picCartaActual.BackColor   = Color.FromArgb(24, 24, 26);
            picCartaActual.BorderStyle = BorderStyle.None;

            btnGritarLoteria.Text   = "¡ L O T E R Í A !";
            btnGritarLoteria.Dock   = DockStyle.Top;
            btnGritarLoteria.Height = 52;
            btnGritarLoteria.Font   = ResolverFuente(14, FontStyle.Bold);
            btnGritarLoteria.BackColor = Color.FromArgb(160, 30, 30);
            btnGritarLoteria.ForeColor = Color.White;
            btnGritarLoteria.FlatStyle = FlatStyle.Flat;
            btnGritarLoteria.FlatAppearance.BorderSize = 0;
            btnGritarLoteria.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 50, 50);
            btnGritarLoteria.Cursor    = Cursors.Hand;
            btnGritarLoteria.Margin    = new Padding(0, 6, 0, 6);
            btnGritarLoteria.Click    += btnGritarLoteria_Click;

            txtHistorialChat.Multiline  = true;
            txtHistorialChat.ReadOnly   = true;
            txtHistorialChat.ScrollBars = ScrollBars.Vertical;
            txtHistorialChat.Dock       = DockStyle.Fill;
            txtHistorialChat.BackColor  = Color.FromArgb(28, 28, 32);
            txtHistorialChat.ForeColor  = Color.FromArgb(190, 190, 195);
            txtHistorialChat.Font       = ResolverFuente(9);
            txtHistorialChat.BorderStyle = BorderStyle.None;

            var panelChat = new FlowLayoutPanel
            {
                Dock          = DockStyle.Bottom,
                Height        = 36,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents  = false,
                BackColor     = Color.Transparent
            };

            AplicarEstiloInput(txtChatInput, "Mensaje...");
            txtChatInput.Width  = 178;
            txtChatInput.Height = 28;
            txtChatInput.Margin = new Padding(0, 4, 4, 0);

            AplicarEstiloBotonPequeno(btnEnviar, "▶", Color.FromArgb(50, 90, 140));
            btnEnviar.Width  = 42;
            btnEnviar.Height = 28;
            btnEnviar.Margin = new Padding(0, 4, 0, 0);
            btnEnviar.Click += btnEnviar_Click;

            panelChat.Controls.AddRange(new Control[] { txtChatInput, btnEnviar });

            panelIzquierdo.Controls.Add(txtHistorialChat);   // Fill
            panelIzquierdo.Controls.Add(panelChat);          // Bottom del panel
            panelIzquierdo.Controls.Add(btnGritarLoteria);   // Top (de abajo hacia arriba)
            panelIzquierdo.Controls.Add(picCartaActual);
            panelIzquierdo.Controls.Add(panelConexion);
            panelIzquierdo.Controls.Add(lblEstado);

            panelHistorialCartas.Dock          = DockStyle.Bottom;
            panelHistorialCartas.Height        = 38;
            panelHistorialCartas.FlowDirection = FlowDirection.LeftToRight;
            panelHistorialCartas.WrapContents  = false;
            panelHistorialCartas.AutoScroll    = true;
            panelHistorialCartas.BackColor     = Color.FromArgb(20, 20, 22);
            panelHistorialCartas.Padding       = new Padding(8, 26, 8, 4);

            lblHistorialTitulo.Text      = "Historial de cartas  (pasa el cursor)";
            lblHistorialTitulo.Font      = ResolverFuente(8, FontStyle.Bold);
            lblHistorialTitulo.ForeColor = Color.FromArgb(180, 140, 30);
            lblHistorialTitulo.AutoSize  = true;
            lblHistorialTitulo.Location  = new Point(8, 8);
            panelHistorialCartas.Controls.Add(lblHistorialTitulo);

            panelHistorialCartas.MouseEnter += (s, e) =>
            {
                panelHistorialCartas.Height = 106;
                lblHistorialTitulo.Text     = "Historial de cartas cantadas";
            };
            panelHistorialCartas.MouseLeave += (s, e) =>
            {
                if (!panelHistorialCartas.DisplayRectangle.Contains(
                        panelHistorialCartas.PointToClient(Cursor.Position)))
                {
                    panelHistorialCartas.Height = 38;
                    lblHistorialTitulo.Text     = "Historial de cartas  (pasa el cursor)";
                }
            };

            panelTablas.Dock          = DockStyle.Fill;
            panelTablas.FlowDirection = FlowDirection.LeftToRight;
            panelTablas.WrapContents  = true;
            panelTablas.AutoScroll    = true;
            panelTablas.BackColor     = Color.FromArgb(28, 28, 32);
            panelTablas.Padding       = new Padding(20, 16, 20, 16);

            this.Controls.Add(panelTablas);             // Fill
            this.Controls.Add(panelHistorialCartas);    // Bottom
            this.Controls.Add(panelIzquierdo);          // Left

            this.ResumeLayout(false);
        }

        // ── Helpers de estilo ─────────────────────────────────────────────

        private static Font ResolverFuente(float size, FontStyle style = FontStyle.Regular)
        {
            foreach (string nombre in new[] { "Helvetica", "Arial", "Segoe UI" })
            {
                try
                {
                    using var p = new Font(nombre, 1);
                    if (p.Name == nombre) return new Font(nombre, size, style);
                }
                catch { }
            }
            return new Font("Segoe UI", size, style);
        }

        private static void AplicarEstiloInput(TextBox t, string placeholder)
        {
            t.BackColor       = Color.FromArgb(40, 40, 46);
            t.ForeColor       = Color.White;
            t.Font            = ResolverFuente(9);
            t.BorderStyle     = BorderStyle.FixedSingle;
            t.PlaceholderText = placeholder;
        }

        private static void AplicarEstiloBotonPequeno(Button b, string texto, Color acento)
        {
            b.Text      = texto;
            b.Font      = ResolverFuente(8, FontStyle.Bold);
            b.BackColor = acento;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Cursor    = Cursors.Hand;
            b.Width     = 58;
            b.Height    = 26;
            b.Margin    = new Padding(0, 3, 3, 0);
        }
    }
}
