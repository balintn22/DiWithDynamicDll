using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace App
{
    public class AssemblyHelper
    {
        public static Type FindTypeThatImplementsInterface(Assembly assembly, Type iFace)
        {
            IEnumerable<Type> implementingTypes =
                assembly.GetTypes().Where(type => type.GetInterface(iFace.Name) != null);

            if (implementingTypes.Count() == 0)
                throw new Exception($"No type found in assembly {assembly.FullName} that implements {iFace.Name}.");

            if (implementingTypes.Count() == 0)
                throw new Exception($"More than one tpye ({string.Join(",", implementingTypes)}) found in assembly {assembly.FullName} that implements {iFace.Name}.");

            return implementingTypes.First();
        }
    }
}
