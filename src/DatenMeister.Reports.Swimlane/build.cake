#addin nuget:?package=Cake.Npm&version=5.1.0

var configuration = Argument("configuration", "Debug");
var target = Argument("target", "Build");

var swimlaneSourceFile = "Assets/Js/DatenMeister.Reports.Swimlane.Source.ts";
var typesSourceFile = "Model/Types.ts";
var mergedOutputFile = "Assets/Js/DatenMeister.Reports.Swimlane.ts";

Task("Merge TS")
    .Does(() =>
    {
        var outputExists = System.IO.File.Exists(mergedOutputFile);
        if (outputExists)
        {
            var outputTime = System.IO.File.GetLastWriteTimeUtc(mergedOutputFile);
            var swimlaneTime = System.IO.File.GetLastWriteTimeUtc(swimlaneSourceFile);
            var typesTime = System.IO.File.GetLastWriteTimeUtc(typesSourceFile);

            if (swimlaneTime <= outputTime && typesTime <= outputTime)
            {
                Information("Skipping Merge TS: {0} is up-to-date.", mergedOutputFile);
                return;
            }
        }

        var swimlane = System.IO.File.ReadAllText(swimlaneSourceFile);
        var types = System.IO.File.ReadAllText(typesSourceFile);
        var result = "// This file is generated in build.cake. Do NOT modify that file.\n";
        result += "// Modify DatenMeister.Reports.Swimlane.Source.ts\n";
        result += "// noinspection DuplicatedCode\n\n";

        result += swimlane + "\n\n" + types;
        System.IO.File.WriteAllText(mergedOutputFile, result);
    });

Task("Compile TS")
    .IsDependentOn("Merge TS")
    .Does(() => 
    {
        StartProcess("npx", new ProcessSettings { Arguments = "tsc" });
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
    
    System.IO.File.Delete(mergedOutputFile);
});

RunTarget(target);