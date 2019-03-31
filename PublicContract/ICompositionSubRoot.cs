using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contract
{
    public interface ICompositionSubRoot
    {
        void ExtendDiCompositionRegistration(WindsorContainer container);
    }
}
