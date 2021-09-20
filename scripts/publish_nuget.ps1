Set-Location ..

Set-Location src
$path = &"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe
& $path /property:Configuration=Release

$projects = (Get-ChildItem .)
ForEach-Object $projects
{
    $content = $_.FullName
    Set-Location $content

    & dotnet pack $content.csproj -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -p:Configuration=Release

    Set-Location ..

}