using Castle.Windsor;
using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Animals
{
    public class CompositionSubRoot : ICompositionSubRoot
    {
        public void ExtendDiCompositionRegistration(WindsorContainer container)
        {
            container.Register(Component.For<ISpeciesInfo>().ImplementedBy<AnimalInfo>());
        }
    }
}
