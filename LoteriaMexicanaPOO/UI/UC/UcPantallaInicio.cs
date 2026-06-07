using System;
using System.Drawing;
using System.Windows.Forms;

namespace LoteriaMexicana.UI.UserControls
{
    public partial class UcPantallaInicio : UserControl
    {
        public event Action<string, int> OnCrearPartida;
        public event Action<string, int, string> OnUnirse;

        public UcPantallaInicio()
        {
            InitializeComponent();
            CentrarTarjeta();
            this.Resize += (s, e) => CentrarTarjeta();
            txtSala.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) btnUnirse_Click(null, null); };
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradas(out string nombre, out int tablas)) return;
            OnCrearPartida?.Invoke(nombre, tablas);
        }

        private void btnUnirse_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradas(out string nombre, out int tablas)) return;
            string sala = txtSala.Text.Trim();
            if (string.IsNullOrWhiteSpace(sala))
            {
                MessageBox.Show("Ingresa el código de sala o IP.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OnUnirse?.Invoke(nombre, tablas, sala);
        }

        private bool ValidarEntradas(out string nombre, out int tablas)
        {
            nombre = txtNombre.Text.Trim();
            tablas = (int)nudTablas.Value;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Por favor ingresa tu nombre.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }
            return true;
        }

        private void CentrarTarjeta()
        {
            panelTarjeta.Left = (Width - panelTarjeta.Width) / 2;
            panelTarjeta.Top = (Height - panelTarjeta.Height) / 2 + 20;
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void lblVersion_Click(object sender, EventArgs e)
        {

        }

        private void lblSala_Click(object sender, EventArgs e)
        {

        }
    }
}