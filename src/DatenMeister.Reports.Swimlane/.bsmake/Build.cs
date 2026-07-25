// Merge.ts

using System.Diagnostics;

MergeTs();
await CompileTs();
await CompileJs();
MoveTs();

void MergeTs()
{
    File.Delete("Assets/Js/Types.ts");
    File.Copy("Model/Types.ts", "Assets/Js/Types.ts");

    var swimlaneSourceFile = "Assets/Js/DatenMeister.Reports.Swimlane.ts";
    var typesSourceFile = "Assets/Js/Types.ts";
    var mergedOutputFile = "Assets/Js/DatenMeister.Reports.Swimlane.Combine.ts";

    var outputExists = File.Exists(mergedOutputFile);
    if (outputExists)
    {
        var outputTime = File.GetLastWriteTimeUtc(mergedOutputFile);
        var swimlaneTime = File.GetLastWriteTimeUtc(swimlaneSourceFile);
        var typesTime = File.GetLastWriteTimeUtc(typesSourceFile);

        if (swimlaneTime <= outputTime && typesTime <= outputTime)
        {
            Console.WriteLine("Skipping Merge TS: {0} is up-to-date.", mergedOutputFile);
            return;
        }
    }

    var swimlane = File.ReadAllText(swimlaneSourceFile);
    var types = File.ReadAllText(typesSourceFile);
    var result = "// This file is generated in build.cake. Do NOT modify that file.\n";
    result += "// Modify DatenMeister.Reports.Swimlane.Source.ts\n";
    result += "// noinspection DuplicatedCode\n\n";

    result += swimlane + "\n\n" + types;
    File.WriteAllText(mergedOutputFile, result);
}

// Compile.ts

async Task CompileTs()
{
    Console.WriteLine("Compiling Typescript");

    await Process.Start("npx", ["tsc"]).WaitForExitAsync();
}


// Asset

async Task CompileJs()
{
    Console.WriteLine("Compiling Javascript");

    await Process.Start("npx",
    [
        "esbuild", "Assets/Js/*.js", "--minify", "--sourcemap", "--outdir=Js", "--platform=browser", "--format=esm"
    ]).WaitForExitAsync();
}

void MoveTs()
{
    Console.WriteLine("Moving Javascript files");

    File.Delete("Assets/Js/DatenMeister.Reports.Swimlane.Combine.ts");
    File.Delete("Assets/Js/DatenMeister.Reports.Swimlane.Combine.js");
    File.Delete("Assets/Js/DatenMeister.Reports.Swimlane.Combine.js.map");
    File.Delete("Js/DatenMeister.Reports.Swimlane.js");
    File.Delete("Js/DatenMeister.Reports.Swimlane.js.map");
    File.Move("Js/DatenMeister.Reports.Swimlane.Combine.js", "Js/DatenMeister.Reports.Swimlane.js");
    File.Move("Js/DatenMeister.Reports.Swimlane.Combine.js.map", "Js/DatenMeister.Reports.Swimlane.js.map");
}