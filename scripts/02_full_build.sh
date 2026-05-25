# Build all the stuff
dotnet build ../datenmeister-new.slnx -c Release
dotnet build ../datenmeister-new.slnx -c Debug

# Copy the Action.Executor to the example directory
echo Copy Action.Executor to example directory

# Create target directory in case it is not existing
mkdir -p ../example/actions/Executable
cp -r ../src/DatenMeister.Action.Executor/bin/Release/net10.0/* ../example/actions/Executable


