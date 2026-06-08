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
        internal Panel panelIzquierdo;
        internal Label lblEstado;
        internal TextBox txtSala;
        internal Button btnAccionRed;
        internal Button btnSalir;
        internal PictureBox picCartaActual;
        internal Button btnGritarLoteria;
        internal TextBox txtHistorialChat;
        internal TextBox txtChatInput;
        internal Button btnEnviar;
        internal FlowLayoutPanel panelTablas;
        internal FlowLayoutPanel panelHistorialCartas;
        internal Label lblHistorialTitulo;
        internal CheckBox chkAutoCantar;
        private Panel panelConexion;
        private Panel panelChat;

        private void InitializeComponent()
        {
            panelIzquierdo = new Panel();
            txtHistorialChat = new TextBox();
            panelChat = new Panel();
            txtChatInput = new TextBox();
            btnEnviar = new Button();
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
            ((System.ComponentModel.ISupportInitialize)picCartaActual).BeginInit();
            panelConexion.SuspendLayout();
            panelHistorialCartas.SuspendLayout();
            SuspendLayout();
            // 
            // panelIzquierdo
            // 
            panelIzquierdo.BackColor = Color.FromArgb(32, 32, 36);
            panelIzquierdo.Controls.Add(txtHistorialChat);
            panelIzquierdo.Controls.Add(panelChat);
            panelIzquierdo.Controls.Add(chkAutoCantar);
            panelIzquierdo.Controls.Add(btnGritarLoteria);
            panelIzquierdo.Controls.Add(picCartaActual);
            panelIzquierdo.Controls.Add(panelConexion);
            panelIzquierdo.Controls.Add(lblEstado);
            panelIzquierdo.Dock = DockStyle.Left;
            panelIzquierdo.Location = new Point(0, 0);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Padding = new Padding(10, 6, 10, 6);
            panelIzquierdo.Size = new Size(295, 1080);
            panelIzquierdo.TabIndex = 2;
            // 
            // txtHistorialChat
            // 
            txtHistorialChat.BackColor = Color.FromArgb(28, 28, 32);
            txtHistorialChat.BorderStyle = BorderStyle.None;
            txtHistorialChat.Dock = DockStyle.Fill;
            txtHistorialChat.Font = new Font("Consolas", 9F);
            txtHistorialChat.ForeColor = Color.FromArgb(190, 190, 195);
            txtHistorialChat.Location = new Point(10, 363);
            txtHistorialChat.Multiline = true;
            txtHistorialChat.Name = "txtHistorialChat";
            txtHistorialChat.ReadOnly = true;
            txtHistorialChat.ScrollBars = ScrollBars.Vertical;
            txtHistorialChat.Size = new Size(275, 675);
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
            panelChat.Size = new Size(275, 36);
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
            txtChatInput.Size = new Size(196, 27);
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
            // chkAutoCantar
            // 
            chkAutoCantar.AutoSize = true;
            chkAutoCantar.BackColor = Color.Transparent;
            chkAutoCantar.Cursor = Cursors.Hand;
            chkAutoCantar.Dock = DockStyle.Top;
            chkAutoCantar.Font = new Font("Segoe UI", 8F);
            chkAutoCantar.ForeColor = Color.FromArgb(160, 160, 165);
            chkAutoCantar.Location = new Point(10, 340);
            chkAutoCantar.Name = "chkAutoCantar";
            chkAutoCantar.Size = new Size(275, 23);
            chkAutoCantar.TabIndex = 2;
            chkAutoCantar.Text = "Auto (carta cada 8s)";
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
            btnGritarLoteria.Location = new Point(10, 288);
            btnGritarLoteria.Name = "btnGritarLoteria";
            btnGritarLoteria.Size = new Size(275, 52);
            btnGritarLoteria.TabIndex = 3;
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
            picCartaActual.Size = new Size(275, 226);
            picCartaActual.SizeMode = PictureBoxSizeMode.Zoom;
            picCartaActual.TabIndex = 4;
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
            panelConexion.Size = new Size(275, 34);
            panelConexion.TabIndex = 5;
            // 
            // txtSala
            // 
            txtSala.BackColor = Color.FromArgb(40, 40, 46);
            txtSala.BorderStyle = BorderStyle.FixedSingle;
            txtSala.Font = new Font("Segoe UI", 9F);
            txtSala.ForeColor = Color.White;
            txtSala.Location = new Point(0, 4);
            txtSala.Name = "txtSala";
            txtSala.Size = new Size(76, 27);
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
            btnAccionRed.Size = new Size(56, 24);
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
            btnSalir.TabIndex = 3;
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
            lblEstado.Size = new Size(275, 22);
            lblEstado.TabIndex = 6;
            lblEstado.Text = "Sin conexion";
            lblEstado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelTablas
            // 
            panelTablas.AutoScroll = true;
            panelTablas.BackColor = Color.FromArgb(28, 28, 32);
            panelTablas.Dock = DockStyle.Fill;
            panelTablas.Location = new Point(295, 0);
            panelTablas.Name = "panelTablas";
            panelTablas.Padding = new Padding(20, 16, 20, 16);
            panelTablas.Size = new Size(1625, 1042);
            panelTablas.TabIndex = 0;
            // 
            // panelHistorialCartas
            // 
            panelHistorialCartas.AutoScroll = true;
            panelHistorialCartas.BackColor = Color.FromArgb(20, 20, 22);
            panelHistorialCartas.Controls.Add(lblHistorialTitulo);
            panelHistorialCartas.Dock = DockStyle.Bottom;
            panelHistorialCartas.Location = new Point(295, 1042);
            panelHistorialCartas.Name = "panelHistorialCartas";
            panelHistorialCartas.Padding = new Padding(8, 26, 8, 4);
            panelHistorialCartas.Size = new Size(1625, 38);
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
            lblHistorialTitulo.Size = new Size(242, 19);
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
            ((System.ComponentModel.ISupportInitialize)picCartaActual).EndInit();
            panelConexion.ResumeLayout(false);
            panelConexion.PerformLayout();
            panelHistorialCartas.ResumeLayout(false);
            panelHistorialCartas.PerformLayout();
            ResumeLayout(false);
        }
        private void PanelHistorial_MouseEnter(object sender, System.EventArgs e)
        {
            panelHistorialCartas.Height = 106;
            lblHistorialTitulo.Text = "Historial de cartas cantadas";
        }

        private void PanelHistorial_MouseLeave(object sender, System.EventArgs e)
        {
            if (!panelHistorialCartas.DisplayRectangle.Contains(
                    panelHistorialCartas.PointToClient(System.Windows.Forms.Cursor.Position)))
            {
                panelHistorialCartas.Height = 38;
                lblHistorialTitulo.Text = "Historial de cartas  (pasa el cursor)";
            }
        }
    }
}