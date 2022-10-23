#!/bin/bash

# Removes working directory
rm -rf working

# Creates the working directory
mkdir working
cd working

# Gets the complete and nice little sourcecode
git clone https://github.com/mbrenn/datenmeister-new.git -b master

# Creates the target directory for the publish
mkdir deploy

# Now moving to the right folder and install the npm packages
cd datenmeister-new
cd src/Web/DatenMeister.WebServer
npm install

# If that is done, let's create the .Net assembly
dotnet publish DatenMeister.WebServer.csproj -c Release -o ../../../../deploy

# Get rid of the AppSettings files which might overwrite the deploy configuration
cd ../../../../deploy
rm appsettings.Development.json
mv appsettings.json appsettings.Example.json

# Remove the repository to keep things clean
cd ..
rm -rf datenmeister-new

# Switch to deploy
cd deploy