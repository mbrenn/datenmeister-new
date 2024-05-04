Set-Location ..
Set-Location src

# Builds the Release Folder
$project = "datenmeister-new.sln"
$parameter = "/p:Configuration=Release"
$switches = "-nologo"

# & dotnet build $switches -v:m $parameter $project

Set-Location ./DatenMeister.SourceGeneration.Console/bin/Release/net8.0/

Write-Output ""
Write-Output "-- Creating for DatenMeister itself"
Write-Output ""

./DatenMeister.SourceGeneration.Console.exe

Set-Location ../../../../

Write-Output ""
Write-Output "-- Creating for DatenMeister.Reports.Forms"
Write-Output ""


./DatenMeister.SourceGeneration.Console/bin/Release/net8.0/DatenMeister.SourceGeneration.Console.exe "DatenMeister.Reports.Forms\xmi\DatenMeister.Reports.Types.xmi" ".\DatenMeister.Reports.Forms\Model" "DatenMeister.Reports.Forms.Model"

# Create .js files
Write-Output ""
Write-Output "-- Creating Java-Script Files"
Write-Output ""

Set-Location ..
Set-Location src/DatenMeister.Reports.Forms

tsc -p .

Move-Item Model\*.js Js\ -Force
Move-Item Model\*.ts Js\ -Force
Move-Item Model\*.map Js\ -Force

Set-Location ../..
Set-Location scripts



