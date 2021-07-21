FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Model Model
COPY Persistence Persistence
COPY Admin Admin
RUN dotnet restore Admin/Admin.csproj

# Copy everything else and build
RUN dotnet publish -c Release -o out ./Admin/Admin.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Admin.dll"]