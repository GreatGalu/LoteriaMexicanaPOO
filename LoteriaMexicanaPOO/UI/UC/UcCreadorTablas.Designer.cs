using LoteriaMexicana.Logic;
using System.Drawing;
using System.Windows.Forms;

namespace LoteriaMexicana.UI.UserControls
{
    partial class UcCreadorTablas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        internal Label lblTituloCreador;
        internal Label lblContador;
        internal Panel panelGrid;
        internal FlowLayoutPanel panelCatalogo;
        internal Button btnConfirmar;
        internal Button btnGuardarJson;
        internal Button btnLimpiar;
        internal Button btnAleatorio;
        private Panel panelSuperior;
        private Panel panelInferior;
        private Label lblCatalogoTitulo;
        private TableLayoutPanel tableLayoutPrincipal;

        private void InitializeComponent()
        {
            lblTituloCreador = new Label();
            lblContador = new Label();
            panelGrid = new Panel();
            panelCatalogo = new FlowLayoutPanel();
            btnConfirmar = new Button();
            btnGuardarJson = new Button();
            btnLimpiar = new Button();
            btnAleatorio = new Button();
            panelSuperior = new Panel();
            panelInferior = new Panel();
            lblCatalogoTitulo = new Label();
            tableLayoutPrincipal = new TableLayoutPanel();
            panelSuperior.SuspendLayout();
            panelInferior.SuspendLayout();
            tableLayoutPrincipal.SuspendLayout();
            SuspendLayout();
            // 
            // lblTituloCreador
            // 
            lblTituloCreador.Dock = DockStyle.Top;
            lblTituloCreador.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTituloCreador.ForeColor = Color.FromArgb(230, 180, 60);
            lblTituloCreador.Location = new Point(0, 0);
            lblTituloCreador.Name = "lblTituloCreador";
            lblTituloCreador.Size = new Size(1920, 36);
            lblTituloCreador.TabIndex = 1;
            lblTituloCreador.Text = "TABLA 1 — Elige 20 cartas en orden";
            lblTituloCreador.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblContador
            // 
            lblContador.Dock = DockStyle.Fill;
            lblContador.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblContador.ForeColor = Color.FromArgb(120, 200, 120);
            lblContador.Location = new Point(0, 36);
            lblContador.Name = "lblContador";
            lblContador.Size = new Size(1920, 28);
            lblContador.TabIndex = 0;
            lblContador.Text = "0 / 20 seleccionadas";
            lblContador.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelGrid
            // 
            panelGrid.BackColor = Color.FromArgb(32, 32, 36);
            panelGrid.Location = new Point(0, 0);
            panelGrid.Margin = new Padding(0);
            panelGrid.Name = "panelGrid";
            panelGrid.Size = new Size(929, 961);
            panelGrid.TabIndex = 0;
            // 
            // panelCatalogo
            // 
            panelCatalogo.AutoScroll = true;
            panelCatalogo.BackColor = Color.FromArgb(28, 28, 32);
            panelCatalogo.Location = new Point(929, 0);
            panelCatalogo.Margin = new Padding(0);
            panelCatalogo.Name = "panelCatalogo";
            panelCatalogo.Padding = new Padding(12);
            panelCatalogo.Size = new Size(983, 961);
            panelCatalogo.TabIndex = 1;
            // 
            // btnConfirmar
            // 
            btnConfirmar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnConfirmar.BackColor = Color.FromArgb(30, 120, 60);
            btnConfirmar.Cursor = Cursors.Hand;
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.Location = new Point(1772, 10);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(140, 32);
            btnConfirmar.TabIndex = 0;
            btnConfirmar.Text = "✔ Confirmar";
            btnConfirmar.UseVisualStyleBackColor = false;
            // 
            // btnGuardarJson
            // 
            btnGuardarJson.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGuardarJson.BackColor = Color.FromArgb(40, 80, 140);
            btnGuardarJson.Cursor = Cursors.Hand;
            btnGuardarJson.FlatAppearance.BorderSize = 0;
            btnGuardarJson.FlatStyle = FlatStyle.Flat;
            btnGuardarJson.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardarJson.ForeColor = Color.White;
            btnGuardarJson.Location = new Point(1632, 10);
            btnGuardarJson.Name = "btnGuardarJson";
            btnGuardarJson.Size = new Size(130, 32);
            btnGuardarJson.TabIndex = 3;
            btnGuardarJson.Text = "💾 Guardar JSON";
            btnGuardarJson.UseVisualStyleBackColor = false;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLimpiar.BackColor = Color.FromArgb(140, 40, 40);
            btnLimpiar.Cursor = Cursors.Hand;
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.Location = new Point(1512, 10);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(110, 32);
            btnLimpiar.TabIndex = 1;
            btnLimpiar.Text = "🗑 Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            // 
            // btnAleatorio
            // 
            btnAleatorio.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAleatorio.BackColor = Color.FromArgb(55, 55, 65);
            btnAleatorio.Cursor = Cursors.Hand;
            btnAleatorio.FlatAppearance.BorderSize = 0;
            btnAleatorio.FlatStyle = FlatStyle.Flat;
            btnAleatorio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAleatorio.ForeColor = Color.White;
            btnAleatorio.Location = new Point(1382, 10);
            btnAleatorio.Name = "btnAleatorio";
            btnAleatorio.Size = new Size(120, 32);
            btnAleatorio.TabIndex = 2;
            btnAleatorio.Text = "🎲 Aleatorio";
            btnAleatorio.UseVisualStyleBackColor = false;
            // 
            // panelSuperior
            // 
            panelSuperior.BackColor = Color.FromArgb(24, 24, 26);
            panelSuperior.Controls.Add(lblContador);
            panelSuperior.Controls.Add(lblTituloCreador);
            panelSuperior.Dock = DockStyle.Top;
            panelSuperior.Location = new Point(0, 0);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Size = new Size(1920, 64);
            panelSuperior.TabIndex = 2;
            // 
            // panelInferior
            // 
            panelInferior.BackColor = Color.FromArgb(20, 20, 22);
            panelInferior.Controls.Add(btnConfirmar);
            panelInferior.Controls.Add(btnGuardarJson);
            panelInferior.Controls.Add(btnLimpiar);
            panelInferior.Controls.Add(btnAleatorio);
            panelInferior.Dock = DockStyle.Bottom;
            panelInferior.Location = new Point(0, 1028);
            panelInferior.Name = "panelInferior";
            panelInferior.Padding = new Padding(8);
            panelInferior.Size = new Size(1920, 52);
            panelInferior.TabIndex = 1;
            // 
            // lblCatalogoTitulo
            // 
            lblCatalogoTitulo.Location = new Point(0, 0);
            lblCatalogoTitulo.Name = "lblCatalogoTitulo";
            lblCatalogoTitulo.Size = new Size(100, 23);
            lblCatalogoTitulo.TabIndex = 0;
            // 
            // tableLayoutPrincipal
            // 
            tableLayoutPrincipal.ColumnCount = 2;
            tableLayoutPrincipal.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPrincipal.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPrincipal.Controls.Add(panelGrid, 0, 0);
            tableLayoutPrincipal.Controls.Add(panelCatalogo, 1, 0);
            tableLayoutPrincipal.Dock = DockStyle.Fill;
            tableLayoutPrincipal.Location = new Point(0, 64);
            tableLayoutPrincipal.Name = "tableLayoutPrincipal";
            tableLayoutPrincipal.RowCount = 1;
            tableLayoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPrincipal.Size = new Size(1920, 964);
            tableLayoutPrincipal.TabIndex = 0;
            // 
            // UcCreadorTablas
            // 
            BackColor = Color.FromArgb(24, 24, 26);
            Controls.Add(tableLayoutPrincipal);
            Controls.Add(panelInferior);
            Controls.Add(panelSuperior);
            Name = "UcCreadorTablas";
            Size = new Size(1920, 1080);
            panelSuperior.ResumeLayout(false);
            panelInferior.ResumeLayout(false);
            tableLayoutPrincipal.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void ConstruirGrid()
        {
            const int ancho = 74, alto = 97, esp = 4, padX = 16, padY = 12;

            for (int f = 0; f < Tablero.FILAS; f++)
            {
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    var pic = new PictureBox
                    {
                        Width = ancho,
                        Height = alto,
                        Left = padX + c * (ancho + esp),
                        Top = padY + f * (alto + esp),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.FromArgb(36, 36, 40),
                        Cursor = Cursors.Hand,
                        Tag = -1
                    };

                    var lbl = new Label
                    {
                        Text = $"{f * Tablero.COLUMNAS + c + 1}",
                        ForeColor = Color.FromArgb(80, 80, 85),
                        Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(3, 2),
                        BackColor = Color.Transparent
                    };
                    pic.Controls.Add(lbl);

                    int fi = f, co = c;
                    pic.Click += (s, e) => QuitarDesdeCelda(fi, co);

                    _celdas[f, c] = pic;
                    panelGrid.Controls.Add(pic);
                }
            }
        }
    }
}