// Merge.ts
using BurnSystems.Make.BuildAgent;

var make = new BsMake(args);

await make.Verb(Verbs.Clean, async () =>
{
    await NpmHelper.DeleteNodeModules();
});

await make.Verb(Verbs.Build, async () =>
{
    await NpmHelper.InstallIfNotInstalled();

    await CompileTs();
    await CompileJs();
});

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
        "Assets/Js/*.js",
        "--minify", 
        "--sourcemap", 
        "--outdir=Js",
        "--platform=browser",
        "--format=esm"
    ]);
}
