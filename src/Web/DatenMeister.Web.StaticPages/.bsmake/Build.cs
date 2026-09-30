// Merge.ts
using BurnSystems.Make.BuildAgent;

await CopyCSS();
await CompileTS();
await CompressJS();

return 0;

// Compile.ts

async Task CopyCSS()
{
    Console.WriteLine("Copying CSS");
    
    System.IO.File.Copy(
        "../DatenMeister.WebServer/wwwroot/css/datenmeister-web.min.css", 
        "Assets/Css/datenmeister-web.min.css",
        true);       
}

async Task CompileTS()
{
    Console.WriteLine("Compiling Typescript");

    await ProcessInvoke.Run("npx", ["tsc"]);
}


async Task CompressJS()
{
    Console.WriteLine("Minifying TypeScript files");

    await ProcessInvoke.Run("npx",
        new[]
        {
            "esbuild",
            "Assets/Js/index.js",
            "--minify",
            "--tree-shaking=true",
            "--bundle",
            "--sourcemap",
            "--outfile=Assets/Js-Bundled/index.js",
            "--platform=browser",
            "--format=esm"
        });
}