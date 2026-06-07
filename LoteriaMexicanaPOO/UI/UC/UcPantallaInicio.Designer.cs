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

        // Controles declarados
        internal Panel panelTarjeta;
        internal Label lblTitulo;
        internal Label lblSubtitulo;
        internal Label lblNombre;
        internal TextBox txtNombre;
        internal Label lblTablas;
        internal NumericUpDown nudTablas;
        internal Label lblSala;
        internal TextBox txtSala;
        internal Button btnCrear;
        internal Button btnUnirse;
        internal Label lblVersion;

        private void InitializeComponent()
        {
            panelTarjeta = new Panel();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblTablas = new Label();
            nudTablas = new NumericUpDown();
            lblSala = new Label();
            txtSala = new TextBox();
            btnCrear = new Button();
            btnUnirse = new Button();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            lblVersion = new Label();
            panelTarjeta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudTablas).BeginInit();
            SuspendLayout();
            // 
            // panelTarjeta
            // 
            panelTarjeta.BackColor = Color.FromArgb(32, 32, 36);
            panelTarjeta.Controls.Add(lblNombre);
            panelTarjeta.Controls.Add(txtNombre);
            panelTarjeta.Controls.Add(lblTablas);
            panelTarjeta.Controls.Add(nudTablas);
            panelTarjeta.Controls.Add(lblSala);
            panelTarjeta.Controls.Add(txtSala);
            panelTarjeta.Controls.Add(btnCrear);
            panelTarjeta.Controls.Add(btnUnirse);
            panelTarjeta.Location = new Point(761, 241);
            panelTarjeta.Name = "panelTarjeta";
            panelTarjeta.Size = new Size(400, 220);
            panelTarjeta.TabIndex = 0;
            // 
            // lblNombre
            // 
            lblNombre.Font = new Font("Segoe UI", 9F);
            lblNombre.ForeColor = Color.FromArgb(160, 160, 165);
            lblNombre.Location = new Point(32, 28);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(340, 18);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre del jugador";
            // 
            // txtNombre
            // 
            txtNombre.BackColor = Color.FromArgb(44, 44, 50);
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 10F);
            txtNombre.ForeColor = Color.White;
            txtNombre.Location = new Point(32, 48);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(340, 30);
            txtNombre.TabIndex = 0;
            // 
            // lblTablas
            // 
            lblTablas.Font = new Font("Segoe UI", 9F);
            lblTablas.ForeColor = Color.FromArgb(160, 160, 165);
            lblTablas.Location = new Point(32, 88);
            lblTablas.Name = "lblTablas";
            lblTablas.Size = new Size(160, 18);
            lblTablas.TabIndex = 1;
            lblTablas.Text = "Cantidad de tablas (1-3)";
            // 
            // nudTablas
            // 
            nudTablas.BackColor = Color.FromArgb(44, 44, 50);
            nudTablas.Font = new Font("Segoe UI", 10F);
            nudTablas.ForeColor = Color.White;
            nudTablas.Location = new Point(32, 108);
            nudTablas.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            nudTablas.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudTablas.Name = "nudTablas";
            nudTablas.Size = new Size(155, 30);
            nudTablas.TabIndex = 1;
            nudTablas.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblSala
            // 
            lblSala.Font = new Font("Segoe UI", 9F);
            lblSala.ForeColor = Color.FromArgb(160, 160, 165);
            lblSala.Location = new Point(200, 88);
            lblSala.Name = "lblSala";
            lblSala.Size = new Size(172, 18);
            lblSala.TabIndex = 2;
            lblSala.Text = "Código de sala (para unirse)";
            // 
            // txtSala
            // 
            txtSala.BackColor = Color.FromArgb(44, 44, 50);
            txtSala.BorderStyle = BorderStyle.FixedSingle;
            txtSala.Font = new Font("Segoe UI", 10F);
            txtSala.ForeColor = Color.White;
            txtSala.Location = new Point(200, 108);
            txtSala.Name = "txtSala";
            txtSala.Size = new Size(172, 30);
            txtSala.TabIndex = 2;
            // 
            // btnCrear
            // 
            btnCrear.BackColor = Color.FromArgb(180, 120, 20);
            btnCrear.Cursor = Cursors.Hand;
            btnCrear.FlatAppearance.BorderSize = 0;
            btnCrear.FlatStyle = FlatStyle.Flat;
            btnCrear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCrear.ForeColor = Color.White;
            btnCrear.Location = new Point(32, 156);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(165, 44);
            btnCrear.TabIndex = 3;
            btnCrear.Text = "CREAR PARTIDA";
            btnCrear.UseVisualStyleBackColor = false;
            btnCrear.Click += btnCrear_Click;
            // 
            // btnUnirse
            // 
            btnUnirse.BackColor = Color.FromArgb(40, 100, 160);
            btnUnirse.Cursor = Cursors.Hand;
            btnUnirse.FlatAppearance.BorderSize = 0;
            btnUnirse.FlatStyle = FlatStyle.Flat;
            btnUnirse.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnUnirse.ForeColor = Color.White;
            btnUnirse.Location = new Point(207, 156);
            btnUnirse.Name = "btnUnirse";
            btnUnirse.Size = new Size(165, 44);
            btnUnirse.TabIndex = 4;
            btnUnirse.Text = "UNIRSE";
            btnUnirse.UseVisualStyleBackColor = false;
            btnUnirse.Click += btnUnirse_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(230, 180, 60);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1920, 70);
            lblTitulo.TabIndex = 3;
            lblTitulo.Text = "LOTERÍA MEXICANA";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Click += lblTitulo_Click;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.BackColor = Color.Transparent;
            lblSubtitulo.Dock = DockStyle.Top;
            lblSubtitulo.Font = new Font("Segoe UI", 10F);
            lblSubtitulo.ForeColor = Color.FromArgb(130, 130, 135);
            lblSubtitulo.Location = new Point(0, 70);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(1920, 28);
            lblSubtitulo.TabIndex = 2;
            lblSubtitulo.Text = "Multijugador Online / LAN";
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            lblVersion.Dock = DockStyle.Bottom;
            lblVersion.Font = new Font("Segoe UI", 8F);
            lblVersion.ForeColor = Color.FromArgb(70, 70, 75);
            lblVersion.Location = new Point(0, 1056);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(1920, 24);
            lblVersion.TabIndex = 1;
            lblVersion.Text = "S.k.M.k.G";
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            lblVersion.Click += lblVersion_Click;
            // 
            // UcPantallaInicio
            // 
            BackColor = Color.FromArgb(24, 24, 26);
            Controls.Add(panelTarjeta);
            Controls.Add(lblVersion);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblTitulo);
            Name = "UcPantallaInicio";
            Size = new Size(1920, 1080);
            panelTarjeta.ResumeLayout(false);
            panelTarjeta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudTablas).EndInit();
            ResumeLayout(false);
        }
    }
}