#addin nuget:?package=Cake.Npm&version=5.0.0


var configuration = Argument("configuration", "Debug");
var target = Argument("target", "Build");

Task("Build")
	.Does(() =>
{
	NpmInstall();

	Information("Copying burnJsPopup Files to wwwroot");
	
	CopyFiles("node_modules/burnJsPopup/dist/js/*.*", "wwwroot/js", false);
	CopyFiles("node_modules/burnJsPopup/dist/css/*.*", "wwwroot/css", false);
		
    var process = new System.Diagnostics.Process
    {
        StartInfo =
        {
            FileName = "tsc",
            UseShellExecute = true,            
            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            CreateNoWindow = true    
        }
    };
    process.Start();
    process.WaitForExit();
	
});

RunTarget(target);