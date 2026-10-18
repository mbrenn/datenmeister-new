// Merge.ts
using BurnSystems.Make.BuildAgent;

CopyNodeModules();
MoveJS();
CopyAssets();

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

void MoveJS()
{
    Console.WriteLine("Copying burnJsPopup Files to wwwroot");

    CopyFiles("node_modules/@mbrenn/burnjspopup/dist/js/", "wwwroot/js/burnsystems/");
    CopyFiles("node_modules/@mbrenn/burnjspopup/dist/css/", "wwwroot/css");

    Console.WriteLine("Copying JQuery FancyTree");

    CopyFiles("node_modules/jquery.fancytree/dist/", "wwwroot/js", searchPattern: "*.min.js");
    CopyFiles("node_modules/jquery.fancytree/dist/skin-win8/", "wwwroot/css/jquery.fancytree/css", searchPattern: "*.css");
}

void CopyFiles(string sourceDirectory, string targetDirectory, string searchPattern = "*")
{
    foreach (var sourceFile in Directory.GetFiles(sourceDirectory, searchPattern, SearchOption.AllDirectories))
    {
        var relativePath = Path.GetRelativePath(sourceDirectory, sourceFile);
        var targetFile = Path.Combine(targetDirectory, relativePath);
        var targetParentDirectory = Path.GetDirectoryName(targetFile)
            ?? throw new InvalidOperationException("Target file has no parent directory");

        Directory.CreateDirectory(targetParentDirectory);
        File.Copy(sourceFile, targetFile, overwrite: true);
    }
}

void CopyAssets()
{
    Console.WriteLine("Copying assets to wwwroot");

    CopyFiles("../DatenMeister.Web.Assets/wwwroot/", "wwwroot");
}