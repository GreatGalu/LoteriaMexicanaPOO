using System;
using System.Drawing;
using System.Windows.Forms;

namespace LoteriaMexicana.UI.UserControls
{
    public partial class UcPostPartida : UserControl
    {
        public event Action OnNuevaPartida;
        public event Action OnSalirAlMenu;

        public UcPostPartida(string ganador, string figura, bool esAnfitrion)
        {
            InitializeComponent();

            lblGanador.Text = $"🏆  {ganador}";
            lblFigura.Text = figura;

            btnNuevaPartida.Enabled = esAnfitrion;
            btnNuevaPartida.Text = esAnfitrion
                ? "▶  Nueva partida"
                : "Esperando al anfitrión...";

            if (!esAnfitrion)
                btnNuevaPartida.BackColor = Color.FromArgb(50, 50, 55);

            btnNuevaPartida.Click += (s, e) => OnNuevaPartida?.Invoke();
            btnSalirAlMenu.Click += (s, e) => OnSalirAlMenu?.Invoke();

            // Centrar al cambiar tamanio
            this.Resize += (s, e) => CentrarTarjetaPublico();
        }

        /// <summary>
        /// Centra el recuadro (panelTarjeta) en el control.
        /// El FormJuego lo llama despues de montar el overlay y al redimensionar.
        /// </summary>
        public void CentrarTarjetaPublico()
        {
            if (panelTarjeta == null || panelTarjeta.IsDisposed) return;
            panelTarjeta.Left = (Width - panelTarjeta.Width) / 2;
            panelTarjeta.Top = (Height - panelTarjeta.Height) / 2;
        }

        // Fondo semitransparente oscuro sobre la pantalla de juego
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using var brush = new SolidBrush(Color.FromArgb(180, 10, 10, 12));
            e.Graphics.FillRectangle(brush, ClientRectangle);
        }
    }
}