using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using LoteriaMexicana.Models;

namespace LoteriaMexicana.Core
{
    public class GestorFiguras
    {
        private readonly List<FiguraGanar> _predefinidas;
        private readonly List<FiguraGanar> _personalizadas = new List<FiguraGanar>();

        public GestorFiguras()
        {
            _predefinidas = CargarPredefinidas();
        }

        public IReadOnlyList<FiguraGanar> Todas
        {
            get
            {
                var nombresPersonalizados = new HashSet<string>(
                    _personalizadas.Select(f => f.Nombre),
                    StringComparer.OrdinalIgnoreCase);

                var resultado = new List<FiguraGanar>();
                resultado.AddRange(_predefinidas.Where(p => !nombresPersonalizados.Contains(p.Nombre)));
                resultado.AddRange(_personalizadas);
                return resultado;
            }
        }

        public void CargarDesdeArchivo(string ruta)
        {
            if (!File.Exists(ruta)) return;
            try
            {
                string json = File.ReadAllText(ruta);
                var figuras = JsonSerializer.Deserialize<List<FiguraGanar>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (figuras == null) return;
                foreach (var f in figuras)
                {
                    _personalizadas.RemoveAll(p =>
                        p.Nombre.Equals(f.Nombre, StringComparison.OrdinalIgnoreCase));
                    _personalizadas.Add(f);
                }
            }
            catch { }
        }

        public void CargarDesdeDirectorio(string directorio)
        {
            if (!Directory.Exists(directorio)) return;
            foreach (var archivo in Directory.GetFiles(directorio, "*.json"))
                CargarDesdeArchivo(archivo);
        }

        public void GuardarEnArchivo(List<FiguraGanar> figuras, string ruta)
        {
            string json = JsonSerializer.Serialize(figuras,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ruta, json);
        }

        public void AgregarPersonalizada(FiguraGanar figura)
        {
            _personalizadas.RemoveAll(p =>
                p.Nombre.Equals(figura.Nombre, StringComparison.OrdinalIgnoreCase));
            _personalizadas.Add(figura);
        }

        private static List<FiguraGanar> CargarPredefinidas()
        {
            var lista = new List<FiguraGanar>();

            // Horizontal: cualquier fila completa
            for (int f = 0; f < 5; f++)
            {
                var casillas = new List<int[]>();
                for (int c = 0; c < 5; c++) casillas.Add(new[] { f, c });
                lista.Add(new FiguraGanar($"Horizontal fila {f + 1}", casillas));
            }

            // Vertical: cualquier columna completa
            for (int c = 0; c < 5; c++)
            {
                var casillas = new List<int[]>();
                for (int f = 0; f < 5; f++) casillas.Add(new[] { f, c });
                lista.Add(new FiguraGanar($"Vertical columna {c + 1}", casillas));
            }

            // Diagonal principal
            lista.Add(new FiguraGanar("Diagonal principal",
                Enumerable.Range(0, 5).Select(i => new[] { i, i }).ToList()));

            // Diagonal secundaria
            lista.Add(new FiguraGanar("Diagonal secundaria",
                Enumerable.Range(0, 5).Select(i => new[] { i, 4 - i }).ToList()));

            // Esquinas
            lista.Add(new FiguraGanar("Esquinas", new List<int[]>
            {
                new[] { 0, 0 }, new[] { 0, 4 },
                new[] { 4, 0 }, new[] { 4, 4 }
            }));

            // Poya / Cruz (fila 3 + columna 3, índice 2)
            var poya = new List<int[]>();
            for (int c = 0; c < 5; c++) poya.Add(new[] { 2, c });
            for (int f = 0; f < 5; f++) if (f != 2) poya.Add(new[] { f, 2 });
            lista.Add(new FiguraGanar("Poya / Cruz", poya));

            return lista;
        }
    }
}