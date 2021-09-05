FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY StudyBuddy.Model StudyBuddy.Model
COPY StudyBuddy.Persistence StudyBuddy.Persistence
COPY StudyBuddy.Api StudyBuddy.Api
RUN dotnet restore StudyBuddy.Api/StudyBuddy.Api.csproj

# Copy everything else and build
RUN dotnet publish -c Release -o out ./StudyBuddy.Api/StudyBuddy.Api.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "StudyBuddy.Api.dll"]