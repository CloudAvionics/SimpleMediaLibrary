FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Player/Player.csproj", "Player/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["DataAccessService/DataAccessService.csproj", "DataAccessService/"]
RUN dotnet restore "Player/Player.csproj"
COPY . .
WORKDIR "/src/Player"
RUN dotnet build "Player.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Player.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS="http://+:80;https://+:443"
RUN mkdir -p /app/Data  # Create Data directory
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Player.dll"]