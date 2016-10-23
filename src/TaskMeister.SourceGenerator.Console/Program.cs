using System.IO;
using DatenMeister.SourcecodeGenerator;

namespace TaskMeister.SourceGenerator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var packagetypes = GlobalDatenMeister.GetModelTypes();

            DatenMeister.SourcecodeGenerator.SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    Name = "TaskMeisterModel",
                    Path = "./",
                    Namespace = "TaskMeister.Model",
                    Types = packagetypes
                });

#if !DEBUG
            File.Copy("TaskMeisterModel.filler.cs", "../../../TaskMeisterLib/Model/TaskMeisterModel.filler.cs", true);
            File.Copy("TaskMeisterModel.class.cs", "../../../TaskMeisterLib/Model/TaskMeisterModel.class.cs", true);
            File.Copy("TaskMeisterModel.dotnet.cs", "../../../TaskMeisterLib/Model/TaskMeisterModel.dotnet.cs", true);
            // File.Copy("TaskMeisterModel.xmi", "../../../TaskMeisterLib/Model/TaskMeisterModel.xmi", true);
#endif
        }
    }
}
