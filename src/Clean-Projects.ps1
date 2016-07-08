$Directories = Get-ChildItem -Directory
ForEach($Directory in $Directories)
{
    $binPath = Join-Path $Directory "bin/"

    if(Test-Path -Path $binPath)
    {
        Write-Host "Removing: $binPath" 
        Remove-Item $binPath -Recurse
    }
}

