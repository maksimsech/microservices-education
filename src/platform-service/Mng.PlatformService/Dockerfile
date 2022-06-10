FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# TODO: move to sln and fix paths
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mng.PlatformService.csproj", "."]
RUN dotnet restore "Mng.PlatformService.csproj"
COPY . .
RUN dotnet build "Mng.PlatformService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mng.PlatformService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mng.PlatformService.dll"]
