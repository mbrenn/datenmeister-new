Set-Location ..
Set-Location src

# Builds the Release Folder
$project = "datenmeister-new.sln"
$parameter = "/p:Configuration=Release"
$switches = "-nologo"

& dotnet build $switches -v:m $parameter $project

Set-Location ./DatenMeister.SourceGeneration.Console/bin/Release/net8.0/

Write-Output ""
Write-Output "-- Creating for DatenMeister itself"
Write-Output ""

./DatenMeister.SourceGeneration.Console.exe

Set-Location ../../../../

Write-Output "-- Creating for DatenMeister.Reports.Forms"


./DatenMeister.SourceGeneration.Console/bin/Release/net8.0/DatenMeister.SourceGeneration.Console.exe "DatenMeister.Reports.Forms\xmi\DatenMeister.Reports.Types.xmi" ".\DatenMeister.Reports.Forms\Model" "DatenMeister.Reports.Forms.Model"

Write-Output "-- Creating for DatenMeister.Extent.Forms"

./DatenMeister.SourceGeneration.Console/bin/Release/net8.0/DatenMeister.SourceGeneration.Console.exe "DatenMeister.Extent.Forms\xmi\DatenMeister.Extent.Forms.Types.xmi" ".\DatenMeister.Extent.Forms\Model" "DatenMeister.Extent.Forms.Model"


# Create .js files
Write-Output "--- Creating Java-Script Files"

Set-Location ..
Set-Location src/DatenMeister.Reports.Forms

tsc -p .

Move-Item Model\*.js Js\ -Force
Move-Item Model\*.ts Js\ -Force
Move-Item Model\*.map Js\ -Force

Set-Location ../..

# Create .js files
Write-Output "--- Creating Java-Script Files"

Set-Location src/DatenMeister.Extent.Forms

tsc -p .

Move-Item Model\*.js Js\ -Force
Move-Item Model\*.ts Js\ -Force
Move-Item Model\*.map Js\ -Force

Set-Location ../..

Set-Location scripts



