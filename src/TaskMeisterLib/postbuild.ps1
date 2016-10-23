param([string] $outputPath)

$targetPath = "..\..\..\..\..\datenmeister-new\src\DatenMeister.Web.Application\${outputPath}plugins\"

if ( Test-Path $targetPath )
{
    Write-Host Copying to plugin: $targetPath
    Copy-Item TaskMeisterLib.dll $targetPath
}
else
{
    Write-Host "Target-Path including plugins not found: {$targetPath}"
}
