using LoteriaMexicana.Logic;
using LoteriaMexicana.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoteriaMexicanaPOO.UI.UC
{
    public partial class UcCreadorFigura : UserControl
    {
        public event Action<FiguraGanar> OnFiguraConfirmada;
        public event Action OnCancelado;

        private readonly bool[,] _seleccion = new bool[Tablero.FILAS, Tablero.COLUMNAS];
        private readonly Button[,] _celdas = new Button[Tablero.FILAS, Tablero.COLUMNAS];
        public UcCreadorFigura()
        {
            InitializeComponent();
            ConstruirGrid();
            btnConfirmar.Click += BtnConfirmar_Click;
            btnLimpiar.Click += (s, e) => LimpiarGrid();
            btnCancelar.Click += (s, e) => OnCancelado?.Invoke();
        }

        private void ConstruirGrid()
        {
            const int TAM = 52;
            const int ESP = 4;

            for (int f = 0; f < Tablero.FILAS; f++)
            {
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    int fi = f, ci = c;
                    var btn = new Button
                    {
                        Size = new Size(TAM, TAM),
                        Location = new Point(c * (TAM + ESP), f * (TAM + ESP)),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.FromArgb(50, 50, 52),
                        ForeColor = Color.FromArgb(160, 160, 165),
                        Font = new Font("Segoe UI", 8F),
                        Text = $"{f},{c}",
                        Cursor = Cursors.Hand
                    };
                    btn.FlatAppearance.BorderColor = Color.FromArgb(70, 70, 75);
                    btn.Click += (s, e) => AlternarCelda(fi, ci);
                    _celdas[f, c] = btn;
                    panelGrid.Controls.Add(btn);
                }
            }
        }

        private void AlternarCelda(int f, int c)
        {
            _seleccion[f, c] = !_seleccion[f, c];
            _celdas[f, c].BackColor = _seleccion[f, c]
                ? Color.FromArgb(200, 120, 40)
                : Color.FromArgb(50, 50, 52);
            ActualizarContador();
        }

        private void LimpiarGrid()
        {
            for (int f = 0; f < Tablero.FILAS; f++)
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    _seleccion[f, c] = false;
                    _celdas[f, c].BackColor = Color.FromArgb(50, 50, 52);
                }
            ActualizarContador();
        }

        private void ActualizarContador()
        {
            int total = 0;
            for (int f = 0; f < Tablero.FILAS; f++)
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                    if (_seleccion[f, c]) total++;
            lblContador.Text = $"{total} casillas seleccionadas";
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingresa un nombre para la figura.", "Nombre requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            var casillas = new List<int[]>();
            for (int f = 0; f < Tablero.FILAS; f++)
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                    if (_seleccion[f, c]) casillas.Add(new[] { f, c });

            if (casillas.Count == 0)
            {
                MessageBox.Show("Selecciona al menos una casilla.", "Sin casillas",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OnFiguraConfirmada?.Invoke(new FiguraGanar(nombre, casillas));
        }
    }
}
