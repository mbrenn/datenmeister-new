# Base stage with system dependencies
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base

RUN dotnet tool install --global BurnSystems.Make.Exe --version 1.0.4
ENV PATH="${PATH}:/root/.dotnet/tools"

RUN apt-get update && apt-get install -y python3 npm && rm -rf /var/lib/apt/lists/*
RUN npm install -g typescript@6.0.3

# Build stage
FROM base AS build
WORKDIR /build

COPY . .

# Take care for all preconditions
## NPM

WORKDIR /build/src/Web/DatenMeister.WebServer
RUN npm install && tsc
WORKDIR /

WORKDIR /build/src/DatenMeister.Reports.Forms
RUN npm install && tsc

WORKDIR /build/src/DatenMeister.Reports.Swimlane
RUN npm install && tsc

WORKDIR /build/src/DatenMeister.DataView.Forms
RUN npm install && tsc

WORKDIR /build/src/Templates/DatenMeister.Templates.TypeScriptAndTypesPlugin
RUN npm install && tsc


FROM build AS publish-executable
WORKDIR /build/scripts
RUN sed -i 's/\r$//' *.sh
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

RUN bsmake

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
