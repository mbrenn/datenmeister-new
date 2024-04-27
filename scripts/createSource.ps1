Set-Location ..
Set-Location src

# Builds the Release Folder
$project = "datenmeister-new.sln"
$parameter = "/p:Configuration=Release"
$switches = "-nologo"

# & dotnet build $switches -v:m $parameter $project

# ./DatenMeister.SourceGeneration.Console/bin/Release/net8.0/DatenMeister.SourceGeneration.Console.exe

./DatenMeister.SourceGeneration.Console/bin/Release/net8.0/DatenMeister.SourceGeneration.Console.exe "DatenMeister.Reports.Forms\xmi\DatenMeister.Reports.Types.xmi" ".\DatenMeister.Reports.Forms\Model" "DatenMeister.Reports.Forms.Model"

# Create .js files
Write-Output ""
Write-Output "Creating Java-Script Files"

Set-Location ..
Set-Location src/DatenMeister.Reports.Forms

tsc -p .

Set-Location ../..
Set-Location scripts



