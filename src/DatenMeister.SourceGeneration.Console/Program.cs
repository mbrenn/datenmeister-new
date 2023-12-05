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

            // CreateSourceForDataViews();

            //CreateSourceCodeForDatenMeister();

            StandardProcedure.CreateSourceCodeForDatenMeisterAllTypes();

            StandardProcedure.CreateTypescriptForDatenMeisterAllTypes();

            StandardProcedure.CreateCodeForStundenPlan();

            System.Console.WriteLine("Closing Source Code Generator");

            var R = StandardProcedure.R;

#if !DEBUG
                File.Copy($"{R}/primitivetypes.cs", $"{R}/../DatenMeister.Core/Models/EMOF/primitivetypes.cs", true);
                File.Copy($"{R}/mof.cs", $"{R}/../DatenMeister.Core/Models/EMOF/mof.cs", true);
                File.Copy($"{R}/uml.cs", $"{R}/../DatenMeister.Core/Models/EMOF/uml.cs", true);
                File.Copy($"{R}/primitivetypes.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/primitivetypes.ts", true);
                File.Copy($"{R}/mof.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/mof.ts", true);
                File.Copy($"{R}/uml.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/uml.ts", true);
                
                File.Copy($"./ExcelModels.class.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
                File.Copy($"./DatenMeister.class.cs", $"{R}/../DatenMeister.Core/Models/DatenMeister.class.cs", true);
                File.Copy($"./DatenMeister.class.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/DatenMeister.class.ts", true);
                File.Copy($"./ExcelModels.class.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/ExcelModels.class.ts", true);

                File.Delete($"{R}/primitivetypes.cs");
                File.Delete($"{R}/primitivetypes.ts");
                File.Delete($"{R}/primitivetypes.js");
                File.Delete($"{R}/mof.cs");
                File.Delete($"{R}/mof.ts");
                File.Delete($"{R}/mof.js");
                File.Delete($"{R}/uml.cs");
                File.Delete($"{R}/uml.ts");
                File.Delete($"{R}/uml.js");
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