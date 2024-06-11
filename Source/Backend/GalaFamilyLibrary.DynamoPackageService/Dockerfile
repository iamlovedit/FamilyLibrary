FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY ["GalaFamilyLibrary.DynamoPackageService/GalaFamilyLibrary.DynamoPackageService.csproj", "GalaFamilyLibrary.DynamoPackageService/"]
COPY ["GalaFamilyLibrary.Infrastructure/GalaFamilyLibrary.Infrastructure.csproj", "GalaFamilyLibrary.Infrastructure/"]
COPY . .
WORKDIR "/src/GalaFamilyLibrary.DynamoPackageService"
RUN dotnet publish "GalaFamilyLibrary.DynamoPackageService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
VOLUME ["/app/logs"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaFamilyLibrary.DynamoPackageService.dll"]