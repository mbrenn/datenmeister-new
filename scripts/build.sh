#!/bin/bash
cd ..

cd src/DatenMeister.Reports.Forms
npm install
cd ../..

cd src/Web/DatenMeister.WebServer
npm install 
cd ../../..

cd src/DatenMeister.Reports.Forms
tsc
cd ../..
cd src/Web/DatenMeister.WebServer
tsc
cd ../../..


cd src/DatenMeister.Reports.Forms
dotnet new tool-manifest --force
dotnet tool install Cake.Tool --version 6.1.0
cd ../..

cd src/Web/DatenMeister.WebServer
dotnet new tool-manifest --force
dotnet tool install Cake.Tool --version 6.1.0
dotnet tool install BS_Remove_File_Attribute_From_JUnit
cd ../../..


dotnet build

cd scripts