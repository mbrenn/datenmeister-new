

$apikey = (Get-ChildItem -Path env:/nuget-apikey).Value

$args | ForEach-Object {
    
    $files = Get-ChildItem $_

    $files | ForEach-Object {
        $package = $_.Name        
        & dotnet nuget push $package -s https://api.nuget.org/v3/index.json --api-key=$apikey        
    }
}