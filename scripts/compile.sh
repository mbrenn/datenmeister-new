cd ../src
cd Web/DatenMeister.WebServer
npm install 
tsc
cd ../..

cd DatenMeister.Reports.Forms
npm install
tsc
cd ..

dotnet build --configuration Release
dotnet build --configuration Debug

cd ../scripts
