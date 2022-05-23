FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY StudyBuddy.Model StudyBuddy.Model
COPY StudyBuddy.Persistence StudyBuddy.Persistence
COPY StudyBuddy.Api StudyBuddy.Api
COPY StudyBuddy.BusinessLogic StudyBuddy.BusinessLogic 
RUN dotnet restore StudyBuddy.Api/StudyBuddy.Api.csproj

# Copy everything else and build
RUN dotnet publish -c Release -o out ./StudyBuddy.Api/StudyBuddy.Api.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev 

WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "StudyBuddy.Api.dll"]