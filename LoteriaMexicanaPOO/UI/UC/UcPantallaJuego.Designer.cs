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

        // ── Controles ────────────────────────────────────────────────────────
        internal Panel          panelIzquierdo;
        internal Label          lblEstado;
        internal TextBox        txtSala;
        internal Button         btnAccionRed;
        internal Button         btnSalir;
        internal PictureBox     picCartaActual;
        internal Button         btnGritarLoteria;
        internal TextBox        txtHistorialChat;
        internal TextBox        txtChatInput;
        internal Button         btnEnviar;
        internal FlowLayoutPanel panelTablas;
        internal FlowLayoutPanel panelHistorialCartas;
        internal Label          lblHistorialTitulo;
        internal CheckBox       chkAutoCantar;
        private  Panel          panelConexion;
        private  Panel          panelChat;

        // ── Reglas de victoria ───────────────────────────────────────────────
        internal GroupBox  grpReglas;
        internal CheckBox  chkHorizontal;
        internal CheckBox  chkVertical;
        internal CheckBox  chkDiagonal;
        internal CheckBox  chkEsquinas;
        internal CheckBox  chkPoyaCruz;

        // ── Lista de jugadores ───────────────────────────────────────────────
        internal GroupBox  grpJugadores;
        internal ListBox   listJugadores;

        // ── Historial de score ───────────────────────────────────────────────
        internal GroupBox  grpScorecard;
        internal ListBox   listScorecard;

        // ── Controles del gritón (solo anfitrión) ─────────────────────────────
        internal Panel  panelAdminControles;
        internal Button btnPausarTimer;
        internal Button btnAumentarVelocidad;
        internal Label  lblVelocidadActual;

        private void InitializeComponent()
        {
            panelIzquierdo      = new Panel();
            txtHistorialChat    = new TextBox();
            panelChat           = new Panel();
            txtChatInput        = new TextBox();
            btnEnviar           = new Button();
            chkAutoCantar       = new CheckBox();
            btnGritarLoteria    = new Button();
            picCartaActual      = new PictureBox();
            panelConexion       = new Panel();
            txtSala             = new TextBox();
            btnAccionRed        = new Button();
            btnSalir            = new Button();
            lblEstado           = new Label();

            // Reglas
            grpReglas           = new GroupBox();
            chkHorizontal       = new CheckBox();
            chkVertical         = new CheckBox();
            chkDiagonal         = new CheckBox();
            chkEsquinas         = new CheckBox();
            chkPoyaCruz         = new CheckBox();

            // Jugadores
            grpJugadores        = new GroupBox();
            listJugadores       = new ListBox();

            // Score
            grpScorecard        = new GroupBox();
            listScorecard       = new ListBox();

            // Admin controls
            panelAdminControles = new Panel();
            btnPausarTimer      = new Button();
            btnAumentarVelocidad = new Button();
            lblVelocidadActual  = new Label();

            panelTablas         = new FlowLayoutPanel();
            panelHistorialCartas = new FlowLayoutPanel();
            lblHistorialTitulo  = new Label();

            panelIzquierdo.SuspendLayout();
            panelChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCartaActual).BeginInit();
            panelConexion.SuspendLayout();
            panelHistorialCartas.SuspendLayout();
            grpReglas.SuspendLayout();
            grpJugadores.SuspendLayout();
            grpScorecard.SuspendLayout();
            panelAdminControles.SuspendLayout();
            SuspendLayout();

            // ── panelIzquierdo ───────────────────────────────────────────────
            panelIzquierdo.BackColor = Color.FromArgb(32, 32, 36);
            panelIzquierdo.Controls.Add(txtHistorialChat);
            panelIzquierdo.Controls.Add(panelChat);
            panelIzquierdo.Controls.Add(grpScorecard);
            panelIzquierdo.Controls.Add(grpJugadores);
            panelIzquierdo.Controls.Add(grpReglas);
            panelIzquierdo.Controls.Add(panelAdminControles);
            panelIzquierdo.Controls.Add(chkAutoCantar);
            panelIzquierdo.Controls.Add(btnGritarLoteria);
            panelIzquierdo.Controls.Add(picCartaActual);
            panelIzquierdo.Controls.Add(panelConexion);
            panelIzquierdo.Controls.Add(lblEstado);
            panelIzquierdo.Dock    = DockStyle.Left;
            panelIzquierdo.Name    = "panelIzquierdo";
            panelIzquierdo.Padding = new Padding(10, 6, 10, 6);
            panelIzquierdo.Size    = new Size(310, 1080);
            panelIzquierdo.TabIndex = 2;

            // ── lblEstado ────────────────────────────────────────────────────
            lblEstado.BackColor   = Color.Transparent;
            lblEstado.Dock        = DockStyle.Top;
            lblEstado.Font        = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEstado.ForeColor   = Color.FromArgb(200, 200, 205);
            lblEstado.Name        = "lblEstado";
            lblEstado.Size        = new Size(290, 22);
            lblEstado.TabIndex    = 6;
            lblEstado.Text        = "Sin conexion";
            lblEstado.TextAlign   = ContentAlignment.MiddleLeft;

            // ── panelConexion ────────────────────────────────────────────────
            panelConexion.BackColor = Color.Transparent;
            panelConexion.Controls.Add(txtSala);
            panelConexion.Controls.Add(btnAccionRed);
            panelConexion.Controls.Add(btnSalir);
            panelConexion.Dock    = DockStyle.Top;
            panelConexion.Name    = "panelConexion";
            panelConexion.Size    = new Size(290, 34);

            txtSala.BackColor   = Color.FromArgb(40, 40, 46);
            txtSala.BorderStyle = BorderStyle.FixedSingle;
            txtSala.Font        = new Font("Segoe UI", 9F);
            txtSala.ForeColor   = Color.White;
            txtSala.Location    = new Point(0, 4);
            txtSala.Name        = "txtSala";
            txtSala.Size        = new Size(76, 27);

            btnAccionRed.BackColor               = Color.FromArgb(55, 100, 55);
            btnAccionRed.Cursor                  = Cursors.Hand;
            btnAccionRed.FlatAppearance.BorderSize = 0;
            btnAccionRed.FlatStyle               = FlatStyle.Flat;
            btnAccionRed.Font                    = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnAccionRed.ForeColor               = Color.White;
            btnAccionRed.Location                = new Point(80, 4);
            btnAccionRed.Name                    = "btnAccionRed";
            btnAccionRed.Size                    = new Size(56, 24);
            btnAccionRed.Text                    = "Crear";
            btnAccionRed.UseVisualStyleBackColor = false;
            btnAccionRed.Click                  += btnAccionRed_ClickCrear;

            btnSalir.BackColor               = Color.FromArgb(100, 40, 40);
            btnSalir.Cursor                  = Cursors.Hand;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.FlatStyle               = FlatStyle.Flat;
            btnSalir.Font                    = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnSalir.ForeColor               = Color.White;
            btnSalir.Location                = new Point(200, 4);
            btnSalir.Name                    = "btnSalir";
            btnSalir.Size                    = new Size(56, 24);
            btnSalir.Text                    = "Salir";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click                  += btnSalir_Click;

            // ── picCartaActual ───────────────────────────────────────────────
            picCartaActual.BackColor = Color.FromArgb(24, 24, 26);
            picCartaActual.Dock      = DockStyle.Top;
            picCartaActual.Name      = "picCartaActual";
            picCartaActual.Size      = new Size(290, 218);
            picCartaActual.SizeMode  = PictureBoxSizeMode.Zoom;
            picCartaActual.TabStop   = false;

            // ── btnGritarLoteria ─────────────────────────────────────────────
            btnGritarLoteria.BackColor               = Color.FromArgb(160, 30, 30);
            btnGritarLoteria.Cursor                  = Cursors.Hand;
            btnGritarLoteria.Dock                    = DockStyle.Top;
            btnGritarLoteria.FlatAppearance.BorderSize = 0;
            btnGritarLoteria.FlatStyle               = FlatStyle.Flat;
            btnGritarLoteria.Font                    = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnGritarLoteria.ForeColor               = Color.White;
            btnGritarLoteria.Name                    = "btnGritarLoteria";
            btnGritarLoteria.Size                    = new Size(290, 48);
            btnGritarLoteria.Text                    = "¡ L O T E R Í A !";
            btnGritarLoteria.UseVisualStyleBackColor = false;
            btnGritarLoteria.Click                  += btnGritarLoteria_Click;

            // ── chkAutoCantar ────────────────────────────────────────────────
            chkAutoCantar.AutoSize   = true;
            chkAutoCantar.BackColor  = Color.Transparent;
            chkAutoCantar.Cursor     = Cursors.Hand;
            chkAutoCantar.Dock       = DockStyle.Top;
            chkAutoCantar.Font       = new Font("Segoe UI", 8F);
            chkAutoCantar.ForeColor  = Color.FromArgb(160, 160, 165);
            chkAutoCantar.Name       = "chkAutoCantar";
            chkAutoCantar.Size       = new Size(290, 22);
            chkAutoCantar.Text       = "Auto (carta cada 5s)";
            chkAutoCantar.UseVisualStyleBackColor = false;
            chkAutoCantar.Visible    = false;

            // ── panelAdminControles (solo anfitrión) ─────────────────────────
            panelAdminControles.BackColor = Color.FromArgb(26, 26, 30);
            panelAdminControles.Controls.Add(lblVelocidadActual);
            panelAdminControles.Controls.Add(btnAumentarVelocidad);
            panelAdminControles.Controls.Add(btnPausarTimer);
            panelAdminControles.Dock    = DockStyle.Top;
            panelAdminControles.Name    = "panelAdminControles";
            panelAdminControles.Size    = new Size(290, 36);
            panelAdminControles.Visible = false;

            btnPausarTimer.BackColor               = Color.FromArgb(55, 85, 130);
            btnPausarTimer.Cursor                  = Cursors.Hand;
            btnPausarTimer.FlatAppearance.BorderSize = 0;
            btnPausarTimer.FlatStyle               = FlatStyle.Flat;
            btnPausarTimer.Font                    = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnPausarTimer.ForeColor               = Color.White;
            btnPausarTimer.Location                = new Point(0, 4);
            btnPausarTimer.Name                    = "btnPausarTimer";
            btnPausarTimer.Size                    = new Size(96, 28);
            btnPausarTimer.Text                    = "⏸ Pausar";
            btnPausarTimer.UseVisualStyleBackColor = false;

            btnAumentarVelocidad.BackColor               = Color.FromArgb(100, 70, 130);
            btnAumentarVelocidad.Cursor                  = Cursors.Hand;
            btnAumentarVelocidad.FlatAppearance.BorderSize = 0;
            btnAumentarVelocidad.FlatStyle               = FlatStyle.Flat;
            btnAumentarVelocidad.Font                    = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnAumentarVelocidad.ForeColor               = Color.White;
            btnAumentarVelocidad.Location                = new Point(100, 4);
            btnAumentarVelocidad.Name                    = "btnAumentarVelocidad";
            btnAumentarVelocidad.Size                    = new Size(108, 28);
            btnAumentarVelocidad.Text                    = "⚡ Más rápido";
            btnAumentarVelocidad.UseVisualStyleBackColor = false;

            lblVelocidadActual.Font      = new Font("Segoe UI", 7.5F);
            lblVelocidadActual.ForeColor = Color.FromArgb(150, 150, 155);
            lblVelocidadActual.Location  = new Point(212, 10);
            lblVelocidadActual.Name      = "lblVelocidadActual";
            lblVelocidadActual.Size      = new Size(74, 18);
            lblVelocidadActual.Text      = "Vel: 5s";

            // ── grpReglas ────────────────────────────────────────────────────
            grpReglas.BackColor = Color.FromArgb(30, 30, 34);
            grpReglas.Dock      = DockStyle.Top;
            grpReglas.Font      = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            grpReglas.ForeColor = Color.FromArgb(180, 140, 30);
            grpReglas.Name      = "grpReglas";
            grpReglas.Padding   = new Padding(8, 4, 8, 4);
            grpReglas.Size      = new Size(290, 118);
            grpReglas.Text      = " Formas de ganar ";
            grpReglas.Controls.Add(chkPoyaCruz);
            grpReglas.Controls.Add(chkEsquinas);
            grpReglas.Controls.Add(chkDiagonal);
            grpReglas.Controls.Add(chkVertical);
            grpReglas.Controls.Add(chkHorizontal);

            chkHorizontal.AutoSize  = true;
            chkHorizontal.Checked   = true;
            chkHorizontal.Font      = new Font("Segoe UI", 8F);
            chkHorizontal.ForeColor = Color.FromArgb(200, 200, 205);
            chkHorizontal.Location  = new Point(10, 20);
            chkHorizontal.Name      = "chkHorizontal";
            chkHorizontal.Text      = "Línea Horizontal";
            chkHorizontal.UseVisualStyleBackColor = true;

            chkVertical.AutoSize  = true;
            chkVertical.Checked   = true;
            chkVertical.Font      = new Font("Segoe UI", 8F);
            chkVertical.ForeColor = Color.FromArgb(200, 200, 205);
            chkVertical.Location  = new Point(10, 40);
            chkVertical.Name      = "chkVertical";
            chkVertical.Text      = "Línea Vertical";
            chkVertical.UseVisualStyleBackColor = true;

            chkDiagonal.AutoSize  = true;
            chkDiagonal.Checked   = true;
            chkDiagonal.Font      = new Font("Segoe UI", 8F);
            chkDiagonal.ForeColor = Color.FromArgb(200, 200, 205);
            chkDiagonal.Location  = new Point(10, 60);
            chkDiagonal.Name      = "chkDiagonal";
            chkDiagonal.Text      = "Diagonal";
            chkDiagonal.UseVisualStyleBackColor = true;

            chkEsquinas.AutoSize  = true;
            chkEsquinas.Checked   = true;
            chkEsquinas.Font      = new Font("Segoe UI", 8F);
            chkEsquinas.ForeColor = Color.FromArgb(200, 200, 205);
            chkEsquinas.Location  = new Point(150, 20);
            chkEsquinas.Name      = "chkEsquinas";
            chkEsquinas.Text      = "Esquinas";
            chkEsquinas.UseVisualStyleBackColor = true;

            chkPoyaCruz.AutoSize  = true;
            chkPoyaCruz.Checked   = true;
            chkPoyaCruz.Font      = new Font("Segoe UI", 8F);
            chkPoyaCruz.ForeColor = Color.FromArgb(200, 200, 205);
            chkPoyaCruz.Location  = new Point(150, 40);
            chkPoyaCruz.Name      = "chkPoyaCruz";
            chkPoyaCruz.Text      = "Poya / Cruz";
            chkPoyaCruz.UseVisualStyleBackColor = true;

            // ── grpJugadores ─────────────────────────────────────────────────
            grpJugadores.BackColor = Color.FromArgb(30, 30, 34);
            grpJugadores.Dock      = DockStyle.Top;
            grpJugadores.Font      = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            grpJugadores.ForeColor = Color.FromArgb(100, 180, 230);
            grpJugadores.Name      = "grpJugadores";
            grpJugadores.Padding   = new Padding(6, 4, 6, 4);
            grpJugadores.Size      = new Size(290, 102);
            grpJugadores.Text      = " Jugadores en sala ";
            grpJugadores.Controls.Add(listJugadores);

            listJugadores.BackColor    = Color.FromArgb(28, 28, 32);
            listJugadores.BorderStyle  = BorderStyle.None;
            listJugadores.Dock         = DockStyle.Fill;
            listJugadores.Font         = new Font("Segoe UI", 8.5F);
            listJugadores.ForeColor    = Color.FromArgb(210, 210, 215);
            listJugadores.Name         = "listJugadores";
            listJugadores.SelectionMode = SelectionMode.None;

            // ── grpScorecard ─────────────────────────────────────────────────
            grpScorecard.BackColor = Color.FromArgb(30, 30, 34);
            grpScorecard.Dock      = DockStyle.Top;
            grpScorecard.Font      = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            grpScorecard.ForeColor = Color.FromArgb(230, 180, 60);
            grpScorecard.Name      = "grpScorecard";
            grpScorecard.Padding   = new Padding(6, 4, 6, 4);
            grpScorecard.Size      = new Size(290, 102);
            grpScorecard.Text      = " Historial de ganadores ";
            grpScorecard.Controls.Add(listScorecard);

            listScorecard.BackColor    = Color.FromArgb(28, 28, 32);
            listScorecard.BorderStyle  = BorderStyle.None;
            listScorecard.Dock         = DockStyle.Fill;
            listScorecard.Font         = new Font("Segoe UI", 8.5F);
            listScorecard.ForeColor    = Color.FromArgb(210, 210, 215);
            listScorecard.Name         = "listScorecard";
            listScorecard.SelectionMode = SelectionMode.None;

            // ── txtHistorialChat ─────────────────────────────────────────────
            txtHistorialChat.BackColor   = Color.FromArgb(28, 28, 32);
            txtHistorialChat.BorderStyle = BorderStyle.None;
            txtHistorialChat.Dock        = DockStyle.Fill;
            txtHistorialChat.Font        = new Font("Consolas", 8.5F);
            txtHistorialChat.ForeColor   = Color.FromArgb(190, 190, 195);
            txtHistorialChat.Multiline   = true;
            txtHistorialChat.Name        = "txtHistorialChat";
            txtHistorialChat.ReadOnly    = true;
            txtHistorialChat.ScrollBars  = ScrollBars.Vertical;

            // ── panelChat ────────────────────────────────────────────────────
            panelChat.BackColor = Color.Transparent;
            panelChat.Controls.Add(txtChatInput);
            panelChat.Controls.Add(btnEnviar);
            panelChat.Dock = DockStyle.Bottom;
            panelChat.Name = "panelChat";
            panelChat.Size = new Size(290, 36);

            txtChatInput.BackColor   = Color.FromArgb(40, 40, 46);
            txtChatInput.BorderStyle = BorderStyle.FixedSingle;
            txtChatInput.Font        = new Font("Segoe UI", 9F);
            txtChatInput.ForeColor   = Color.White;
            txtChatInput.Location    = new Point(0, 4);
            txtChatInput.Name        = "txtChatInput";
            txtChatInput.Size        = new Size(196, 27);

            btnEnviar.BackColor               = Color.FromArgb(50, 90, 140);
            btnEnviar.Cursor                  = Cursors.Hand;
            btnEnviar.FlatAppearance.BorderSize = 0;
            btnEnviar.FlatStyle               = FlatStyle.Flat;
            btnEnviar.Font                    = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnEnviar.ForeColor               = Color.White;
            btnEnviar.Location                = new Point(200, 4);
            btnEnviar.Name                    = "btnEnviar";
            btnEnviar.Size                    = new Size(55, 26);
            btnEnviar.Text                    = "Enviar";
            btnEnviar.UseVisualStyleBackColor = false;
            btnEnviar.Click                  += btnEnviar_Click;

            // ── panelTablas ──────────────────────────────────────────────────
            panelTablas.AutoScroll = true;
            panelTablas.BackColor  = Color.FromArgb(28, 28, 32);
            panelTablas.Dock       = DockStyle.Fill;
            panelTablas.Name       = "panelTablas";
            panelTablas.Padding    = new Padding(20, 16, 20, 16);
            panelTablas.TabIndex   = 0;

            // ── panelHistorialCartas ─────────────────────────────────────────
            panelHistorialCartas.AutoScroll     = true;
            panelHistorialCartas.BackColor      = Color.FromArgb(20, 20, 22);
            panelHistorialCartas.Controls.Add(lblHistorialTitulo);
            panelHistorialCartas.Dock           = DockStyle.Bottom;
            panelHistorialCartas.Name           = "panelHistorialCartas";
            panelHistorialCartas.Padding        = new Padding(8, 26, 8, 4);
            panelHistorialCartas.Size           = new Size(1610, 38);
            panelHistorialCartas.TabIndex       = 1;
            panelHistorialCartas.WrapContents   = false;
            panelHistorialCartas.MouseEnter    += PanelHistorial_MouseEnter;
            panelHistorialCartas.MouseLeave    += PanelHistorial_MouseLeave;

            lblHistorialTitulo.AutoSize  = true;
            lblHistorialTitulo.BackColor = Color.Transparent;
            lblHistorialTitulo.Font      = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblHistorialTitulo.ForeColor = Color.FromArgb(180, 140, 30);
            lblHistorialTitulo.Location  = new Point(11, 26);
            lblHistorialTitulo.Name      = "lblHistorialTitulo";
            lblHistorialTitulo.Text      = "Historial de cartas  (pasa el cursor)";

            // ── UcPantallaJuego ──────────────────────────────────────────────
            BackColor = Color.FromArgb(24, 24, 26);
            Controls.Add(panelTablas);
            Controls.Add(panelHistorialCartas);
            Controls.Add(panelIzquierdo);
            Name = "UcPantallaJuego";
            Size = new Size(1920, 1080);

            panelIzquierdo.ResumeLayout(false);
            panelIzquierdo.PerformLayout();
            panelChat.ResumeLayout(false);
            panelChat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picCartaActual).EndInit();
            panelConexion.ResumeLayout(false);
            panelConexion.PerformLayout();
            panelHistorialCartas.ResumeLayout(false);
            panelHistorialCartas.PerformLayout();
            grpReglas.ResumeLayout(false);
            grpReglas.PerformLayout();
            grpJugadores.ResumeLayout(false);
            grpScorecard.ResumeLayout(false);
            panelAdminControles.ResumeLayout(false);
            ResumeLayout(false);
        }

        // ── Animación del historial de cartas ────────────────────────────────
        private void PanelHistorial_MouseEnter(object sender, System.EventArgs e)
        {
            panelHistorialCartas.Height = 106;
            lblHistorialTitulo.Text     = "Historial de cartas cantadas";
        }

        private void PanelHistorial_MouseLeave(object sender, System.EventArgs e)
        {
            if (!panelHistorialCartas.DisplayRectangle.Contains(
                    panelHistorialCartas.PointToClient(System.Windows.Forms.Cursor.Position)))
            {
                panelHistorialCartas.Height = 38;
                lblHistorialTitulo.Text     = "Historial de cartas  (pasa el cursor)";
            }
        }
    }
}