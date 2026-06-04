#addin nuget:?package=Cake.Npm&version=5.1.0


var configuration = Argument("configuration", "Debug");
var target = Argument("target", "Build");

Task("Build")
	.Does(() =>
{
	NpmInstall();

    var args = new ProcessArgumentBuilder()
        .AppendQuoted("Js/DatenMeister.Reports.Forms.ts")
        .AppendQuoted("Js/DatenMeister.Reports.Types.ts")
        .Append("--minify")
        .Append("--sourcemap")
        .Append("--outdir=Js")
        .Append("--platform=browser")
        .Append("--format=esm");

    var process = new System.Diagnostics.Process
    {
        StartInfo =
        {
            FileName = "npx",
            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            UseShellExecute = true,
            CreateNoWindow = true,
            Arguments = "esbuild " + args.Render()
        }
    };
    process.Start();
    process.WaitForExit();
});

RunTarget(target);