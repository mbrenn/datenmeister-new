﻿using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Excel.Models;
using DatenMeister.Integration.DotNet;
using DatenMeister.Provider.XMI;
using DatenMeister.SourcecodeGenerator;
using DatenMeister.SourcecodeGenerator.SourceParser;
using DatenMeister.Types;

namespace DatenMeister.SourceGeneration.Console
{
    class Program
    {
        public const string R = "../../..";

        public static void Main(string[] args)
        {
            // First, creates
            CreateSourceForUmlAndMof();

            CreateSourceForExcel();

            // CreateSourceForDataViews();
            
            //CreateSourceCodeForDatenMeister();

            CreateSourceCodeForDatenMeisterAllTypes();
            
            System.Console.WriteLine("Closing Source Code Generator");

#if !DEBUG
            File.Copy($"{R}/primitivetypes.cs", $"{R}/../DatenMeister.Core/Models/EMOF/primitivetypes.cs", true);
            File.Copy($"{R}/mof.cs", $"{R}/../DatenMeister.Core/Models/EMOF/mof.cs", true);
            File.Copy($"{R}/uml.cs", $"{R}/../DatenMeister.Core/Models/EMOF/uml.cs", true);
            
            File.Copy($"./ExcelModels.class.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
            File.Copy($"./DatenMeister.class.cs", $"{R}/../DatenMeister.Core/Models/DatenMeister.class.cs", true);
#endif
        }

        private static void CreateSourceCodeForDatenMeisterAllTypes()
        {
            var dm = GiveMe.DatenMeister();
            
            System.Console.Write("Create Sourcecode for DatenMeister...");

            var pseudoExtent = new LocalTypeSupport(dm.WorkspaceLogic, dm.ScopeStorage).InternalTypes;

            ////////////////////////////////////////
            // Creates the class tree

            // Creates the source parser which is needed to navigate through the package
            var sourceParser = new ElementSourceParser();
            var classTreeGenerator = new ClassTreeGenerator(sourceParser)
            {
                Namespace = "DatenMeister.Core.Models"
            };

            classTreeGenerator.Walk(pseudoExtent);

            var pathOfClassTree = "DatenMeister.class.cs";
            var fileContent = classTreeGenerator.Result.ToString();
            File.WriteAllText(pathOfClassTree, fileContent);
            System.Console.WriteLine(" Done");
        }

        private static void CreateSourceForExcel()
        {
            System.Console.Write("Create Sourcecode for Excel...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = WorkspaceNames.UriExtentInternalTypes,
                    Name = "ExcelModels",
                    Path = "./",
                    Namespace = "DatenMeister.Excel.Models",
                    Types = ExcelModels.AllTypes
                });
            System.Console.WriteLine(" Done");
        }

        private static void CreateSourceForUmlAndMof()
        {
            var umlExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentUml);
            var mofExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentMof);
            var primitiveTypeExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentPrimitiveTypes);

            var loader = new SimpleLoader();
            loader.LoadFromFile(new MofFactory(umlExtent), umlExtent, "data/UML.xmi");
            loader.LoadFromFile(new MofFactory(mofExtent), mofExtent, "data/MOF.xmi");
            loader.LoadFromFile(new MofFactory(primitiveTypeExtent), primitiveTypeExtent, "data/PrimitiveTypes.xmi");

            // Generates tree for UML
            var generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Core.Models.EMOF"
            };

            generator.Walk(umlExtent);

            /*var extentCreator = new FillClassTreeByExtentCreator("DatenMeister.Models.EMOF._UML")
            {
                Namespace = "DatenMeister.Models.EMOF"
            };
            extentCreator.Walk(umlExtent);*/

            File.WriteAllText($"{R}/uml.cs", generator.Result.ToString());
            // File.WriteAllText($"{R}/FillTheUML.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for UML written");

            // Generates tree for MOF
            generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Core.Models.EMOF"
            };
            generator.Walk(mofExtent);

            /*7extentCreator = new FillClassTreeByExtentCreator("DatenMeister.Models.EMOF._MOF")
            {
                Namespace = "DatenMeister.Models.EMOF"
            };

            extentCreator.Walk(mofExtent);*/

            File.WriteAllText($"{R}/mof.cs", generator.Result.ToString());
            // File.WriteAllText($"{R}/FillTheMOF.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for MOF written");

            // Generates tree for PrimitiveTypes
            generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Core.Models.EMOF"
            };
            generator.Walk(primitiveTypeExtent);

            /*extentCreator = new FillClassTreeByExtentCreator("DatenMeister.Models.EMOF._PrimitiveTypes")
            {
                Namespace = "DatenMeister.Models.EMOF"
            };

            extentCreator.Walk(primitiveTypeExtent);*/

            File.WriteAllText($"{R}/primitivetypes.cs", generator.Result.ToString());
            // File.WriteAllText($"{R}/FillThePrimitiveTypes.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for PrimitiveTypes written");
        }
    }
}

