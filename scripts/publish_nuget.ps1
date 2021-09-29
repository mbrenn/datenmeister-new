Set-Location ..

Set-Location src
$path = &"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe
& $path /property:Configuration=Release

$projects = (Get-ChildItem . -Directory)

$projects | Format-Table

$projects | ForEach-Object {
    $content = $_.FullName
    Set-Location $content

    $projectFiles = (Get-ChildItem *.csproj)

    $projectFiles | ForEach-Object {
        $projectFile = $_.FullName
        if(Test-Path $projectFile -PathType Leaf) {
            & dotnet pack $projectFile -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -p:Configuration=Release

            Move-Item bin/Release/*.nupkg ../../nugets/ -Force
            Move-Item bin/Release/*.snupkg ../../nugets/ -Force
        }
    }

    Set-Location ..
}

Set-Location ..
Set-Location scripts