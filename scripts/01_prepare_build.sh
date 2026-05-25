# Install .Net Pre-Requisites
apt-get update && apt-get install -y python3 npm git dotnet-sdk-10.0
npm install -g typescript@5.9.2

# Install .Net Cake
cd ../src/DatenMeister.Reports.Forms
dotnet new tool-manifest --force
dotnet tool install Cake.Tool --version 6.1.0
cd ../../scripts


cd ../src/DatenMeister.Reports.Forms
dotnet new tool-manifest --force
dotnet tool install Cake.Tool --version 6.1.0
cd ../../scripts


