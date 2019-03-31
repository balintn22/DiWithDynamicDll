using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    public interface ICompositionRoot
    {
        void WriteSingleSpeciesName();
    }


    public class CompositionRoot : ICompositionRoot
    {
        readonly ISpeciesInfo _speciesInfo;


        public CompositionRoot(ISpeciesInfo speciesInfo)
        {
            _speciesInfo = speciesInfo;
        }

        public void WriteSingleSpeciesName( )
        {
            Console.WriteLine(_speciesInfo.GetRandomSpeciesName());
        }
    }
}
