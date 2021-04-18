# Creates Directory
New-Item -ItemType Directory -Force -Path output | Out-Null

# Removes existing .png files
Remove-Item *.png
Remove-Item output/*.png

# Creates the webfiles
$files = Get-ChildItem "*.svg"

foreach($file in $files)
{
    $filename = $file.BaseName
    & inkscape.exe --export-type="png" $file

    & inkscape --export-type="png" --export-width=128 --export-filename="$filename-128.png" $file
    & inkscape --export-type="png" --export-width=64 --export-filename="$filename-64.png" $file
    & inkscape --export-type="png" --export-width=32 --export-filename="$filename-32.png" $file
    & inkscape --export-type="png" --export-width=48 --export-filename="$filename-48.png" $file
}

# Waits until inkscape is closed
Write-Output "Waiting for Inkscape"
Wait-Process -name "inkscape"
Write-Output "Inkscape closed"

# Now move the png to the output directory
Move-Item *.png output/