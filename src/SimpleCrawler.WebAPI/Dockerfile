FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SimpleCrawler.Engine/src/SimpleCrawler.WebAPI/SimpleCrawler.WebAPI.csproj", "SimpleCrawler.WebAPI/"]
RUN dotnet restore "SimpleCrawler.Engine/src/SimpleCrawler.WebAPI/SimpleCrawler.WebAPI.csproj"
COPY . .
WORKDIR "/src/SimpleCrawler.WebAPI"
RUN dotnet build "SimpleCrawler.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleCrawler.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleCrawler.WebAPI.dll"]
