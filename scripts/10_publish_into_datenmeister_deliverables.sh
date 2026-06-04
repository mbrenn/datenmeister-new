#!/bin/bash
# 1) Build the Debug and Release branch of the DatenMeister solution
# 2) Copies all files into the ../datenmeister-deliverables solutions as available in
#    https://github.com/mbrenn/datenmeister-deliverables
# This allows to immediately publish an updated DatenMeister-Assembly package

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
DELIVERABLES_DIR="$SCRIPT_DIR/../../datenmeister-deliverables"
ASSEMBLIES_DIR="$DELIVERABLES_DIR/assemblies"
WEBSERVER_BIN="$SCRIPT_DIR/../src/Web/DatenMeister.WebServer/bin"
SOLUTION="$SCRIPT_DIR/../datenmeister-new.slnx"

# Check if the ../../datenmeister-deliverables folder is available, otherwise create an error message
if [ ! -d "$DELIVERABLES_DIR" ]; then
    echo "ERROR: The datenmeister-deliverables folder was not found at:"
    echo "  $DELIVERABLES_DIR"
    echo "Please clone https://github.com/mbrenn/datenmeister-deliverables next to this repository."
    exit 1
fi

# If yes, remove the contents of ../../datenmeister-deliverables/assemblies and its subdirectories
# This avoids that old assemblies or framework artefacts are existing
echo "Cleaning assemblies directory: $ASSEMBLIES_DIR"
rm -rf "$ASSEMBLIES_DIR"
mkdir -p "$ASSEMBLIES_DIR"

# Compile the DatenMeister Solution as Debug and Release build
echo "Building solution (Debug)..."
dotnet build "$SOLUTION" -c Debug

echo "Building solution (Release)..."
dotnet build "$SOLUTION" -c Release

# Copy all corresponding files from DatenMeister.WebServer/bin/ into the ../../datenmeister-deliverables/assemblies folder
echo "Copying Debug assemblies..."
mkdir -p "$ASSEMBLIES_DIR/Debug"
cp -r "$WEBSERVER_BIN/Debug/." "$ASSEMBLIES_DIR/Debug/"

echo "Copying Release assemblies..."
mkdir -p "$ASSEMBLIES_DIR/Release"
cp -r "$WEBSERVER_BIN/Release/." "$ASSEMBLIES_DIR/Release/"

echo "Done. Assemblies published to: $ASSEMBLIES_DIR"
