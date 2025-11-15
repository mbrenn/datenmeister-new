Set-Location ..
Set-Location src

# Builds the Release Folder
$project = "datenmeister-new.sln"
$parameter = "/p:Configuration=Release"
$switches = "-nologo"

$onlyOne = $false;


if($args -contains '--onlyone')
{
    $onlyOne = $true;
    Write-Host "We just test our automatic generation on one type  (--onlyone)"
}

if(!($args -contains '--nobuild')) {    
    Write-Output "Building DatenMeister (can be skipped with --nobuild)"
    Set-Location DatenMeister.SourceGeneration.Console
    & dotnet build DatenMeister.SourceGeneration.Console.csproj $switches -v:m $parameter
    Set-Location ..
}
else
{
    Write-Host "Skipping build"
}


if(!$onlyOne)
{    
    Write-Output "-- Mof"
    dotnet ./DatenMeister.SourceGeneration.Console/bin/Release/net9.0/DatenMeister.SourceGeneration.Console.dll "DatenMeister.Core/XmiFiles/MOF.xmi" "./DatenMeister.Core.Model/EMOF" "DatenMeister.Core.Models.EMOF" "dm:///_internal/model/mof"

    Write-Output "-- UML"
    dotnet ./DatenMeister.SourceGeneration.Console/bin/Release/net9.0/DatenMeister.SourceGeneration.Console.dll "DatenMeister.Core/XmiFiles/UML.xmi" "./DatenMeister.Core.Model/EMOF" "DatenMeister.Core.Models.EMOF" "dm:///_internal/model/uml"

    Write-Output "-- PrimitiveTypes"
    dotnet ./DatenMeister.SourceGeneration.Console/bin/Release/net9.0/DatenMeister.SourceGeneration.Console.dll "DatenMeister.Core/XmiFiles/PrimitiveTypes.xmi" "./DatenMeister.Core.Model/EMOF" "DatenMeister.Core.Models.EMOF" "dm:///_internal/model/primitivetypes"

    Write-Output "-- Creating for DatenMeister.Core"
    dotnet ./DatenMeister.SourceGeneration.Console/bin/Release/net9.0/DatenMeister.SourceGeneration.Console.dll "DatenMeister.Core/XmiFiles/Types/DatenMeister.xmi" "./DatenMeister.Core.Model" "DatenMeister.Core.Models" "dm:///_internal/types/internal"

    Write-Output "-- Creating for DatenMeister.Reports.Forms"
    dotnet ./DatenMeister.SourceGeneration.Console/bin/Release/net9.0/DatenMeister.SourceGeneration.Console.dll "DatenMeister.Reports.Forms/Xmi/DatenMeister.Reports.Types.xmi" "./DatenMeister.Reports.Forms/Model" "DatenMeister.Reports.Forms.Model" "dm:///_internal/types/internal"

    Write-Output "-- Creating for DatenMeister.Extent.Forms"
    dotnet ./DatenMeister.SourceGeneration.Console/bin/Release/net9.0/DatenMeister.SourceGeneration.Console.dll "DatenMeister.Extent.Forms/Xmi/DatenMeister.Extent.Forms.Types.xmi" "./DatenMeister.Extent.Forms/Model" "DatenMeister.Extent.Forms.Model" "dm:///_internal/types/internal" 
}

Write-Output "-- Creating for DatenMeister.Zip"
dotnet ./DatenMeister.SourceGeneration.Console/bin/Release/net9.0/DatenMeister.SourceGeneration.Console.dll "DatenMeister.Zip/Xmi/DatenMeister.Zip.Types.xmi" "./DatenMeister.Zip/Model" "DatenMeister.Zip.Model" "dm:///_internal/types/internal"


# Create .js files
Write-Output "--- Creating Java-Script Files for Reports"

Set-Location ..
Set-Location src/DatenMeister.Reports.Forms

tsc -p .

Move-Item Model/*.js Js/ -Force
Move-Item Model/*.ts Js/ -Force
Move-Item Model/*.map Js/ -Force

Set-Location ../..

# Create .js files
Write-Output "--- Creating Java-Script Files for Extents"

Set-Location src/DatenMeister.Extent.Forms

tsc -p .

Move-Item Model/*.js Js/ -Force
Move-Item Model/*.ts Js/ -Force
Move-Item Model/*.map Js/ -Force

Set-Location ../..

Move-Item src/DatenMeister.Core.Model/DatenMeister.ts src/Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/DatenMeister.class.ts -Force
Move-Item src/DatenMeister.Core.Model/EMOF/mof.ts src/Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/mof.ts -Force
Move-Item src/DatenMeister.Core.Model/EMOF/uml.ts src/Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/uml.ts -Force
Move-Item src/DatenMeister.Core.Model/EMOF/primitivetypes.ts src/Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/primitivetypes.ts -Force

Set-Location scripts



