#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GalaFamilyLibrary.FamilyService/GalaFamilyLibrary.FamilyService.csproj", "GalaFamilyLibrary.FamilyService/"]
COPY ["GalaFamilyLibrary.Infrastructure/GalaFamilyLibrary.Infrastructure.csproj", "GalaFamilyLibrary.Infrastructure/"]
RUN dotnet restore "GalaFamilyLibrary.FamilyService/GalaFamilyLibrary.FamilyService.csproj"
COPY . .
WORKDIR "/src/GalaFamilyLibrary.FamilyService"
RUN dotnet build "GalaFamilyLibrary.FamilyService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GalaFamilyLibrary.FamilyService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaFamilyLibrary.FamilyService.dll"]