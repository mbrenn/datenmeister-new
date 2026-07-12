#addin nuget:?package=Cake.Npm&version=5.1.0

var configuration = Argument("configuration", "Debug");
var target = Argument("target", "Build");

Task("Merge TS")
    .Does(() =>
    {
        var swimlane = System.IO.File.ReadAllText("Assets/Js/DatenMeister.Reports.Swimlane.Source.ts");
        var types = System.IO.File.ReadAllText("Model/Types.ts");
        var result = "// This file is generated in build.cake. Do NOT modify that file.\n";
        result += "// Modify DatenMeister.Reports.Swimlane.Source.ts\n";
        result += "// noinspection DuplicatedCode\n\n";
        
        result += swimlane + "\n\n" + types;
        System.IO.File.WriteAllText("Assets/Js/DatenMeister.Reports.Swimlane.ts", result);
    });

Task("Compile TS")
    .IsDependentOn("Merge TS")
    .Does(() => 
    {
        NpmExec("tsc");
    });
    
Task("Build")
    .IsDependentOn("Compile TS")
	.Does(() =>
{
	NpmInstall();

    var args = new ProcessArgumentBuilder()
        .AppendQuoted("Assets/Js/*.js")
        .Append("--minify")
        .Append("--sourcemap")
        .Append("--outdir=Js")
        .Append("--platform=browser")
        .Append("--format=esm");
        
    NpmExec("esbuild", new [] { args.Render() });
});

RunTarget(target);