cd ../src
cd Web/DatenMeister.WebServer
npm install 
cd ../..

cd DatenMeister.Reports.Forms
npm install
cd ..

dotnet build --configuration Release
dotnet build --configuration Debug

cd ../scripts


