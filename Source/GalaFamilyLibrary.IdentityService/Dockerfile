#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GalaFamilyLibrary.IdentityService/GalaFamilyLibrary.IdentityService.csproj", "GalaFamilyLibrary.IdentityService/"]
COPY ["GalaFamilyLibrary.Infrastructure/GalaFamilyLibrary.Infrastructure.csproj", "GalaFamilyLibrary.Infrastructure/"]
RUN dotnet restore "GalaFamilyLibrary.IdentityService/GalaFamilyLibrary.IdentityService.csproj"
COPY . .
WORKDIR "/src/GalaFamilyLibrary.IdentityService"
RUN dotnet build "GalaFamilyLibrary.IdentityService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GalaFamilyLibrary.IdentityService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaFamilyLibrary.IdentityService.dll"]