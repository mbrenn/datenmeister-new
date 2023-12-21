# Get out of the scripts and to the src

Set-Location ..
Set-Location src

$pathToDeliverables = "..\datenmeister-deliverables\assemblies"

# Builds the Release Folder
$project = "datenmeister-new.sln"
$parameter = "/p:Configuration=Release"
$switches = "-nologo"

& dotnet build $switches -v:m $parameter $project

# Builds the Debug Folder

$parameter = "/p:Configuration=Debug"

& dotnet build $switches -v:m $parameter $project


# Get Back to the root
Set-Location ..

Remove-Item -Force -Recurse -Path $pathToDeliverables
New-Item -ItemType Directory -Force -Path "$($pathToDeliverables)"
New-Item -ItemType Directory -Force -Path "$($pathToDeliverables)/Debug"
New-Item -ItemType Directory -Force -Path "$($pathToDeliverables)/Debug\net6.0"
New-Item -ItemType Directory -Force -Path "$($pathToDeliverables)/Debug\net7.0"
New-Item -ItemType Directory -Force -Path "$($pathToDeliverables)/Release"
New-Item -ItemType Directory -Force -Path "$($pathToDeliverables)/Release\net6.0"
New-Item -ItemType Directory -Force -Path "$($pathToDeliverables)/Release\net7.0"

# Now copy all the stuff to the deliverables

Copy-Item .\src\Web\DatenMeister.WebServer\bin\Debug\net6.0\*.dll "$($pathToDeliverables)\Debug\net6.0"
Copy-Item .\src\Web\DatenMeister.WebServer\bin\Debug\net6.0\*.pdb "$($pathToDeliverables)\Debug\net6.0"
Copy-Item .\src\Web\DatenMeister.WebServer\bin\Release\net6.0\*.dll "$($pathToDeliverables)\Release\net6.0"
Copy-Item .\src\Web\DatenMeister.WebServer\bin\Release\net6.0\*.pdb "$($pathToDeliverables)\Release\net6.0"

Copy-Item .\src\Web\DatenMeister.WebServer\bin\Debug\net7.0\*.dll "$($pathToDeliverables)\Debug\net7.0"
Copy-Item .\src\Web\DatenMeister.WebServer\bin\Debug\net7.0\*.pdb "$($pathToDeliverables)\Debug\net7.0"
Copy-Item .\src\Web\DatenMeister.WebServer\bin\Release\net7.0\*.dll "$($pathToDeliverables)\Release\net7.0"
Copy-Item .\src\Web\DatenMeister.WebServer\bin\Release\net7.0\*.pdb "$($pathToDeliverables)\Release\net7.0"


Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Release\net6.0\*.pdb "$($pathToDeliverables)\Release\net6.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Release\net7.0\*.pdb "$($pathToDeliverables)\Release\net7.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Debug\net6.0\*.pdb "$($pathToDeliverables)\Debug\net6.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Debug\net7.0\*.pdb "$($pathToDeliverables)\Debug\net7.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Release\net6.0\*.dll "$($pathToDeliverables)\Release\net6.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Release\net7.0\*.dll "$($pathToDeliverables)\Release\net7.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Debug\net6.0\*.dll "$($pathToDeliverables)\Debug\net6.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Debug\net7.0\*.dll "$($pathToDeliverables)\Debug\net7.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Release\net6.0\*.exe "$($pathToDeliverables)\Release\net6.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Release\net7.0\*.exe "$($pathToDeliverables)\Release\net7.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Debug\net6.0\*.exe "$($pathToDeliverables)\Debug\net6.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Debug\net7.0\*.exe "$($pathToDeliverables)\Debug\net7.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Release\net6.0\*.json "$($pathToDeliverables)\Release\net6.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Release\net7.0\*.json "$($pathToDeliverables)\Release\net7.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Debug\net6.0\*.json "$($pathToDeliverables)\Debug\net6.0"
Copy-Item .\src\DatenMeister.SourceGeneration.Console\bin\Debug\net7.0\*.json "$($pathToDeliverables)\Debug\net7.0"


# And now back to scripts
Set-Location scripts