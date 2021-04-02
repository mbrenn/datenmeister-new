using System;
using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.DotNet;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Runtime;
using DatenMeister.SourcecodeGenerator.SourceParser;

namespace DatenMeister.SourcecodeGenerator
{
    public static class SourceGenerator
    {
        public static void GenerateSourceFor(SourceGeneratorOptions options)
        {
            ////////////////////////////////////////
            // First of all, create all Mof types, representing the objects under concern
            var extent = new MofUriExtent(new InMemoryProvider(), options.ExtentUrl);
            var factory = new MofFactory(extent);

            // Creates the package
            var package = factory.create(_UML.TheOne.Packages.__Package);
            package.set("name", options.Name);
            extent.elements().add(package);

            // Do the conversion from dotnet types to real MOF Types
            var dotNetProvider = new DotNetTypeGenerator(factory, extent);
            foreach (var type in options.Types)
            {
                var typeObject = dotNetProvider.CreateTypeFor(
                    type,
                    new DotNetTypeGeneratorOptions
                    {
                        IntegrateInheritedProperties = false
                    });

                if (typeObject != null)
                {
                    // And adds the converted elements to package and the package to the temporary MOF Extent
                    PackageMethods.AddObjectToPackage(package, typeObject);
                    extent.TypeLookup.Add(
                        typeObject.GetUri() ?? throw new NotImplementedException("GetUri() == null"),
                        type);
                }
            }

            // Creates the source parser which is needed to navigate through the package
            var sourceParser = new ElementSourceParser();

            ////////////////////////////////////////
            // Creates the class tree
            var classTreeGenerator = new ClassTreeGenerator(sourceParser)
            {
                Namespace = options.Namespace
            };

            classTreeGenerator.Walk(extent);

            var pathOfClassTree = GetPath(options, ".class.cs");
            var fileContent = classTreeGenerator.Result.ToString();
            File.WriteAllText(pathOfClassTree, fileContent);


            /*
             * No filler
             *
            ////////////////////////////////////////
            // Creates now the filler
            var fillerGenerator = new FillClassTreeByExtentCreator(options.Name + "Filler", sourceParser)
            {
                Namespace = options.Namespace,
                ClassNameOfTree = classTreeGenerator.UsedClassName ?? string.Empty
            };

            fillerGenerator.Walk(extent);

            var pathOfFillerTree = GetPath(options, ".filler.cs");
            fileContent = fillerGenerator.Result.ToString();
            File.WriteAllText(pathOfFillerTree, fileContent);
            */

            ////////////////////////////////////////
            // Creates the Dot Net Integration Parser
            var dotNetGenerator = new DotNetIntegrationGenerator();
            dotNetGenerator.Create(
                options.Namespace ?? string.Empty,
                options.Name ?? string.Empty,
                options.Types);

            var pathOfDotNetIntegration = GetPath(options, ".dotnet.cs");
            File.WriteAllText(pathOfDotNetIntegration, dotNetGenerator.Result.ToString());
        }

        /// <summary>
        /// Gets the path by adding the extension to the suggested filename
        /// </summary>
        /// <param name="options">Options for Source code generator</param>
        /// <param name="extension">File extension to be set</param>
        /// <returns>Path to be used</returns>
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