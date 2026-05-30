using System;
using System.Collections.Generic;
using System.Text.Json;
using LoteriaMexicana.Models;

namespace LoteriaMexicana.Core
{
    /// <summary>
    /// Carga y expone el catálogo completo de 54 cartas.
    /// Lee desde Data/cartas.json si existe; si no, usa el catálogo
    /// canónico embebido como fallback para no romper el juego.
    ///
    /// Formato esperado del JSON:
    /// [
    ///   { "id": 1, "nombre": "El Gallo" },
    ///   ...
    /// ]
    ///
    /// Las rutas de imagen y audio se resuelven dinámicamente
    /// a través de GestorArchivos, sin almacenarlas en el JSON.
    /// </summary>
    public static class CatalogoCartas
    {
        private static List<Carta> _cartas;

        /// <summary>
        /// Devuelve la lista completa de cartas (carga perezosa, una sola vez).
        /// </summary>
        public static IReadOnlyList<Carta> Todas
        {
            get
            {
                _cartas ??= Cargar();
                return _cartas;
            }
        }

        /// <summary>Busca una carta por Id. Devuelve null si no existe.</summary>
        public static Carta BuscarPorId(int id) =>
            Todas.FirstOrDefault(c => c.Id == id);

        // ── Carga ─────────────────────────────────────────────────

        private static List<Carta> Cargar()
        {
            string json = GestorArchivos.LeerJsonCatalogo();
            if (json != null)
            {
                try { return DeserializarJson(json); }
                catch { /* JSON corrupto: usar fallback */ }
            }
            return CatalogoCanonico();
        }

        private static List<Carta> DeserializarJson(string json)
        {
            // Estructura mínima del JSON: array de { id, nombre }
            var registros = JsonSerializer.Deserialize<List<RegistroCarta>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var lista = new List<Carta>(registros.Count);
            foreach (var r in registros)
            {
                lista.Add(new Carta(
                    id:         r.Id,
                    nombre:     r.Nombre,
                    imagenPath: GestorArchivos.RutaImagen(r.Id),
                    audioPath:  GestorArchivos.RutaAudioCarta(r.Id) ?? string.Empty
                ));
            }
            return lista;
        }

        // ── Fallback canónico ─────────────────────────────────────

        private static List<Carta> CatalogoCanonico()
        {
            var datos = new (int id, string nombre)[]
            {
                ( 1,"El Gallo"),     ( 2,"El Diablito"),  ( 3,"La Dama"),
                ( 4,"El Catrín"),    ( 5,"El Paraguas"),  ( 6,"La Sirena"),
                ( 7,"La Escalera"),  ( 8,"La Botella"),   ( 9,"El Barril"),
                (10,"El Árbol"),     (11,"El Melón"),     (12,"El Valiente"),
                (13,"El Gorrito"),   (14,"La Muerte"),    (15,"La Pera"),
                (16,"La Bandera"),   (17,"El Bandolón"),  (18,"El Violoncello"),
                (19,"La Garza"),     (20,"El Pájaro"),    (21,"La Mano"),
                (22,"La Bota"),      (23,"La Luna"),      (24,"El Cotorro"),
                (25,"El Borracho"),  (26,"El Negrito"),   (27,"El Corazón"),
                (28,"La Sandía"),    (29,"El Tambor"),    (30,"El Camarón"),
                (31,"Las Jaras"),    (32,"El Músico"),    (33,"La Araña"),
                (34,"El Soldado"),   (35,"La Estrella"),  (36,"El Cazo"),
                (37,"El Mundo"),     (38,"El Apache"),    (39,"El Nopal"),
                (40,"El Alacrán"),   (41,"La Rosa"),      (42,"La Calavera"),
                (43,"La Campana"),   (44,"El Cantarito"), (45,"El Venado"),
                (46,"El Sol"),       (47,"La Corona"),    (48,"La Chalupa"),
                (49,"El Pino"),      (50,"El Pescado"),   (51,"La Palma"),
                (52,"La Maceta"),    (53,"El Arpa"),      (54,"La Rana"),
            };

            var lista = new List<Carta>(54);
            foreach (var (id, nombre) in datos)
            {
                lista.Add(new Carta(
                    id:         id,
                    nombre:     nombre,
                    imagenPath: GestorArchivos.RutaImagen(id),
                    audioPath:  GestorArchivos.RutaAudioCarta(id) ?? string.Empty
                ));
            }
            return lista;
        }

        // ── DTO interno para deserializar el JSON ─────────────────
        private class RegistroCarta
        {
            public int    Id     { get; set; }
            public string Nombre { get; set; }
        }
    }
}
