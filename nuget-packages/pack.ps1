
Import-Module -Name "./Invoke-MsBuild.psm1" 


$projects = "DatenMeister", "DatenMeister.Runtime", "DatenMeister.CSV", "DatenMeister.XMI", "DatenMeister.Uml", "DatenMeister.Provider.DotNet"

ForEach ($project in $projects)
{
    
    ## Build the project
    Write-Host "{$project}: Building"
    Push-Location

    cd ..\src\$project
    $buildResult = Invoke-MsBuild -Path "$project.csproj" -MsBuildParameters "/property:Configuration=Release"
    if ( -Not $buildResult)
    {
        Write-Host "Build of $project failed. Aborting"
        Pop-Location
        Exit
    }

    Pop-Location
    
    ## Copy the binaries to the $project/lib/dotnet folder
    Write-Host "{$project}: Copying binaries"

    if (Test-Path .\$project\lib)
    {
        Remove-Item -Path .\$project\lib -Recurse
    }

    New-Item -ItemType Directory -Path $project/lib -Force | Out-Null
    New-Item -ItemType Directory -Path $project/lib/dotnet -Force | Out-Null

    Copy-Item ..\src\$project\bin\Release\$project.dll $project/lib/dotnet 
    Copy-Item ..\src\$project\bin\Release\$project.pdb $project/lib/dotnet 

    ## Creates the packages
    Write-Host "{$project}: Create NuGet-Packages"
    Push-Location

    cd $project
    nuget pack $project.nuspec | Out-Null

    Pop-Location

    New-Item -ItemType Directory -Path All -Force | Out-Null
    Copy-Item $project/*.nupkg All/
    
    ## Now clean the created directories, only the 'All' folder shall contain values.
    Write-Host "{$project}: Cleaning" 
    Remove-Item $project/*.nupkg    
    Remove-Item -Path .\$project\lib -Recurse
}

Write-Host
Write-Host "The packages are created, you can now push them via nuget push All/yourpackage.nupkg"