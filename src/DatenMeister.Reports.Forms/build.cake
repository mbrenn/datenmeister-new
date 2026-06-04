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
        
    NpmExec("esbuild", new [] { args.Render() });
});

RunTarget(target);