using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Contract;

namespace Plants
{
    public class CompositionSubRoot : ICompositionSubRoot
    {
        public void ExtendDiCompositionRegistration(WindsorContainer container)
        {
            container.Register(Component.For<ISpeciesInfo>().ImplementedBy<PlantInfo>());
        }
    }
}
