using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Excel.Models;
using DatenMeister.SourcecodeGenerator;

namespace DatenMeister.SourceGeneration.Console;

public static class StandardProcedure
{
    /// <summary>
    /// Defines the target path into which the files shall be stored in case
    /// of a Release Build. This is done by copying the files from t
    /// </summary>
    public const string R = "../../..";

    /// <summary>
    /// Defines the target path into which the files will be stored temporarily.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public const string T = "./";

    public static void CreateSourceForExcel()
    {
        System.Console.Write("Create Sourcecode for Excel...");
        
        SourceGenerator.GenerateSourceFor(
            new SourceGeneratorOptions
            {
                ExtentUrl = WorkspaceNames.UriExtentInternalTypes,
                Name = "ExcelModels",
                Path = "./",
                Namespace = "DatenMeister.Excel.Models",
                Types = ExcelModelInfo.AllTypes
            });
        
        System.Console.WriteLine(" Done");
    }
}

