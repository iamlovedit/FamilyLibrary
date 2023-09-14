FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY ["GalaFamilyLibrary.FileStorageService/GalaFamilyLibrary.FileStorageService.csproj", "GalaFamilyLibrary.FileStorageService/"]
COPY ["GalaFamilyLibrary.Infrastructure/GalaFamilyLibrary.Infrastructure.csproj", "GalaFamilyLibrary.Infrastructure/"]
COPY . .
WORKDIR "/src/GalaFamilyLibrary.FileStorageService"
RUN dotnet publish "GalaFamilyLibrary.FileStorageService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
VOLUME ["/app/files"]
COPY ["GalaFamilyLibrary.FileStorageService/files","/app/files"] 
ENTRYPOINT ["dotnet", "GalaFamilyLibrary.FileStorageService.dll"]