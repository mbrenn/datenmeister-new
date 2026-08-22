// Merge.ts
using BurnSystems.Make.BuildAgent;

if (MergeTs())
{
    await CompileTs();
    await CompileJs();
    MoveTs();
}

return 0;

/// Returns true, in case the merge was done
bool MergeTs()
{
    File.Delete("Assets/Js/Types.ts");
    File.Copy("Model/Types.ts", "Assets/Js/Types.ts");

    var result = "// This file is generated in build.cake. Do NOT modify that file.\n";
    result += "// Modify DatenMeister.Reports.Swimlane.Source.ts\n";
    result += "// noinspection DuplicatedCode\n\n";

    var mergeOptions = new FileMergeOptions()
    {
        FirstFile = "Assets/Js/DatenMeister.Reports.Swimlane.ts",
        SecondFile = "Assets/Js/Types.ts",
        TargetFile = "Assets/Js/DatenMeister.Reports.Swimlane.Combine.ts",
        PrefixTextTarget = result
    };

    if (!FileHelper.MergeFiles(mergeOptions))
    {
        return false;
    }

    Console.WriteLine($"{mergeOptions.TargetFile} was created: {File.Exists(mergeOptions.TargetFile)}");
    return true;
}

// Compile.ts

async Task CompileTs()
{
    Console.WriteLine("Compiling Typescript");

    await ProcessInvoke.Run("npx", ["tsc"]);
}

// Asset
async Task CompileJs()
{
    Console.WriteLine("Compiling Javascript");

    await ProcessInvoke.Run("npx",
    [
        "esbuild", "Assets/Js/*.js", "--minify", "--sourcemap", "--outdir=Js", "--platform=browser", "--format=esm"
    ]);
}

void MoveTs()
{
    Console.WriteLine("Moving Javascript files");

    File.Delete("Js/DatenMeister.Reports.Swimlane.js");
    File.Delete("Js/DatenMeister.Reports.Swimlane.js.map");
    File.Move("Js/DatenMeister.Reports.Swimlane.Combine.js", "Js/DatenMeister.Reports.Swimlane.js");
    File.Move("Js/DatenMeister.Reports.Swimlane.Combine.js.map", "Js/DatenMeister.Reports.Swimlane.js.map");
}