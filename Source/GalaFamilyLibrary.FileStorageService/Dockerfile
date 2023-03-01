
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GalaFamilyLibrary.FileStorageService/GalaFamilyLibrary.FileStorageService.csproj", "GalaFamilyLibrary.FileStorageService/"]
COPY ["GalaFamilyLibrary.Infrastructure/GalaFamilyLibrary.Infrastructure.csproj", "GalaFamilyLibrary.Infrastructure/"]
RUN dotnet restore "GalaFamilyLibrary.FileStorageService/GalaFamilyLibrary.FileStorageService.csproj"
COPY . .
WORKDIR "/src/GalaFamilyLibrary.FileStorageService"
RUN dotnet build "GalaFamilyLibrary.FileStorageService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GalaFamilyLibrary.FileStorageService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaFamilyLibrary.FileStorageService.dll"]