FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY backend.sln backend.sln
COPY model model
COPY persistence persistence
COPY admin admin
COPY services services
RUN dotnet restore admin/admin.csproj

# Copy everything else and build
RUN dotnet publish -c Release -o out ./admin/admin.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "admin.dll"]