// Merge.ts
using BurnSystems.Make.BuildAgent;

await CompileTs();
await CompileJs();

return 0;

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
        "esbuild", 
        "Assets/Js/DatenMeister.Reports.Forms.js",
        "Assets/Js/DatenMeister.Reports.Types.js",
        "--minify", 
        "--sourcemap", 
        "--outdir=Js",
        "--platform=browser",
        "--format=esm"
    ]);
}
