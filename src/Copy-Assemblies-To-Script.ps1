
Import-Module -Name "./Invoke-MsBuild.psm1" 
$projects = "DatenMeister", 
    "DatenMeister.Runtime", 
    "DatenMeister.CSV", 
    "DatenMeister.XMI", 
    "DatenMeister.Uml", 
    "DatenMeister.Provider.DotNet", 
    "DatenMeister.ManualMapping", 
    "DatenMeister.Full.Integration"

if (!(Test-Path -Path .\scripts\assembly))
{
    New-Item -ItemType Directory -Path .\scripts\assembly | Out-Null
}

ForEach ($project in $projects)
{    
    ## Build the project
    Write-Host "{$project}: Building"
    Push-Location

    cd ..\src\$project
    $buildResult = Invoke-MsBuild -Path "$project.csproj" -MsBuildParameters "/property:Configuration=Release"

    Pop-Location
    
    Copy-Item ..\src\$project\bin\Debug\$project.dll .\scripts\assembly\$project.dll
    Copy-Item ..\src\$project\bin\Debug\$project.pdb .\scripts\assembly\$project.pdb

}