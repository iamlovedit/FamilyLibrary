FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY ["GalaFamilyLibrary.ParameterService/GalaFamilyLibrary.ParameterService.csproj", "GalaFamilyLibrary.ParameterService/"]
COPY ["GalaFamilyLibrary.Infrastructure/GalaFamilyLibrary.Infrastructure.csproj", "GalaFamilyLibrary.Infrastructure/"]
COPY ["GalaFamilyLibrary.Domain/GalaFamilyLibrary.Domain.csproj", "GalaFamilyLibrary.Domain/"]
COPY . .
WORKDIR "/src/GalaFamilyLibrary.ParameterService"
RUN dotnet publish "GalaFamilyLibrary.ParameterService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
VOLUME ["/app/logs"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaFamilyLibrary.ParameterService.dll"]