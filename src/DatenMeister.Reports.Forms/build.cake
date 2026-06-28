#addin nuget:?package=Cake.Npm&version=5.1.0


var configuration = Argument("configuration", "Debug");
var target = Argument("target", "Build");


Task("Compile TS")
    .Does(() => 
    {    
        Information("Compile TypeScript files");        
        
        NpmExec("tsc");
    });
    
Task("Build")
    .IsDependentOn("Compile TS")
	.Does(() =>
{
	NpmInstall();

    var args = new ProcessArgumentBuilder()
        .AppendQuoted("Assets/Js/DatenMeister.Reports.Forms.js")
        .AppendQuoted("Assets/Js/DatenMeister.Reports.Types.js")
        .Append("--minify")
        .Append("--sourcemap")
        .Append("--outdir=Js")
        .Append("--platform=browser")
        .Append("--format=esm");
        
    NpmExec("esbuild", new [] { args.Render() });
});

RunTarget(target);