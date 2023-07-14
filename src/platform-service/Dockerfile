FROM mcr.microsoft.com/dotnet/aspnet:6.0.20-jammy-arm64v8 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0.412-jammy-arm64v8 AS build
WORKDIR /src
COPY Mng.PlatformService/Mng.PlatformService.csproj Mng.PlatformService/
RUN dotnet restore Mng.PlatformService/Mng.PlatformService.csproj
COPY . .
RUN dotnet build Mng.PlatformService/Mng.PlatformService.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Mng.PlatformService/Mng.PlatformService.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mng.PlatformService.dll"]
