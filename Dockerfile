# Base stage with system dependencies
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
RUN apt-get update && apt-get install -y python3 npm && rm -rf /var/lib/apt/lists/*
RUN npm install -g typescript@5.9.2

# Build stage
FROM base AS build
WORKDIR /src

COPY . .

# Take care for all preconditions
## NPM

WORKDIR /src/src/DatenMeister.Reports.Forms
RUN npm install
RUN tsc
RUN dotnet new tool-manifest --force
RUN dotnet tool install Cake.Tool --version 5.0.0
WORKDIR /src

WORKDIR /src/src/Web/DatenMeister.WebServer
RUN npm install
RUN tsc
RUN dotnet new tool-manifest --force
RUN dotnet tool install Cake.Tool --version 5.0.0
WORKDIR /src

RUN dotnet publish src/Web/DatenMeister.WebServer/DatenMeister.WebServer.csproj \
    -c Release \
    -o /app/publish \
    --no-self-contained

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000

EXPOSE 5000

ENTRYPOINT ["dotnet", "DatenMeister.WebServer.dll"]
