FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GalaFamilyLibrary.ParameterService/GalaFamilyLibrary.ParameterService.csproj", "GalaFamilyLibrary.ParameterService/"]
COPY ["GalaFamilyLibrary.Infrastructure/GalaFamilyLibrary.Infrastructure.csproj", "GalaFamilyLibrary.Infrastructure/"]
COPY ["GalaFamilyLibrary.DataTransferObject/GalaFamilyLibrary.DataTransferObject.csproj", "GalaFamilyLibrary.DataTransferObject/"]
COPY ["GalaFamilyLibrary.Model/GalaFamilyLibrary.Model.csproj", "GalaFamilyLibrary.Model/"]
COPY ["GalaFamilyLibrary.Service/GalaFamilyLibrary.Service.csproj", "GalaFamilyLibrary.Service/"]
RUN dotnet restore "GalaFamilyLibrary.ParameterService/GalaFamilyLibrary.ParameterService.csproj"
COPY . .
WORKDIR "/src/GalaFamilyLibrary.ParameterService"
RUN dotnet build "GalaFamilyLibrary.ParameterService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GalaFamilyLibrary.ParameterService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
VOLUME ["/app/logs"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaFamilyLibrary.ParameterService.dll"]
