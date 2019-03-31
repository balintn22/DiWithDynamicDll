using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animals
{
    public class AnimalInfo : ISpeciesInfo
    {
        private Random _rnd = new Random();

        public string GetRandomSpeciesName()
        {
            List<string> species = new List<string>
            {
                "lion", "tiger", "cat", "dog", "elephant",
            };

            int index = _rnd.Next(species.Count);
            return species[index];
        }
    }
}
