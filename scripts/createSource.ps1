Set-Location ..
Set-Location src

$pathToDeliverables = "..\datenmeister-deliverables\assemblies"

# Builds the Release Folder
$project = "datenmeister-new.sln"
$parameter = "/p:Configuration=Release"
$switches = "-nologo"

& dotnet build $switches -v:m $parameter $project

Set-Location DatenMeister.SourceGeneration.Console/bin/Release/net6.0

./DatenMeister.SourceGeneration.Console.exe

Set-Location ../../../..


# And now back to scripts
Set-Location ..
Set-Location scripts