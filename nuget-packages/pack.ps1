
Import-Module -Name "./Invoke-MsBuild.psm1" 


$projects = "DatenMeister", "DatenMeister.CSV"

ForEach($project in $projects)
{
    Write-Host "Creating package for $project"
    Push-Location

    cd ..\src\$project
    $buildResult = Invoke-MsBuild -Path "$project.csproj" -MsBuildParameters "/property:Configuration=Release"
    if ( -Not $buildResult)
    {
        Write-Host "Build of $project failed. Aborting"
        Exit
    }

    Pop-Location

    Remove-Item -Path .\$project\lib -Recurse
    New-Item -ItemType Directory -Path $project/lib -Force | Out-Null
    New-Item -ItemType Directory -Path $project/lib/dotnet -Force | Out-Null

    Copy-Item ..\src\$project\bin\Release/*.* $project/lib/dotnet 


    Push-Location

    cd $project
    nuget pack $project.nuspec

    Pop-Location

    New-Item -ItemType Directory -Path All -Force | Out-Null
    Copy-Item $project/*.nupkg All/
}


Write-Host "The packages are created, you can now push them via nuget push All/yourpackage.nupkg"