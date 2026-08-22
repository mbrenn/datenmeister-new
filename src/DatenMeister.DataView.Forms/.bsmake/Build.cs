// Merge.ts
using BurnSystems.Make.BuildAgent;

await CopyModel();
// await MergeJs(); /* We need to find a better solution here */
await CompileTs();
await CompileJs();

return 0;

// Compile.ts

async Task CopyModel()
{
    Console.WriteLine("Copying model");
    File.Copy("Model/Types.ts", "Assets/Js/Types.ts", true);
}

async Task MergeJs()
{
    Console.WriteLine("Merge model");
    var mergeOptions = new FileMergeOptions()
    {
        FirstFile = "Assets/Js/DatenMeister.ViewNode.Forms.ts",
        SecondFile = "Assets/Js/Types.ts",
        TargetFile = "Assets/Js/Packed.ts"
    };
    
    FileHelper.MergeFiles(mergeOptions);
}

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
        "esbuild", 
        "Assets/Js/*.js",
        "--minify", 
        "--sourcemap", 
        "--outdir=Js",
        "--platform=browser",
        "--format=esm"
    ]);
}
