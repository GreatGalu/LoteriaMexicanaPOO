namespace LoteriaMexicanaPOO.UI.UC
{
    partial class UcCreadorFigura
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            panelGrid = new Panel();
            txtNombre = new TextBox();
            lblNombre = new Label();
            lblContador = new Label();
            btnConfirmar = new Button();
            btnLimpiar = new Button();
            btnCancelar = new Button();

            SuspendLayout();

            // ── fondo general ────────────────────────────────────────────────
            BackColor = Color.FromArgb(24, 24, 28);
            Size = new Size(700, 500);

            // ── lblTitulo ────────────────────────────────────────────────────
            var lblTitulo = new Label
            {
                Text = "CREAR FIGURA DE VICTORIA",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 160, 40),
                AutoSize = true,
                Location = new Point(24, 22)
            };

            // ── panelGrid ────────────────────────────────────────────────────
            panelGrid.BackColor = Color.FromArgb(32, 32, 36);
            panelGrid.Location = new Point(24, 70);
            panelGrid.Size = new Size(284, 284);
            panelGrid.Name = "panelGrid";

            // ── panel derecho ────────────────────────────────────────────────
            var panelDerecho = new Panel
            {
                BackColor = Color.FromArgb(30, 30, 34),
                Location = new Point(340, 70),
                Size = new Size(320, 284),
                Name = "panelDerecho"
            };

            lblNombre.Text = "Nombre de la figura:";
            lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombre.ForeColor = Color.FromArgb(180, 180, 190);
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(12, 18);

            txtNombre.Font = new Font("Segoe UI", 10F);
            txtNombre.BackColor = Color.FromArgb(42, 42, 48);
            txtNombre.ForeColor = Color.FromArgb(220, 220, 225);
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Location = new Point(12, 42);
            txtNombre.Size = new Size(296, 28);
            txtNombre.Name = "txtNombre";

            lblContador.Text = "0 casillas seleccionadas";
            lblContador.Font = new Font("Segoe UI", 9F);
            lblContador.ForeColor = Color.FromArgb(140, 180, 140);
            lblContador.AutoSize = true;
            lblContador.Location = new Point(12, 86);
            lblContador.Name = "lblContador";

            var lblAyuda = new Label
            {
                Text = "Haz clic en las casillas del tablero\npara marcar las posiciones que\nforman tu figura.",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(120, 120, 130),
                AutoSize = true,
                Location = new Point(12, 114)
            };

            panelDerecho.Controls.Add(lblNombre);
            panelDerecho.Controls.Add(txtNombre);
            panelDerecho.Controls.Add(lblContador);
            panelDerecho.Controls.Add(lblAyuda);

            // ── botones ──────────────────────────────────────────────────────
            EstilarBoton(btnConfirmar, "✔ Confirmar", Color.FromArgb(40, 110, 60));
            EstilarBoton(btnLimpiar, "✖ Limpiar", Color.FromArgb(80, 60, 30));
            EstilarBoton(btnCancelar, "← Cancelar", Color.FromArgb(60, 40, 40));

            btnConfirmar.Location = new Point(24, 380);
            btnLimpiar.Location = new Point(154, 380);
            btnCancelar.Location = new Point(284, 380);

            Controls.Add(lblTitulo);
            Controls.Add(panelGrid);
            Controls.Add(panelDerecho);
            Controls.Add(btnConfirmar);
            Controls.Add(btnLimpiar);
            Controls.Add(btnCancelar);

            ResumeLayout(false);
        }

        private static void EstilarBoton(Button btn, string texto, Color fondo)
        {
            btn.Text = texto;
            btn.Size = new Size(120, 34);
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = fondo;
            btn.ForeColor = Color.FromArgb(220, 220, 225);
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 90);
        }

        #endregion
        internal Panel panelGrid;
        internal TextBox txtNombre;
        internal Label lblNombre;
        internal Label lblContador;
        internal Button btnConfirmar;
        internal Button btnLimpiar;
        internal Button btnCancelar;
    }
}
