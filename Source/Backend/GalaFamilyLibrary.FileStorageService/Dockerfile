FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GalaFamilyLibrary.FileStorageService/GalaFamilyLibrary.FileStorageService.csproj", "GalaFamilyLibrary.FileStorageService/"]
COPY ["GalaFamilyLibrary.Infrastructure/GalaFamilyLibrary.Infrastructure.csproj", "GalaFamilyLibrary.Infrastructure/"]
COPY ["GalaFamilyLibrary.DataTransferObject/GalaFamilyLibrary.DataTransferObject.csproj", "GalaFamilyLibrary.DataTransferObject/"]
RUN dotnet restore "GalaFamilyLibrary.FileStorageService/GalaFamilyLibrary.FileStorageService.csproj"
COPY . .
WORKDIR "/src/GalaFamilyLibrary.FileStorageService"
RUN dotnet build "GalaFamilyLibrary.FileStorageService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GalaFamilyLibrary.FileStorageService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
VOLUME ["/app/logs"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaFamilyLibrary.FileStorageService.dll"]
