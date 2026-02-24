# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy everything from the RestaurauntApp folder
COPY RestaurauntApp/. ./RestaurauntApp/

# Restore packages
WORKDIR /source/RestaurauntApp
RUN dotnet restore RestaurauntApp.sln

# Build & publish
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RestaurauntApp.dll"]
