// Merge.ts
using BurnSystems.Make.BuildAgent;

await CopyModel();
await CompileTs();
await CompileJs();

return 0;

async Task CopyModel()
{
    Console.WriteLine("Copying model");
    File.Copy("Model/Types.ts", "Assets/Js/Types.ts", true);
}

async Task CompileTs()
{
    Console.WriteLine("Compiling Typescript");

    await ProcessInvoke.Run("npx", ["tsc"]);
}

async Task CompileJs()
{
    Console.WriteLine("Compiling Javascript");

    await ProcessInvoke.Run("npx",
    [
        "esbuild", "Assets/Js/*.js", "--minify", "--sourcemap", "--outdir=Js", "--platform=browser", "--format=esm"
    ]);
}