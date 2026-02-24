# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy solution and csproj
COPY *.sln .
COPY RestaurauntApp/*.csproj ./RestaurauntApp/

# Restore packages
RUN dotnet restore RestaurauntApp.sln

# Copy everything else
COPY RestaurauntApp/. ./RestaurauntApp/

# Build
WORKDIR /source/RestaurauntApp
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RestaurauntApp.dll"]
