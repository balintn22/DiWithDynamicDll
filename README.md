# DiWithDynamicDll
POC project to find ways of using dependency injection with dynamically loaded dlls.

To understand the basics of dependency injection implementation, I suggest reading this fantastic tutorial from Ed Mays: https://www.codementor.io/copperstarconsulting/getting-started-with-dependency-injection-using-castle-windsor-4meqzcsvh

*Problem*
When using a dynamically loaded assembly, some of the types (the ones in the dynamically loaded assembly) are not available immediately, thus they can't be used in startup dependency container resolutions.

*Concept*
1.) Implement basic dependency injection (DI, see Ed's article above). The important bits are:
 - The DI container, which contains type resolution information for all types in tha application.
 - CompositionRoot - we'll implement a similar type called CompositionSubRoot in the dynamic dll.
2.) Implement the dynamic dll with a class that implements a new interface called ICompositionSubroot
3.) Load the dyanmic dll.
4.) Find the type that implements the ICompositionSubRoot interface (called CompsitionSubRoot) and instantiate it.
5.) Use compositionSubRoot to add all the DI registrations implemented in the dynamic dll.
6.) After all that is done, when all DI types are rgistered, instantiate CompositionRoot.

There may be a simpler solution, the example is there to illustrate the point.

*Sample implementation steps*
Create solution
Create a command line project called app
Create a dll project called Animals
 - add a class called AnumalInfo implementing the ISpeciesInfo interface
 Create a dll project called Plants
 - add a class called PlantInfo implementing the ISpeciesInfo interface
Create a dll project called PublicContract
 - add an interface called ISpeciesInfo
 - in the App, Animals and Plants projects, add a reference to the Contract project
Add DI framework
 - use Nuget manager to add Castle.Windsor to all projects (including the Contract project)
   (if targeting .Net Framework 4.0, use CW version 3.4.0)
Add a class called CompositionSubRoot to the Animals project, that will add type resolution registrations implemented in thes assembly to the DI container:
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Contract;

    namespace Plants
    {
        public class CompositionSubRoot : ICompositionSubRoot
        {
            public void ExtendDiCompositionRegistration(WindsorContainer container)
            {
                container.Register(Component.For<ISpeciesInfo>().ImplementedBy<AnimalInfo>());
            }
        }
    }
Do the same for the Plants project, using ....ImplementedBy<PlantInfo>() in the registration line.
In the application startup code, impleement the dynamid assembly load, find the class that implements the ICompositionSubRoot interface, instantiate it and call the published subroot registration method. Once the DI types are registered, go on to start up the application. This way the implementation of the type registration doesn't need to be known to the caller, just the interface. Program.cs now looks like this:
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Contract;
    using System;
    using System.Reflection;

    namespace App
    {
        class Program
        {
            static void Main(string[] args)
            {
                var container = new WindsorContainer();

                // Register the CompositionRoot type with the container
                container.Register(Component.For<ICompositionRoot>().ImplementedBy<CompositionRoot>());

                // Dynamic load one of the assemblies and use it to extend DI container registrations.
                // You can change this line to load from @"..\..\..\Animals\bin\Debug\Animals.dll"
                Assembly dynamicAssembly = Assembly.LoadFrom(@"..\..\..\Plants\bin\Debug\Plants.dll");

                Type compositionSubRootType =
                    AssemblyHelper.FindTypeThatImplementsInterface (dynamicAssembly, typeof(ICompositionSubRoot));
                ICompositionSubRoot subRoot = 
                    dynamicAssembly.CreateInstance(compositionSubRootType.FullName) as ICompositionSubRoot;
                subRoot.ExtendDiCompositionRegistration(container);

                // Extend DI component registrations make sure that it all happens
                // before resolving ICompositionRoot
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
FindTypeThatImplementsInterface() is a method that searches an assembly for a class that implements an interface, assuming there is only one such class:
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
