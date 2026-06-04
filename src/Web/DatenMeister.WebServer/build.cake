#addin nuget:?package=Cake.Npm&version=5.1.0


var configuration = Argument("configuration", "Debug");
var target = Argument("target", "Build");

Task("Install Npm")
    .Does(() =>
{
	NpmInstall();
});

Task("Copy-NodeModules")
	.Does(() =>
{
    var nodeModules = Directory("./node_modules");
    var target = Directory("./wwwroot/js/node_modules");

    var files = GetFiles("./node_modules/**/*.js");
    files.Add(GetFiles("./node_modules/**/*.d.ts"));
    files.Add(GetFiles("./node_modules/**/*.css"));

    foreach (var file in files)
    {
        var positionNodeModules = file.ToString().IndexOf("node_modules/"); 
        if(positionNodeModules == -1 )
        {
            Console.WriteLine("Some obscure error occured");
        }
        
        var relativePath = file.ToString().Substring(positionNodeModules + "node_modules/".Length);
        var targetDir = System.IO.Path.GetDirectoryName(target.Path.Combine(relativePath).ToString());
        
        if (!DirectoryExists(targetDir))
        {
            CreateDirectory(targetDir);
        }
        
        CopyFileToDirectory(file, targetDir);
    }
});

Task("Compress CSS")
    .Does(() =>
    {
        Information("Compressing CSS Files");
        
        var cssFiles = System.IO.Directory.GetFiles("Assets/css/", "*.css").Where(x=>x.StartsWith("Assets/css/datenmeister.")).ToList();
        cssFiles.Add("Assets/css/burnJsPopup.css");
             	
     	var outputFile = "wwwroot/css/datenmeister-web.min.css";
     	var args = new ProcessArgumentBuilder()
     		.Append("-o")
     		.AppendQuoted(outputFile);
     		
     	foreach(var file in cssFiles)
     	{
     		args.AppendQuoted(file);
     	}
     
        // Run the minimum                        
        var process = new System.Diagnostics.Process
        {
            StartInfo =
            {
                FileName = "npx",
                UseShellExecute = true,            
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                CreateNoWindow = true,    
                Arguments = "cleancss --format keep-breaks --source-map " + args.Render(),
            }
        };
        process.Start();
        process.WaitForExit();
        
        if(process.ExitCode != 0)        
        {
            throw new Exception($"cleancss failed with exit code {process.ExitCode}");
        }
    });

Task("Compile TS")
    .Does(() => 
    {    
        Information("Compile TypeScript files");        
        
        NpmExec("tsc");
        
        /*

        var mainProcess = new System.Diagnostics.Process
        {
            StartInfo =
            {
                FileName = "tsc",
                UseShellExecute = false,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true               
            }
        };
        
        mainProcess.Start();
        var stdout = mainProcess.StandardOutput.ReadToEnd();
        var stderr = mainProcess.StandardError.ReadToEnd();
        mainProcess.WaitForExit();
        if (!string.IsNullOrWhiteSpace(stdout)) Information(stdout);
        if (!string.IsNullOrWhiteSpace(stderr)) Error(stderr);
        
        if(mainProcess.ExitCode != 0)        
        {
            throw new Exception($"tsc failed with exit code {mainProcess.ExitCode}");
        }*/
    });

Task("Compress JS")
    .Does(() =>
    {
        Information("Compiling and minifying TypeScript files");
        
        NpmExec("esbuild", new [] {"\"Assets/js/**/*.js\" --minify --sourcemap --outbase=Assets/js --outdir=wwwroot/js --platform=browser --format=esm"});        
    });

Task("Build")
    .IsDependentOn("Install Npm")
    .IsDependentOn("Compress CSS")    
    .IsDependentOn("Compile TS")
    .IsDependentOn("Compress JS")
	.IsDependentOn("Copy-NodeModules")
	.Does(() =>
{
	Information("Copying burnJsPopup Files to wwwroot");
	
	CopyFiles("node_modules/burnJsPopup/dist/js/*.*", "wwwroot/js", false);
	CopyFiles("node_modules/burnJsPopup/dist/css/*.*", "wwwroot/css", false);
	
	Information("Copying JQuery FancyTree");
	
	CopyFiles("node_modules/jquery.fancytree/dist/*.min.js", "wwwroot/js", false);
	CopyFiles("node_modules/jquery.fancytree/dist/skin-win8/*.css", "wwwroot/css/jquery.fancytree/css", false);
	CopyFiles("node_modules/jquery.fancytree/dist/skin-win8/*.gif", "wwwroot/css/jquery.fancytree/skin-win8", false);
});

RunTarget(target);