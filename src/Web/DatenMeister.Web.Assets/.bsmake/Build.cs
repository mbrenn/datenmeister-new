// Merge.ts
using BurnSystems.Make.BuildAgent;

var programArguments = ProgramArguments.Parse(args);

await CompileSass();
await CompressCSS();
await CompileTS();
await CompressJS(programArguments);

return 0;

async Task CompileSass()
{
    Console.WriteLine("Compiling Sass/SCSS files");

    await ProcessInvoke.Run("npx", [
        "--package=sass-embedded",        
        "sass",
        "Assets/css:Assets/css",
        "--no-source-map"
    ]);
}

async Task CompressCSS()
{
    Console.WriteLine("Compressing CSS Files");

    var cssFiles = System.IO.Directory.GetFiles("Assets/css/", "*.css")
        .Where(x => x.StartsWith("Assets/css/datenmeister.")).ToList();
    cssFiles.Add("Assets/css/burnJsPopup.css");

    var outputFile = "wwwroot/css/datenmeister-web.min.css";

    var args = new List<string>();
    args.Add("cleancss");
    args.Add("--format");
    args.Add("keep-breaks");
    args.Add("--source-map");
    args.Add("-o");
    args.Add(outputFile);
    args.AddRange(cssFiles);

    await ProcessInvoke.Run("npx", args.ToArray());
}

async Task CompileTS()
{
    Console.WriteLine("Compiling Typescript");

    await ProcessInvoke.Run("npx", ["tsc"]);
}

async Task CompressJS(ProgramArguments arguments)
{
    if (arguments.Configuration == "Debug")
    {
        Console.WriteLine("Minifying TypeScript files - Debug");

        Console.Write("Version esbuild: ");

        await ProcessInvoke.Run("npx",
            new[]
            {
                "esbuild",
                "--version"
            });

        Console.Write("Version tsc: ");

        await ProcessInvoke.Run("tsc",
            new[]
            {
                "-v"
            });


        await ProcessInvoke.Run("npx",
            new[]
            {
                "esbuild",
                "Assets/js/burnsystems/**/*.js",
                "--sourcemap",
                "--outbase=Assets/js/burnsystems",
                "--outdir=wwwroot/js/burnsystems",
                "--platform=browser",
                "--format=esm"
            });
        await ProcessInvoke.Run("npx",
            new[]
            {
                "esbuild",
                "Assets/js/datenmeister/**/*.js",
                "--sourcemap",
                "--outbase=Assets/js/datenmeister",
                "--outdir=wwwroot/js/datenmeister",
                "--platform=browser",
                "--format=esm"
            });
        await ProcessInvoke.Run("npx",
            new[]
            {
                "esbuild",
                "Assets/JsTests/**/*.js",
                "--sourcemap",
                "--outbase=Assets/JsTests",
                "--outdir=wwwroot/js/JsTests",
                "--platform=browser",
                "--format=esm"
            });
    }
    else
    {
        Console.WriteLine("Minifying TypeScript files - Release");

        await ProcessInvoke.Run("npx",
            new[]
            {
                "esbuild",
                "Assets/js/burnsystems/**/*.js",
                "--minify",
                "--sourcemap",
                "--outbase=Assets/js/burnsystems",
                "--outdir=wwwroot/js/burnsystems",
                "--platform=browser",
                "--format=esm"
            });
        await ProcessInvoke.Run("npx",
            new[]
            {
                "esbuild",
                "Assets/js/datenmeister/**/*.js",
                "--minify",
                "--sourcemap",
                "--outbase=Assets/js/datenmeister",
                "--outdir=wwwroot/js/datenmeister",
                "--platform=browser",
                "--format=esm"
            });
        await ProcessInvoke.Run("npx",
            new[]
            {
                "esbuild",
                "Assets/JsTests/**/*.js",
                "--minify",
                "--sourcemap",
                "--outbase=Assets/JsTests",
                "--outdir=wwwroot/js/JsTests",
                "--platform=browser",
                "--format=esm"
            });
    }
}