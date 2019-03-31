using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System.Linq;
using System.Text;
using System.Reflection;
using Contract;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();

            // Register the CompositionRoot type with the container
            container.Register(Component.For<ICompositionRoot>().ImplementedBy<CompositionRoot>());

            // TODO
            // Dynamic load one of the assemblies and use it to extend DI container registrations.
//            Assembly dynamicAssembly = Assembly.LoadFrom(@"..\..\..\Animals\bin\Debug\Animals.dll");
            Assembly dynamicAssembly = Assembly.LoadFrom(@"..\..\..\Plants\bin\Debug\Plants.dll");
            Type compositionSubRootType = AssemblyHelper.FindTypeThatImplementsInterface(dynamicAssembly, typeof(ICompositionSubRoot));
            ICompositionSubRoot subRoot = dynamicAssembly.CreateInstance(compositionSubRootType.FullName) as ICompositionSubRoot;
            subRoot.ExtendDiCompositionRegistration(container);

            // Extend DI component registrations make sure that it all happens before resolving ICompositionRoot
            // (using the type registrations that come from the just loaded assembly)

            // Resolve an object of type ICompositionRoot (ask the container for an instance)
            // This is analagous to calling new() in a non-IoC application.
            var root = container.Resolve<ICompositionRoot>();

            root.WriteSingleSpeciesName();

            // Wait for user input so they can check the program's output.
            Console.ReadLine();
        }
    }
}
