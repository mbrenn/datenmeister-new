# Base stage with system dependencies
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base
RUN apt-get update && apt-get install -y python3 npm && rm -rf /var/lib/apt/lists/*
RUN npm install -g typescript@5.9.2

# Build stage
FROM base AS build
WORKDIR /build

COPY . .

# Take care for all preconditions
## NPM

WORKDIR src/DatenMeister.Reports.Forms
RUN npm install
RUN tsc
RUN dotnet new tool-manifest --force
RUN dotnet tool install Cake.Tool --version 6.1.0

WORKDIR /build
WORKDIR src/Web/DatenMeister.WebServer
RUN npm install
RUN tsc
RUN dotnet new tool-manifest --force
RUN dotnet tool install Cake.Tool --version 6.1.0
WORKDIR /

FROM build AS publish-executable
WORKDIR /build/scripts
RUN ./02_full_build.sh
RUN ./02a_clean_for_docker.sh
WORKDIR /

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime-bash
WORKDIR /app
COPY --from=publish-executable /build .

ENTRYPOINT ["bash"]

# Build stage
FROM build AS publish
WORKDIR /build

RUN dotnet publish src/Web/DatenMeister.WebServer/DatenMeister.WebServer.csproj \
    -c Release \
    -o /app/publish \
    --no-self-contained 

RUN mv /app/publish/DatenMeister.Settings.Docker.xmi /app/publish/DatenMeister.Settings.xmi  

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:5000

EXPOSE 5000

ENTRYPOINT ["dotnet", "DatenMeister.WebServer.dll"]
