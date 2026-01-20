FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore FiapCloudGames.Users/FiapCloudGames.Users.Api/FiapCloudGames.Users.Api.csproj
RUN dotnet publish FiapCloudGames.Users/FiapCloudGames.Users.Api/FiapCloudGames.Users.Api.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FiapCloudGames.Users.Api.dll"]
