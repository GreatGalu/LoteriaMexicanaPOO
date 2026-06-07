using System.Drawing;
using System.Windows.Forms;

namespace LoteriaMexicana.UI.UserControls
{
    partial class UcPostPartida
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        // ── Controles ────────────────────────────────────────────────────
        // REGLA: solo controles estandar WinForms con propiedades de valor literal.
        // Sin lambdas, sin bucles, sin logica — todo eso va en el .cs

        internal Panel panelTarjeta;
        internal Label lblTitulo;
        internal Label lblGanador;
        internal Label lblFigura;
        internal Panel panelSeparador;
        internal Button btnNuevaPartida;
        internal Button btnSalirAlMenu;

        private void InitializeComponent()
        {
            panelTarjeta = new Panel();
            btnNuevaPartida = new Button();
            btnSalirAlMenu = new Button();
            panelSeparador = new Panel();
            lblFigura = new Label();
            lblGanador = new Label();
            lblTitulo = new Label();
            panelTarjeta.SuspendLayout();
            SuspendLayout();
            // 
            // panelTarjeta
            // 
            panelTarjeta.BackColor = Color.FromArgb(36, 36, 40);
            panelTarjeta.Controls.Add(btnNuevaPartida);
            panelTarjeta.Controls.Add(btnSalirAlMenu);
            panelTarjeta.Controls.Add(panelSeparador);
            panelTarjeta.Controls.Add(lblFigura);
            panelTarjeta.Controls.Add(lblGanador);
            panelTarjeta.Controls.Add(lblTitulo);
            panelTarjeta.Location = new Point(100, 100);
            panelTarjeta.Name = "panelTarjeta";
            panelTarjeta.Size = new Size(400, 280);
            panelTarjeta.TabIndex = 0;
            // 
            // btnNuevaPartida
            // 
            btnNuevaPartida.BackColor = Color.FromArgb(40, 120, 60);
            btnNuevaPartida.Cursor = Cursors.Hand;
            btnNuevaPartida.Dock = DockStyle.Bottom;
            btnNuevaPartida.FlatAppearance.BorderSize = 0;
            btnNuevaPartida.FlatStyle = FlatStyle.Flat;
            btnNuevaPartida.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNuevaPartida.ForeColor = Color.White;
            btnNuevaPartida.Location = new Point(0, 192);
            btnNuevaPartida.Name = "btnNuevaPartida";
            btnNuevaPartida.Size = new Size(400, 44);
            btnNuevaPartida.TabIndex = 0;
            btnNuevaPartida.Text = "▶  Nueva partida";
            btnNuevaPartida.UseVisualStyleBackColor = false;
            // 
            // btnSalirAlMenu
            // 
            btnSalirAlMenu.BackColor = Color.FromArgb(70, 70, 80);
            btnSalirAlMenu.Cursor = Cursors.Hand;
            btnSalirAlMenu.Dock = DockStyle.Bottom;
            btnSalirAlMenu.FlatAppearance.BorderSize = 0;
            btnSalirAlMenu.FlatStyle = FlatStyle.Flat;
            btnSalirAlMenu.Font = new Font("Segoe UI", 10F);
            btnSalirAlMenu.ForeColor = Color.White;
            btnSalirAlMenu.Location = new Point(0, 236);
            btnSalirAlMenu.Name = "btnSalirAlMenu";
            btnSalirAlMenu.Size = new Size(400, 44);
            btnSalirAlMenu.TabIndex = 1;
            btnSalirAlMenu.Text = "✖  Salir al menú";
            btnSalirAlMenu.UseVisualStyleBackColor = false;
            // 
            // panelSeparador
            // 
            panelSeparador.BackColor = Color.Transparent;
            panelSeparador.Dock = DockStyle.Top;
            panelSeparador.Location = new Point(0, 124);
            panelSeparador.Name = "panelSeparador";
            panelSeparador.Size = new Size(400, 12);
            panelSeparador.TabIndex = 2;
            // 
            // lblFigura
            // 
            lblFigura.BackColor = Color.Transparent;
            lblFigura.Dock = DockStyle.Top;
            lblFigura.Font = new Font("Segoe UI", 10F);
            lblFigura.ForeColor = Color.FromArgb(120, 200, 120);
            lblFigura.Location = new Point(0, 96);
            lblFigura.Name = "lblFigura";
            lblFigura.Size = new Size(400, 28);
            lblFigura.TabIndex = 3;
            lblFigura.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblGanador
            // 
            lblGanador.BackColor = Color.Transparent;
            lblGanador.Dock = DockStyle.Top;
            lblGanador.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblGanador.ForeColor = Color.White;
            lblGanador.Location = new Point(0, 56);
            lblGanador.Name = "lblGanador";
            lblGanador.Size = new Size(400, 40);
            lblGanador.TabIndex = 4;
            lblGanador.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(230, 180, 60);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(400, 56);
            lblTitulo.TabIndex = 5;
            lblTitulo.Text = "¡ L O T E R Í A !";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // UcPostPartida
            // 
            BackColor = Color.Transparent;
            Controls.Add(panelTarjeta);
            Name = "UcPostPartida";
            Size = new Size(1833, 823);
            panelTarjeta.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}