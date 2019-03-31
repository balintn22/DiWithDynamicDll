using Castle.Windsor;
using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Plants
{
    public class PlantInfo : ISpeciesInfo
    {
        private Random _rnd = new Random();

        public string GetRandomSpeciesName()
        {
            List<string> species = new List<string>
            {
                "cedar", "ash", "fir", "lime", "orange",
            };

            int index = _rnd.Next(species.Count);
            return species[index];
        }
    }
}
