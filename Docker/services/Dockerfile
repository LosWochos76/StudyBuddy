FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Model Model
COPY Persistence Persistence
COPY Services Services
RUN dotnet restore Services/Services.csproj

# Copy everything else and build
RUN dotnet publish -c Release -o out ./Services/Services.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Services.dll"]