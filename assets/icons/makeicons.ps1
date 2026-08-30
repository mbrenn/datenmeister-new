param(
    [string]$TargetFolder = $PSScriptRoot
)

if (-not $TargetFolder) {
    $TargetFolder = "."
}

$outputDir = Join-Path $TargetFolder "output"

# Creates Directory if not exists
if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Force -Path $outputDir | Out-Null
}

# Removes existing .png files in the output directory and source directory
Remove-Item (Join-Path $TargetFolder "*.png") -Force -ErrorAction SilentlyContinue
Remove-Item (Join-Path $outputDir "*.png") -Force -ErrorAction SilentlyContinue

# Detect available converter (prefer resvg / sharp-cli / rsvg-convert / magick)
$hasResvg = (Get-Command resvg -ErrorAction SilentlyContinue) -ne $null
$hasRsvg = (Get-Command rsvg-convert -ErrorAction SilentlyContinue) -ne $null
$hasMagick = (Get-Command magick -ErrorAction SilentlyContinue) -ne $null
$hasNpx = (Get-Command npx -ErrorAction SilentlyContinue) -ne $null

Write-Output "SVG to PNG Converter detected:"
if ($hasResvg) {
    Write-Output "Using: resvg"
} elseif ($hasNpx) {
    Write-Output "Using: sharp-cli (via npx)"
} elseif ($hasRsvg) {
    Write-Output "Using: rsvg-convert"
} elseif ($hasMagick) {
    Write-Output "Using: magick"
} else {
    Write-Error "No supported modern SVG converter found (npx/sharp-cli, resvg, rsvg-convert, or magick)."
    exit 1
}

$sizes = @(
    @{ Name = ""; Width = 256; Height = 256 },
    @{ Name = "-128"; Width = 128; Height = 128 },
    @{ Name = "-64"; Width = 64; Height = 64 },
    @{ Name = "-48"; Width = 48; Height = 48 },
    @{ Name = "-32"; Width = 32; Height = 32 },
    @{ Name = "-16"; Width = 16; Height = 16 }
)

# Convert all SVGs
$files = Get-ChildItem -Path $TargetFolder -Filter "*.svg"

foreach ($file in $files) {
    $filename = $file.BaseName
    Write-Output "Converting $($file.Name)..."

    foreach ($size in $sizes) {
        $outName = if ($size.Name -eq "") { "$filename.png" } else { "$filename$($size.Name).png" }
        $outPath = Join-Path $outputDir $outName

        if ($hasResvg) {
            & resvg --width $size.Width --height $size.Height $file.FullName $outPath
        } elseif ($hasNpx) {
            if ($size.Name -eq "") {
                & npx --yes sharp-cli -i $file.FullName -o $outPath
            } else {
                & npx --yes sharp-cli -i $file.FullName -o $outPath resize $size.Width $size.Height
            }
        } elseif ($hasRsvg) {
            & rsvg-convert -w $size.Width -h $size.Height -f png -o $outPath $file.FullName
        } elseif ($hasMagick) {
            & magick -background none -density 300 $file.FullName -resize "$($size.Width)x$($size.Height)" $outPath
        }
    }
}

Write-Output "Icon conversion completed successfully. Output saved to: $outputDir"