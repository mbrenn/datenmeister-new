#addin nuget:?package=Cake.Npm&version=5.0.0


var configuration = Argument("configuration", "Debug");
var target = Argument("target", "Build");

Task("Build")
	.Does(() =>
{
	NpmInstall();

    var process = new System.Diagnostics.Process
    {
        StartInfo =
        {
            FileName = "tsc",
            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            UseShellExecute = true,
            CreateNoWindow = true    
        }
    };
    process.Start();
    process.WaitForExit();
	
});

RunTarget(target);