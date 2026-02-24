# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy solution and project files
COPY *.sln .
COPY RestaurauntApp/*.csproj ./RestaurauntApp/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Publish the app
WORKDIR /source/RestaurauntApp
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Configure Kestrel to listen on port 10000
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

# Start the app
ENTRYPOINT ["dotnet", "RestaurauntApp.dll"]
