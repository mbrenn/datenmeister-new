Push-Location

cd ..\src\DatenMeister
msbuild DatenMeister.csproj /property:Configuration=Release

Pop-Location

Remove-Item -Path .\DatenMeister\lib -Recurse
New-Item -ItemType Directory -Path DatenMeister/lib -Force | Out-Null
New-Item -ItemType Directory -Path DatenMeister/lib/dotnet -Force | Out-Null

Copy-Item ..\src\DatenMeister\bin\Release/*.* DatenMeister/lib/dotnet 


Push-Location

cd DatenMeister
nuget pack DatenMeister.nuspec

Pop-Location

New-Item -ItemType Directory -Path All -Force | Out-Null
Copy-Item DatenMeister/*.nupkg All/


Write-Host "The packages are created, you can now push them via nuget push All/yourpackage.nupkg"