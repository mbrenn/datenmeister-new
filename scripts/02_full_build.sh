# Build all the stuff
dotnet clean ../datenmeister-new.slnx -c Release
dotnet clean ../datenmeister-new.slnx -c Debug
dotnet build ../datenmeister-new.slnx -c Release
dotnet build ../datenmeister-new.slnx -c Debug

# Install .Net Cake
cd ../src/DatenMeister.Reports.Forms
npm install
tsc
dotnet new tool-manifest --force
dotnet tool install Cake.Tool --version 6.1.0
cd ../../scripts


cd ../src/DatenMeister.Reports.Forms
npm install
tsc
dotnet new tool-manifest --force
dotnet tool install Cake.Tool --version 6.1.0
cd ../../scripts


# Copy the Action.Executor to the example directory
echo Copy Action.Executor to example directory

# Create target directory in case it is not existing
mkdir -p ../example/actions/Executable
cp -r ../src/DatenMeister.Action.Executor/bin/Release/net10.0/* ../example/actions/Executable


