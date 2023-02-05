FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GalaFamilyLibrary.Gateway/GalaFamilyLibrary.Gateway.csproj", "GalaFamilyLibrary.Gateway/"]
RUN dotnet restore "GalaFamilyLibrary.Gateway/GalaFamilyLibrary.Gateway.csproj"
COPY . .
WORKDIR "/src/GalaFamilyLibrary.Gateway"
RUN dotnet build "GalaFamilyLibrary.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GalaFamilyLibrary.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaFamilyLibrary.Gateway.dll"]
