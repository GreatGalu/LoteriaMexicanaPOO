namespace LoteriaMexicana.UI
{
    partial class FormJuego
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        // ══════════════════════════════════════════════════════════
        //  CONTROLES DECLARADOS
        //  Nomenclatura clara: cada control dice qué es y dónde vive.
        // ══════════════════════════════════════════════════════════

        // -- Panel izquierdo (conexion + carta actual + chat) ------
        internal System.Windows.Forms.Panel        panelControl;
        internal System.Windows.Forms.Label        lblEstado;
        internal System.Windows.Forms.TextBox      txtIpServidor;
        internal System.Windows.Forms.Button       btnCrearPartida;
        internal System.Windows.Forms.Button       btnUnirse;
        internal System.Windows.Forms.PictureBox   picCartaActual;
        internal System.Windows.Forms.Button       btnGritarLoteria;
        internal System.Windows.Forms.TextBox      txtHistorialChat;
        internal System.Windows.Forms.TextBox      txtChatInput;
        internal System.Windows.Forms.Button       btnEnviar;

        // -- Panel central (tablas de juego) -----------------------
        internal System.Windows.Forms.FlowLayoutPanel panelTablas;

        // -- Panel inferior (historial de cartas cantadas) ---------
        internal System.Windows.Forms.FlowLayoutPanel panelHistorialCartas;
        private  System.Windows.Forms.Label           lblHistorialTitulo;

        private void InitializeComponent()
        {
            // ── Instanciacion ────────────────────────────────────────────
            components           = new System.ComponentModel.Container();
            panelControl         = new System.Windows.Forms.Panel();
            lblEstado            = new System.Windows.Forms.Label();
            txtIpServidor        = new System.Windows.Forms.TextBox();
            btnCrearPartida      = new System.Windows.Forms.Button();
            btnUnirse            = new System.Windows.Forms.Button();
            picCartaActual       = new System.Windows.Forms.PictureBox();
            btnGritarLoteria     = new System.Windows.Forms.Button();
            txtHistorialChat     = new System.Windows.Forms.TextBox();
            txtChatInput         = new System.Windows.Forms.TextBox();
            btnEnviar            = new System.Windows.Forms.Button();
            panelTablas          = new System.Windows.Forms.FlowLayoutPanel();
            panelHistorialCartas = new System.Windows.Forms.FlowLayoutPanel();
            lblHistorialTitulo   = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // ── panelControl (izquierdo, ancho fijo 310 px) ──────────────
            panelControl.Dock    = System.Windows.Forms.DockStyle.Left;
            panelControl.Width   = 310;
            panelControl.Padding = new System.Windows.Forms.Padding(12);

            // lblEstado — nombre de sala o estado de conexion
            lblEstado.AutoSize = false;
            lblEstado.Text     = "Sin conexion";
            lblEstado.Dock     = System.Windows.Forms.DockStyle.Top;
            lblEstado.Height   = 22;

            // Fila de conexion: IP + Crear + Unirse
            var panelConexion = new System.Windows.Forms.FlowLayoutPanel
            {
                Dock          = System.Windows.Forms.DockStyle.Top,
                Height        = 34,
                FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight,
                WrapContents  = false,
                Padding       = new System.Windows.Forms.Padding(0, 2, 0, 2)
            };

            txtIpServidor.Width            = 90;
            txtIpServidor.PlaceholderText  = "Codigo sala";
            txtIpServidor.Margin           = new System.Windows.Forms.Padding(0, 3, 4, 0);

            btnCrearPartida.Text   = "Crear";
            btnCrearPartida.Width  = 70;
            btnCrearPartida.Height = 26;
            btnCrearPartida.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            btnCrearPartida.Click += new System.EventHandler(this.btnCrearPartida_Click);

            btnUnirse.Text   = "Unirse";
            btnUnirse.Width  = 70;
            btnUnirse.Height = 26;
            btnUnirse.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            btnUnirse.Click += new System.EventHandler(this.btnUnirse_Click);

            panelConexion.Controls.AddRange(new System.Windows.Forms.Control[]
                { txtIpServidor, btnCrearPartida, btnUnirse });

            // picCartaActual — imagen de la carta actualmente cantada
            picCartaActual.Dock        = System.Windows.Forms.DockStyle.Top;
            picCartaActual.Height      = 240;
            picCartaActual.SizeMode    = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            picCartaActual.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picCartaActual.Margin      = new System.Windows.Forms.Padding(0, 6, 0, 6);

            // btnGritarLoteria — boton grande de victoria
            btnGritarLoteria.Text   = "!LOTERIA!";
            btnGritarLoteria.Dock   = System.Windows.Forms.DockStyle.Top;
            btnGritarLoteria.Height = 48;
            btnGritarLoteria.Click += new System.EventHandler(this.btnGritarLoteria_Click);

            // txtHistorialChat — log de mensajes
            txtHistorialChat.Multiline    = true;
            txtHistorialChat.ReadOnly     = true;
            txtHistorialChat.ScrollBars   = System.Windows.Forms.ScrollBars.Vertical;
            txtHistorialChat.Dock         = System.Windows.Forms.DockStyle.Fill;

            // Fila inferior de chat: input + enviar
            var panelChat = new System.Windows.Forms.FlowLayoutPanel
            {
                Dock          = System.Windows.Forms.DockStyle.Bottom,
                Height        = 34,
                FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight,
                WrapContents  = false
            };

            txtChatInput.Width  = 188;
            txtChatInput.Height = 26;
            txtChatInput.Margin = new System.Windows.Forms.Padding(0, 4, 4, 0);

            btnEnviar.Text   = "Enviar";
            btnEnviar.Width  = 80;
            btnEnviar.Height = 26;
            btnEnviar.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);

            panelChat.Controls.AddRange(new System.Windows.Forms.Control[]
                { txtChatInput, btnEnviar });

            // Agregar controles al panelControl en orden (Bottom -> Fill -> Top)
            panelControl.Controls.Add(txtHistorialChat);    // Fill
            panelControl.Controls.Add(panelChat);           // Bottom (antes del Fill)
            panelControl.Controls.Add(btnGritarLoteria);    // Top (se apila desde arriba)
            panelControl.Controls.Add(picCartaActual);
            panelControl.Controls.Add(panelConexion);
            panelControl.Controls.Add(lblEstado);

            // ── panelHistorialCartas (inferior, colapsable) ──────────────
            panelHistorialCartas.Dock          = System.Windows.Forms.DockStyle.Bottom;
            panelHistorialCartas.Height        = 40;
            panelHistorialCartas.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            panelHistorialCartas.WrapContents  = false;
            panelHistorialCartas.AutoScroll    = true;
            panelHistorialCartas.Padding       = new System.Windows.Forms.Padding(8, 30, 8, 4);

            lblHistorialTitulo.Text      = "Historial de cartas cantadas  (hover para expandir)";
            lblHistorialTitulo.ForeColor = System.Drawing.Color.Gold;
            lblHistorialTitulo.Font      = new System.Drawing.Font("Segoe UI", 8, System.Drawing.FontStyle.Bold);
            lblHistorialTitulo.AutoSize  = true;
            lblHistorialTitulo.Location  = new System.Drawing.Point(8, 10);

            panelHistorialCartas.Controls.Add(lblHistorialTitulo);

            // Hover: expandir / colapsar
            panelHistorialCartas.MouseEnter += (s, e) =>
            {
                panelHistorialCartas.Height = 110;
                lblHistorialTitulo.Text     = "Historial de cartas cantadas";
            };
            panelHistorialCartas.MouseLeave += (s, e) =>
            {
                if (!panelHistorialCartas.DisplayRectangle.Contains(
                        panelHistorialCartas.PointToClient(System.Windows.Forms.Cursor.Position)))
                {
                    panelHistorialCartas.Height = 40;
                    lblHistorialTitulo.Text     = "Historial de cartas cantadas  (hover para expandir)";
                }
            };

            // ── panelTablas (central, ocupa el resto del espacio) ────────
            panelTablas.Dock          = System.Windows.Forms.DockStyle.Fill;
            panelTablas.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            panelTablas.WrapContents  = true;
            panelTablas.AutoScroll    = true;
            panelTablas.Padding       = new System.Windows.Forms.Padding(20, 16, 20, 16);

            // ── Formulario ───────────────────────────────────────────────
            this.Text          = "Loteria Mexicana Online";
            this.MinimumSize   = new System.Drawing.Size(900, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // Orden de agregado: Fill se agrega antes de los Dock fijos
            this.Controls.Add(panelTablas);             // Fill (primero)
            this.Controls.Add(panelHistorialCartas);    // Bottom
            this.Controls.Add(panelControl);            // Left

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
