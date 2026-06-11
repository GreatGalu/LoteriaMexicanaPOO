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
        internal CheckBox  chkTablaLlena;
        internal CheckBox  chkPoyaCruz;
        internal CheckBox  chkCartasDobles;

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
            panelIzquierdo = new Panel();
            lblVelocidadActual = new Label();
            txtHistorialChat = new TextBox();
            panelChat = new Panel();
            txtChatInput = new TextBox();
            btnEnviar = new Button();
            grpScorecard = new GroupBox();
            listScorecard = new ListBox();
            grpJugadores = new GroupBox();
            listJugadores = new ListBox();
            grpReglas = new GroupBox();
            btnNuevaFigura = new Button();
            btnCargarFiguras = new Button();
            clbFiguras = new CheckedListBox();
            chkCartasDobles = new CheckBox();
            chkTablaLlena = new CheckBox();
            panelAdminControles = new Panel();
            btnDisminuirVelocidad = new Button();
            btnAumentarVelocidad = new Button();
            btnPausarTimer = new Button();
            chkAutoCantar = new CheckBox();
            btnGritarLoteria = new Button();
            picCartaActual = new PictureBox();
            panelConexion = new Panel();
            txtSala = new TextBox();
            btnAccionRed = new Button();
            btnSalir = new Button();
            lblEstado = new Label();
            panelTablas = new FlowLayoutPanel();
            panelHistorialCartas = new FlowLayoutPanel();
            lblHistorialTitulo = new Label();
            panelIzquierdo.SuspendLayout();
            panelChat.SuspendLayout();
            grpScorecard.SuspendLayout();
            grpJugadores.SuspendLayout();
            grpReglas.SuspendLayout();
            panelAdminControles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCartaActual).BeginInit();
            panelConexion.SuspendLayout();
            panelHistorialCartas.SuspendLayout();
            SuspendLayout();
            // 
            // panelIzquierdo
            // 
            panelIzquierdo.BackColor = Color.FromArgb(32, 32, 36);
            panelIzquierdo.Controls.Add(lblVelocidadActual);
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
            panelIzquierdo.Dock = DockStyle.Left;
            panelIzquierdo.Location = new Point(0, 0);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Padding = new Padding(10, 6, 10, 6);
            panelIzquierdo.Size = new Size(310, 1080);
            panelIzquierdo.TabIndex = 2;
            // 
            // lblVelocidadActual
            // 
            lblVelocidadActual.Font = new Font("Segoe UI", 7.5F);
            lblVelocidadActual.ForeColor = Color.FromArgb(150, 150, 155);
            lblVelocidadActual.Location = new Point(94, 9);
            lblVelocidadActual.Name = "lblVelocidadActual";
            lblVelocidadActual.Size = new Size(74, 14);
            lblVelocidadActual.TabIndex = 0;
            lblVelocidadActual.Text = "Vel: 5s";
            // 
            // txtHistorialChat
            // 
            txtHistorialChat.BackColor = Color.FromArgb(28, 28, 32);
            txtHistorialChat.BorderStyle = BorderStyle.None;
            txtHistorialChat.Dock = DockStyle.Fill;
            txtHistorialChat.Font = new Font("Consolas", 8.5F);
            txtHistorialChat.ForeColor = Color.FromArgb(190, 190, 195);
            txtHistorialChat.Location = new Point(10, 703);
            txtHistorialChat.Multiline = true;
            txtHistorialChat.Name = "txtHistorialChat";
            txtHistorialChat.ReadOnly = true;
            txtHistorialChat.ScrollBars = ScrollBars.Vertical;
            txtHistorialChat.Size = new Size(290, 335);
            txtHistorialChat.TabIndex = 0;
            // 
            // panelChat
            // 
            panelChat.BackColor = Color.Transparent;
            panelChat.Controls.Add(txtChatInput);
            panelChat.Controls.Add(btnEnviar);
            panelChat.Dock = DockStyle.Bottom;
            panelChat.Location = new Point(10, 1038);
            panelChat.Name = "panelChat";
            panelChat.Size = new Size(290, 36);
            panelChat.TabIndex = 1;
            // 
            // txtChatInput
            // 
            txtChatInput.BackColor = Color.FromArgb(40, 40, 46);
            txtChatInput.BorderStyle = BorderStyle.FixedSingle;
            txtChatInput.Font = new Font("Segoe UI", 9F);
            txtChatInput.ForeColor = Color.White;
            txtChatInput.Location = new Point(0, 4);
            txtChatInput.Name = "txtChatInput";
            txtChatInput.Size = new Size(196, 23);
            txtChatInput.TabIndex = 0;
            // 
            // btnEnviar
            // 
            btnEnviar.BackColor = Color.FromArgb(50, 90, 140);
            btnEnviar.Cursor = Cursors.Hand;
            btnEnviar.FlatAppearance.BorderSize = 0;
            btnEnviar.FlatStyle = FlatStyle.Flat;
            btnEnviar.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnEnviar.ForeColor = Color.White;
            btnEnviar.Location = new Point(200, 4);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(55, 26);
            btnEnviar.TabIndex = 1;
            btnEnviar.Text = "Enviar";
            btnEnviar.UseVisualStyleBackColor = false;
            btnEnviar.Click += btnEnviar_Click;
            // 
            // grpScorecard
            // 
            grpScorecard.BackColor = Color.FromArgb(30, 30, 34);
            grpScorecard.Controls.Add(listScorecard);
            grpScorecard.Dock = DockStyle.Top;
            grpScorecard.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            grpScorecard.ForeColor = Color.FromArgb(230, 180, 60);
            grpScorecard.Location = new Point(10, 601);
            grpScorecard.Name = "grpScorecard";
            grpScorecard.Padding = new Padding(6, 4, 6, 4);
            grpScorecard.Size = new Size(290, 102);
            grpScorecard.TabIndex = 2;
            grpScorecard.TabStop = false;
            grpScorecard.Text = " Historial de ganadores ";
            // 
            // listScorecard
            // 
            listScorecard.BackColor = Color.FromArgb(28, 28, 32);
            listScorecard.BorderStyle = BorderStyle.None;
            listScorecard.Dock = DockStyle.Fill;
            listScorecard.Font = new Font("Segoe UI", 8.5F);
            listScorecard.ForeColor = Color.FromArgb(210, 210, 215);
            listScorecard.ItemHeight = 13;
            listScorecard.Location = new Point(6, 20);
            listScorecard.Name = "listScorecard";
            listScorecard.SelectionMode = SelectionMode.None;
            listScorecard.Size = new Size(278, 78);
            listScorecard.TabIndex = 0;
            // 
            // grpJugadores
            // 
            grpJugadores.BackColor = Color.FromArgb(30, 30, 34);
            grpJugadores.Controls.Add(listJugadores);
            grpJugadores.Dock = DockStyle.Top;
            grpJugadores.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            grpJugadores.ForeColor = Color.FromArgb(100, 180, 230);
            grpJugadores.Location = new Point(10, 499);
            grpJugadores.Name = "grpJugadores";
            grpJugadores.Padding = new Padding(6, 4, 6, 4);
            grpJugadores.Size = new Size(290, 102);
            grpJugadores.TabIndex = 3;
            grpJugadores.TabStop = false;
            grpJugadores.Text = " Jugadores en sala ";
            // 
            // listJugadores
            // 
            listJugadores.BackColor = Color.FromArgb(28, 28, 32);
            listJugadores.BorderStyle = BorderStyle.None;
            listJugadores.Dock = DockStyle.Fill;
            listJugadores.Font = new Font("Segoe UI", 8.5F);
            listJugadores.ForeColor = Color.FromArgb(210, 210, 215);
            listJugadores.ItemHeight = 13;
            listJugadores.Location = new Point(6, 20);
            listJugadores.Name = "listJugadores";
            listJugadores.SelectionMode = SelectionMode.None;
            listJugadores.Size = new Size(278, 78);
            listJugadores.TabIndex = 0;
            // 
            // grpReglas
            // 
            grpReglas.BackColor = Color.FromArgb(30, 30, 34);
            grpReglas.Controls.Add(btnNuevaFigura);
            grpReglas.Controls.Add(btnCargarFiguras);
            grpReglas.Controls.Add(clbFiguras);
            grpReglas.Controls.Add(chkCartasDobles);
            grpReglas.Controls.Add(chkTablaLlena);
            grpReglas.Dock = DockStyle.Top;
            grpReglas.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            grpReglas.ForeColor = Color.FromArgb(180, 140, 30);
            grpReglas.Location = new Point(10, 381);
            grpReglas.Name = "grpReglas";
            grpReglas.Padding = new Padding(8, 4, 8, 4);
            grpReglas.Size = new Size(290, 118);
            grpReglas.TabIndex = 4;
            grpReglas.TabStop = false;
            grpReglas.Text = " Formas de ganar ";
            // 
            // btnNuevaFigura
            // 
            btnNuevaFigura.BackColor = Color.FromArgb(160, 30, 80);
            btnNuevaFigura.Cursor = Cursors.Hand;
            btnNuevaFigura.FlatAppearance.BorderSize = 0;
            btnNuevaFigura.FlatStyle = FlatStyle.Flat;
            btnNuevaFigura.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnNuevaFigura.ForeColor = Color.White;
            btnNuevaFigura.Location = new Point(212, 66);
            btnNuevaFigura.Name = "btnNuevaFigura";
            btnNuevaFigura.Size = new Size(56, 42);
            btnNuevaFigura.TabIndex = 5;
            btnNuevaFigura.Text = "Crear";
            btnNuevaFigura.UseVisualStyleBackColor = false;
            btnNuevaFigura.Click += BtnNuevaFigura_Click;
            // 
            // btnCargarFiguras
            // 
            btnCargarFiguras.BackColor = Color.FromArgb(160, 80, 30);
            btnCargarFiguras.Cursor = Cursors.Hand;
            btnCargarFiguras.FlatAppearance.BorderSize = 0;
            btnCargarFiguras.FlatStyle = FlatStyle.Flat;
            btnCargarFiguras.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnCargarFiguras.ForeColor = Color.White;
            btnCargarFiguras.Location = new Point(150, 66);
            btnCargarFiguras.Name = "btnCargarFiguras";
            btnCargarFiguras.Size = new Size(56, 42);
            btnCargarFiguras.TabIndex = 4;
            btnCargarFiguras.Text = "Cargar";
            btnCargarFiguras.UseVisualStyleBackColor = false;
            // 
            // clbFiguras
            // 
            clbFiguras.BackColor = Color.FromArgb(32, 32, 36);
            clbFiguras.ForeColor = SystemColors.Info;
            clbFiguras.FormattingEnabled = true;
            clbFiguras.Location = new Point(11, 21);
            clbFiguras.Name = "clbFiguras";
            clbFiguras.Size = new Size(133, 76);
            clbFiguras.TabIndex = 3;
            // 
            // chkCartasDobles
            // 
            chkCartasDobles.AutoSize = true;
            chkCartasDobles.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            chkCartasDobles.ForeColor = Color.FromArgb(220, 100, 100);
            chkCartasDobles.Location = new Point(150, 43);
            chkCartasDobles.Name = "chkCartasDobles";
            chkCartasDobles.Size = new Size(110, 17);
            chkCartasDobles.TabIndex = 1;
            chkCartasDobles.Text = "⭐ Cartas Dobles";
            chkCartasDobles.UseVisualStyleBackColor = true;
            // 
            // chkTablaLlena
            // 
            chkTablaLlena.AutoSize = true;
            chkTablaLlena.Font = new Font("Segoe UI", 8F);
            chkTablaLlena.ForeColor = Color.FromArgb(200, 200, 205);
            chkTablaLlena.Location = new Point(150, 20);
            chkTablaLlena.Name = "chkTablaLlena";
            chkTablaLlena.Size = new Size(81, 17);
            chkTablaLlena.TabIndex = 2;
            chkTablaLlena.Text = "Tabla llena";
            chkTablaLlena.UseVisualStyleBackColor = true;
            // 
            // panelAdminControles
            // 
            panelAdminControles.BackColor = Color.FromArgb(26, 26, 30);
            panelAdminControles.Controls.Add(btnDisminuirVelocidad);
            panelAdminControles.Controls.Add(btnAumentarVelocidad);
            panelAdminControles.Controls.Add(btnPausarTimer);
            panelAdminControles.Dock = DockStyle.Top;
            panelAdminControles.Location = new Point(10, 345);
            panelAdminControles.Name = "panelAdminControles";
            panelAdminControles.Size = new Size(290, 36);
            panelAdminControles.TabIndex = 5;
            panelAdminControles.Visible = false;
            // 
            // btnDisminuirVelocidad
            // 
            btnDisminuirVelocidad.BackColor = Color.Teal;
            btnDisminuirVelocidad.Cursor = Cursors.Hand;
            btnDisminuirVelocidad.FlatAppearance.BorderSize = 0;
            btnDisminuirVelocidad.FlatStyle = FlatStyle.Flat;
            btnDisminuirVelocidad.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnDisminuirVelocidad.ForeColor = Color.White;
            btnDisminuirVelocidad.Location = new Point(196, 4);
            btnDisminuirVelocidad.Name = "btnDisminuirVelocidad";
            btnDisminuirVelocidad.Size = new Size(90, 28);
            btnDisminuirVelocidad.TabIndex = 3;
            btnDisminuirVelocidad.Text = "Más lento";
            btnDisminuirVelocidad.UseVisualStyleBackColor = false;
            btnDisminuirVelocidad.Click += btnDisminuirVelocidad_Click;
            // 
            // btnAumentarVelocidad
            // 
            btnAumentarVelocidad.BackColor = Color.FromArgb(100, 70, 130);
            btnAumentarVelocidad.Cursor = Cursors.Hand;
            btnAumentarVelocidad.FlatAppearance.BorderSize = 0;
            btnAumentarVelocidad.FlatStyle = FlatStyle.Flat;
            btnAumentarVelocidad.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnAumentarVelocidad.ForeColor = Color.White;
            btnAumentarVelocidad.Location = new Point(101, 4);
            btnAumentarVelocidad.Name = "btnAumentarVelocidad";
            btnAumentarVelocidad.Size = new Size(90, 28);
            btnAumentarVelocidad.TabIndex = 1;
            btnAumentarVelocidad.Text = "⚡ Más rápido";
            btnAumentarVelocidad.UseVisualStyleBackColor = false;
            // 
            // btnPausarTimer
            // 
            btnPausarTimer.BackColor = Color.FromArgb(55, 85, 130);
            btnPausarTimer.Cursor = Cursors.Hand;
            btnPausarTimer.FlatAppearance.BorderSize = 0;
            btnPausarTimer.FlatStyle = FlatStyle.Flat;
            btnPausarTimer.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnPausarTimer.ForeColor = Color.White;
            btnPausarTimer.Location = new Point(0, 4);
            btnPausarTimer.Name = "btnPausarTimer";
            btnPausarTimer.Size = new Size(96, 28);
            btnPausarTimer.TabIndex = 2;
            btnPausarTimer.Text = "⏸ Pausar";
            btnPausarTimer.UseVisualStyleBackColor = false;
            // 
            // chkAutoCantar
            // 
            chkAutoCantar.AutoSize = true;
            chkAutoCantar.BackColor = Color.Transparent;
            chkAutoCantar.Cursor = Cursors.Hand;
            chkAutoCantar.Dock = DockStyle.Top;
            chkAutoCantar.Font = new Font("Segoe UI", 8F);
            chkAutoCantar.ForeColor = Color.FromArgb(160, 160, 165);
            chkAutoCantar.Location = new Point(10, 328);
            chkAutoCantar.Name = "chkAutoCantar";
            chkAutoCantar.Size = new Size(290, 17);
            chkAutoCantar.TabIndex = 6;
            chkAutoCantar.Text = "Automático";
            chkAutoCantar.UseVisualStyleBackColor = false;
            chkAutoCantar.Visible = false;
            // 
            // btnGritarLoteria
            // 
            btnGritarLoteria.BackColor = Color.FromArgb(160, 30, 30);
            btnGritarLoteria.Cursor = Cursors.Hand;
            btnGritarLoteria.Dock = DockStyle.Top;
            btnGritarLoteria.FlatAppearance.BorderSize = 0;
            btnGritarLoteria.FlatStyle = FlatStyle.Flat;
            btnGritarLoteria.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnGritarLoteria.ForeColor = Color.White;
            btnGritarLoteria.Location = new Point(10, 280);
            btnGritarLoteria.Name = "btnGritarLoteria";
            btnGritarLoteria.Size = new Size(290, 48);
            btnGritarLoteria.TabIndex = 7;
            btnGritarLoteria.Text = "¡ L O T E R Í A !";
            btnGritarLoteria.UseVisualStyleBackColor = false;
            btnGritarLoteria.Click += btnGritarLoteria_Click;
            // 
            // picCartaActual
            // 
            picCartaActual.BackColor = Color.FromArgb(24, 24, 26);
            picCartaActual.Dock = DockStyle.Top;
            picCartaActual.Location = new Point(10, 62);
            picCartaActual.Name = "picCartaActual";
            picCartaActual.Size = new Size(290, 218);
            picCartaActual.SizeMode = PictureBoxSizeMode.Zoom;
            picCartaActual.TabIndex = 8;
            picCartaActual.TabStop = false;
            // 
            // panelConexion
            // 
            panelConexion.BackColor = Color.Transparent;
            panelConexion.Controls.Add(txtSala);
            panelConexion.Controls.Add(btnAccionRed);
            panelConexion.Controls.Add(btnSalir);
            panelConexion.Dock = DockStyle.Top;
            panelConexion.Location = new Point(10, 28);
            panelConexion.Name = "panelConexion";
            panelConexion.Size = new Size(290, 34);
            panelConexion.TabIndex = 9;
            // 
            // txtSala
            // 
            txtSala.BackColor = Color.FromArgb(40, 40, 46);
            txtSala.BorderStyle = BorderStyle.FixedSingle;
            txtSala.Font = new Font("Segoe UI", 9F);
            txtSala.ForeColor = Color.White;
            txtSala.Location = new Point(0, 4);
            txtSala.Name = "txtSala";
            txtSala.Size = new Size(76, 23);
            txtSala.TabIndex = 0;
            // 
            // btnAccionRed
            // 
            btnAccionRed.BackColor = Color.FromArgb(55, 100, 55);
            btnAccionRed.Cursor = Cursors.Hand;
            btnAccionRed.FlatAppearance.BorderSize = 0;
            btnAccionRed.FlatStyle = FlatStyle.Flat;
            btnAccionRed.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnAccionRed.ForeColor = Color.White;
            btnAccionRed.Location = new Point(80, 4);
            btnAccionRed.Name = "btnAccionRed";
            btnAccionRed.Size = new Size(93, 24);
            btnAccionRed.TabIndex = 1;
            btnAccionRed.Text = "Crear";
            btnAccionRed.UseVisualStyleBackColor = false;
            btnAccionRed.Click += btnAccionRed_ClickCrear;
            // 
            // btnSalir
            // 
            btnSalir.BackColor = Color.FromArgb(100, 40, 40);
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnSalir.ForeColor = Color.White;
            btnSalir.Location = new Point(200, 4);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(56, 24);
            btnSalir.TabIndex = 2;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += btnSalir_Click;
            // 
            // lblEstado
            // 
            lblEstado.BackColor = Color.Transparent;
            lblEstado.Dock = DockStyle.Top;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEstado.ForeColor = Color.FromArgb(200, 200, 205);
            lblEstado.Location = new Point(10, 6);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(290, 22);
            lblEstado.TabIndex = 6;
            lblEstado.Text = "Sin conexion";
            lblEstado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelTablas
            // 
            panelTablas.AutoScroll = true;
            panelTablas.BackColor = Color.FromArgb(28, 28, 32);
            panelTablas.Dock = DockStyle.Fill;
            panelTablas.Location = new Point(310, 0);
            panelTablas.Name = "panelTablas";
            panelTablas.Padding = new Padding(20, 16, 20, 16);
            panelTablas.Size = new Size(1610, 1042);
            panelTablas.TabIndex = 0;
            // 
            // panelHistorialCartas
            // 
            panelHistorialCartas.AutoScroll = true;
            panelHistorialCartas.BackColor = Color.FromArgb(20, 20, 22);
            panelHistorialCartas.Controls.Add(lblHistorialTitulo);
            panelHistorialCartas.Dock = DockStyle.Bottom;
            panelHistorialCartas.Location = new Point(310, 1042);
            panelHistorialCartas.Name = "panelHistorialCartas";
            panelHistorialCartas.Padding = new Padding(8, 26, 8, 4);
            panelHistorialCartas.Size = new Size(1610, 38);
            panelHistorialCartas.TabIndex = 1;
            panelHistorialCartas.WrapContents = false;
            panelHistorialCartas.MouseEnter += PanelHistorial_MouseEnter;
            panelHistorialCartas.MouseLeave += PanelHistorial_MouseLeave;
            // 
            // lblHistorialTitulo
            // 
            lblHistorialTitulo.AutoSize = true;
            lblHistorialTitulo.BackColor = Color.Transparent;
            lblHistorialTitulo.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblHistorialTitulo.ForeColor = Color.FromArgb(180, 140, 30);
            lblHistorialTitulo.Location = new Point(11, 26);
            lblHistorialTitulo.Name = "lblHistorialTitulo";
            lblHistorialTitulo.Size = new Size(184, 13);
            lblHistorialTitulo.TabIndex = 0;
            lblHistorialTitulo.Text = "Historial de cartas  (pasa el cursor)";
            // 
            // UcPantallaJuego
            // 
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
            grpScorecard.ResumeLayout(false);
            grpJugadores.ResumeLayout(false);
            grpReglas.ResumeLayout(false);
            grpReglas.PerformLayout();
            panelAdminControles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picCartaActual).EndInit();
            panelConexion.ResumeLayout(false);
            panelConexion.PerformLayout();
            panelHistorialCartas.ResumeLayout(false);
            panelHistorialCartas.PerformLayout();
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
        internal Button btnDisminuirVelocidad;
        private CheckedListBox clbFiguras;
        internal Button btnCargarFiguras;
        internal Button btnNuevaFigura;
    }
}