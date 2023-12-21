using System.IO;
using CommandLine;
using CommandLine.Text;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Integration.DotNet;
using DatenMeister.SourcecodeGenerator;

namespace DatenMeister.SourceGeneration.Console
{
    class Program
    {
        public static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                PerformStandardProcedure();
            }
            else
            {
                var value = CommandLine.Parser.Default.ParseArguments<CommandOptions>(args);
                value.WithParsed(x => CreateCodeForTypes(x.PathXml, x.PathTarget, x.Namespace));
                value.WithNotParsed(x => System.Console.WriteLine(HelpText.AutoBuild(value, h => h)));
            }
        }

        /// <summary>
        /// Performs the standard procedure
        /// </summary>
        private static void PerformStandardProcedure()
        {
            System.Console.WriteLine("Perform the standard procedure.");

            // First, creates
            StandardProcedure.CreateSourceForUmlAndMof();

            StandardProcedure.CreateTypescriptForUmlAndMof();

            StandardProcedure.CreateSourceForExcel();

            StandardProcedure.CreateSourceCodeForDatenMeisterAllTypes();

            StandardProcedure.CreateTypescriptForDatenMeisterAllTypes();
            
            System.Console.WriteLine("Closing Source Code Generator");

#if !DEBUG

            var R = StandardProcedure.R;
            var T = StandardProcedure.T;

            File.Copy($"{T}/primitivetypes.cs", $"{R}/../DatenMeister.Core/Models/EMOF/primitivetypes.cs", true);
                File.Copy($"{T}/mof.cs", $"{R}/../DatenMeister.Core/Models/EMOF/mof.cs", true);
                File.Copy($"{T}/uml.cs", $"{R}/../DatenMeister.Core/Models/EMOF/uml.cs", true);
                File.Copy($"{T}/primitivetypes.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/primitivetypes.ts", true);
                File.Copy($"{T}/mof.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/mof.ts", true);
                File.Copy($"{T}/uml.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/uml.ts", true);
                
                File.Copy($"./ExcelModels.class.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
                File.Copy($"./DatenMeister.class.cs", $"{R}/../DatenMeister.Core/Models/DatenMeister.class.cs", true);
                File.Copy($"./DatenMeister.class.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/DatenMeister.class.ts", true);
                File.Copy($"./ExcelModels.class.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/ExcelModels.class.ts", true);

                File.Delete($"{T}/primitivetypes.cs");
                File.Delete($"{T}/primitivetypes.ts");
                File.Delete($"{T}/primitivetypes.js");
                File.Delete($"{T}/mof.cs");
                File.Delete($"{T}/mof.ts");
                File.Delete($"{T}/mof.js");
                File.Delete($"{T}/uml.cs");
                File.Delete($"{T}/uml.ts");
                File.Delete($"{T}/uml.js");
#endif
        }


        public static void CreateCodeForTypes(string pathXml, string pathTarget, string theNamespace)
        {
            System.Console.WriteLine("Reading from: " + pathXml);
            System.Console.WriteLine("Writing to  : " + pathTarget);
            System.Console.WriteLine();

            using var dm = GiveMe.DatenMeister();
            var filename = Path.GetFileNameWithoutExtension(pathXml);

            var typeExtent = new MofUriExtent(
                XmiProvider.CreateByFile(pathXml),
                "dm:///types/", null);

            dm.WorkspaceLogic.AddExtent(dm.WorkspaceLogic.GetDataWorkspace(), typeExtent);

            // Generates tree for Type Script
            var generator = new TypeScriptInterfaceGenerator();
            generator.Walk(typeExtent);

            File.WriteAllText(Path.Combine(pathTarget, $"{filename}.ts"), generator.Result.ToString());
            System.Console.WriteLine("TypeScript Code for StundenPlan written");

            // Generates tree for StundenPlan
            var classGenerator = new ClassTreeGenerator
            {
                Namespace = theNamespace
            };

            classGenerator.Walk(typeExtent);

            var pathOfClassTree = Path.Combine(pathTarget, $"{filename}.cs");
            var fileContent = classGenerator.Result.ToString();
            File.WriteAllText(pathOfClassTree, fileContent);

            System.Console.WriteLine("C#-Code for StundenPlan written");
        }
    }
}