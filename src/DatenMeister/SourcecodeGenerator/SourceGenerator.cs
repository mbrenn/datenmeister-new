using System.Collections.Generic;
using System.IO;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.SourcecodeGenerator.SourceParser;

namespace DatenMeister.SourcecodeGenerator
{
    public static class SourceGenerator
    {
        public static void GenerateSourceFor(SourceGeneratorOptions options, _UML uml = null)
        {
            uml = uml ?? new _UML(); // Verifies that a uml is existing

            var extent = new UriExtent(new InMemoryProvider(), "dm:///");
            var factory = (IFactory) null; // new InMemoryFactory();

            // Creates the package
            var package = factory.create(uml.Packages.__Package);
            package.set("name", options.Name);

            // Do the conversion from dotnet types to real MOF Types
            var dotNetProvider = new DotNetTypeGenerator(factory, uml);
            var elements = new List<IElement>();
            foreach (var type in options.Types)
            {
                var typeObject = dotNetProvider.CreateTypeFor(type);
                elements.Add(typeObject);
            }

            // And adds the converted elements to package and the package to the temporary MOF Extent
            package.set(_UML._Packages._Package.packagedElement, elements);
            extent.elements().add(package);

            // Creates the source parser which is needed to navigate through the package
            var sourceParser = new ElementSourceParser(uml);
            
            ////////////////////////////////////////
            // Creates the class tree
            var classTreeGenerator = new ClassTreeGenerator(sourceParser)
            {
                Namespace = options.Namespace
            };

            classTreeGenerator.Walk(extent);

            var pathOfClassTree = GetPath(options, ".class.cs");
            File.WriteAllText(pathOfClassTree, classTreeGenerator.Result.ToString());

            ////////////////////////////////////////
            // Creates now the filler
            var fillerGenerator = new FillClassTreeByExtentCreator(options.Name + "Filler", sourceParser)
            {
                Namespace = options.Namespace,
                ClassNameOfTree = classTreeGenerator.UsedClassName
            };

            fillerGenerator.Walk(extent);

            var pathOfFillerTree = GetPath(options, ".filler.cs");
            File.WriteAllText(pathOfFillerTree, fillerGenerator.Result.ToString());

            ////////////////////////////////////////
            // Creates the Dot Net Integration Parser
            var dotNetGenerator = new DotNetIntegrationGenerator();
            dotNetGenerator.Create(options.Namespace, options.Name, options.Types);

            var pathOfDotNetIngegration = GetPath(options, ".dotnet.cs");
            File.WriteAllText(pathOfDotNetIngegration, dotNetGenerator.Result.ToString());
        }

        private static string GetPath(SourceGeneratorOptions options, string extension)
        {
            var pathOfDotNetIngegration =
                Path.ChangeExtension(
                    Path.Combine(
                        options.Path,
                        options.Name),
                    extension);
            return pathOfDotNetIngegration;
        }
    }
}