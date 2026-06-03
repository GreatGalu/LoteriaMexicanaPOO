using System.Drawing;
using System.Windows.Forms;

namespace LoteriaMexicana.UI
{
    partial class FormJuego
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        internal Panel contenedor;

        private void InitializeComponent()
        {
            components  = new System.ComponentModel.Container();
            contenedor  = new Panel();

            this.SuspendLayout();

            contenedor.Dock      = DockStyle.Fill;
            contenedor.BackColor = Color.FromArgb(24, 24, 26);

            this.Text          = "Loteria Mexicana Online";
            this.BackColor     = Color.FromArgb(24, 24, 26);
            this.MinimumSize   = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState     = FormWindowState.Maximized;
            this.KeyPreview      = true;

            this.Controls.Add(contenedor);
            this.ResumeLayout(false);
        }
    }
}
