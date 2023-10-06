# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the solution and restore any dependencies
COPY ["SimpleMediaLibrary.sln.sln", "./"]
COPY ["Player/Player.csproj", "Player/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["DataAccessService/DataAccessService.csproj", "DataAccessService/"]
RUN dotnet restore "YourSolution.sln"

# Copy everything else and build
COPY . .
WORKDIR "/src/Player"
RUN dotnet build "Player.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Player.csproj" -c Release -o /app/publish

# Copy the build app to the base image and define entrypoint
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Player.dll"]
