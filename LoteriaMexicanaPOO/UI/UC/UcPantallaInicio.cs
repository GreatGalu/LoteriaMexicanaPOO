using System;
using System.Drawing;
using System.Windows.Forms;

namespace LoteriaMexicana.UI.UserControls
{
    public partial class UcPantallaInicio : UserControl
    {
        public event Action<string, int>        OnCrearPartida;
        public event Action<string, int, string> OnUnirse;

        public UcPantallaInicio()
        {
            InitializeComponent();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradas(out string nombre, out int tablas)) return;
            OnCrearPartida?.Invoke(nombre, tablas);
        }

        private void btnUnirse_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradas(out string nombre, out int tablas)) return;

            string ip = txtSala.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                MostrarError("Ingresa el codigo de sala o IP del servidor.");
                return;
            }

            OnUnirse?.Invoke(nombre, tablas, ip);
        }

        private bool ValidarEntradas(out string nombre, out int tablas)
        {
            nombre = txtNombre.Text.Trim();
            tablas = (int)nudTablas.Value;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarError("Por favor ingresa tu nombre.");
                txtNombre.Focus();
                return false;
            }
            return true;
        }

        private void MostrarError(string msg) =>
            MessageBox.Show(msg, "Datos incompletos",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void txtSala_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnUnirse_Click(null, EventArgs.Empty);
        }
    }
}
