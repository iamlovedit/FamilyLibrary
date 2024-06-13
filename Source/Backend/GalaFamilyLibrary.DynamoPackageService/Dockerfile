FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GalaFamilyLibrary.DynamoPackageService/GalaFamilyLibrary.DynamoPackageService.csproj", "GalaFamilyLibrary.DynamoPackageService/"]
COPY ["GalaFamilyLibrary.Infrastructure/GalaFamilyLibrary.Infrastructure.csproj", "GalaFamilyLibrary.Infrastructure/"]
COPY ["GalaFamilyLibrary.DataTransferObject/GalaFamilyLibrary.DataTransferObject.csproj", "GalaFamilyLibrary.DataTransferObject/"]
COPY ["GalaFamilyLibrary.Model/GalaFamilyLibrary.Model.csproj", "GalaFamilyLibrary.Model/"]
COPY ["GalaFamilyLibrary.Repository/GalaFamilyLibrary.Repository.csproj", "GalaFamilyLibrary.Repository/"]
COPY ["GalaFamilyLibrary.Service/GalaFamilyLibrary.Service.csproj", "GalaFamilyLibrary.Service/"]
RUN dotnet restore "GalaFamilyLibrary.DynamoPackageService/GalaFamilyLibrary.DynamoPackageService.csproj"
COPY . .
WORKDIR "/src/GalaFamilyLibrary.DynamoPackageService"
RUN dotnet build "GalaFamilyLibrary.DynamoPackageService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GalaFamilyLibrary.DynamoPackageService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
VOLUME ["/app/logs"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaFamilyLibrary.DynamoPackageService.dll"]
