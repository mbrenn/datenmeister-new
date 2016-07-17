using System;
using System.Collections.Generic;
using System.IO;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.SourcecodeGenerator.SourceParser;

namespace DatenMeister.SourcecodeGenerator
{
    public static class SourceGenerator
    {
        public static void GenerateSourceFor(SourceGeneratorOptions options)
        {
            var uml = new _UML();
            var extent = new MofUriExtent("dm:///sourcegenerator");
            var factory = new MofFactory();

            // Creates the dotnet types to real MOF Types
            var dotNetProvider = new DotNetTypeGenerator(factory, uml);
            var package = factory.create(uml.Packages.__Package);
            package.set("name", options.Name);

            // Do the conversion 
            var elements = new List<IElement>();
            foreach (var type in options.Types)
            {
                var typeObject = dotNetProvider.CreateTypeFor(type);
                elements.Add(typeObject);
            }

            package.set(_UML._Packages._Package.packagedElement, elements);

            // Adds the package
            extent.elements().add(package);

            var sourceParser = new ElementSourceParser(uml);
            
            // Creates the class tree
            var classTreeGenerator = new ClassTreeGenerator(sourceParser)
            {
                Namespace = options.Namespace
            };

            classTreeGenerator.Walk(extent);
            var sourceClass = classTreeGenerator.Result.ToString();

            var pathOfClassTree =
                Path.ChangeExtension(
                    Path.Combine(
                        options.Path,
                        options.Name),
                    ".class.cs");

            File.WriteAllText(pathOfClassTree, sourceClass);

            // Creates now the filler
            var fillerGenerator = new FillClassTreeByExtentCreator(options.Name + "Filler", sourceParser)
            {
                Namespace = options.Namespace,
                ClassNameOfTree = classTreeGenerator.UsedClassName
            };

            fillerGenerator.Walk(extent);
            var sourceFiller = fillerGenerator.Result.ToString();
            
            var pathOfFillerTree =
                Path.ChangeExtension(
                    Path.Combine(
                        options.Path,
                        options.Name),
                    ".filler.cs");

            File.WriteAllText(pathOfFillerTree, sourceFiller);
        }
    }
}