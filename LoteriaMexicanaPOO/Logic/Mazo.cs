using System;
using System.Collections.Generic;
using System.Linq;
using LoteriaMexicana.Core;
using LoteriaMexicana.Models;

namespace LoteriaMexicana.Logic
{
    public class Mazo
    {
        private readonly List<Carta> _cartas;
        private readonly Random      _rng;

        public int  CartasRestantes => _cartas.Count;
        public bool EstaAgotado     => _cartas.Count == 0;

        public Mazo()
        {
            _rng    = new Random();
            _cartas = new List<Carta>();
            Reiniciar();
        }

        public void Barajar()
        {
            int n = _cartas.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (_cartas[i], _cartas[j]) = (_cartas[j], _cartas[i]);
            }
        }

        public Carta SacarCarta()
        {
            if (EstaAgotado)
                throw new InvalidOperationException("El mazo esta agotado.");
            var carta = _cartas[0];
            _cartas.RemoveAt(0);
            return carta;
        }

        public void Reiniciar(int? idCartaDoble = null)
        {
            _cartas.Clear();
            _cartas.AddRange(CatalogoCartas.Todas);
        }
    }
}
