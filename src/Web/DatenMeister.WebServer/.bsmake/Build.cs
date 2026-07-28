// Merge.ts
using BurnSystems.Make.BuildAgent;

CopyNodeModules();
await CompressCSS();
await CompileTS();
await CompressJS();
MoveJS();

return 0;

void CopyNodeModules()
{
    var target = ("./wwwroot/js/node_modules");

    var files = Directory.GetFiles("./node_modules/", "*", SearchOption.AllDirectories)
        .Where(x =>
        {
            var extension = Path.GetExtension(x);
            return extension == ".js" || extension == ".css" || extension == ".d.ts";
        }).ToList();

    foreach (var file in files)
    {
        var positionNodeModules = file.IndexOf("node_modules/", StringComparison.InvariantCulture); 
        if(positionNodeModules == -1 )
        {
            Console.WriteLine("Some obscure error occured");
        }
        
        var relativePath = file.Substring(positionNodeModules + "node_modules/".Length);
        var targetDir = Path.GetDirectoryName(Path.Combine(target, relativePath)) 
            ?? throw new InvalidOperationException("Should not happen");
        
        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }
        
        File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);
    }
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

async Task CompressJS()
{
    Console.WriteLine("Minifying TypeScript files");

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
}

void MoveJS()
{
    Console.WriteLine("Copying burnJsPopup Files to wwwroot");

    CopyFiles("node_modules/@mbrenn/burnjspopup/dist/js/", "wwwroot/js/burnsystems/");
    CopyFiles("node_modules/@mbrenn/burnjspopup/dist/css/", "wwwroot/css");

    Console.WriteLine("Copying JQuery FancyTree");

    CopyFiles("node_modules/jquery.fancytree/dist/", "wwwroot/js", searchPattern: "*.min.js");
    CopyFiles("node_modules/jquery.fancytree/dist/skin-win8/", "wwwroot/css/jquery.fancytree/css", searchPattern: "*.css");
    
}

void CopyFiles(string sourceDirectory, string targetDirectory, bool overwrite = true, string searchPattern = "*.*")
{
    foreach (var sourceFile in Directory.GetFiles(sourceDirectory, searchPattern))
    {
        var filename = Path.GetFileName(sourceFile);
        var targetFile = Path.Combine(targetDirectory, filename);
        File.Copy(sourceFile, targetFile, overwrite);
    }
}