FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["VotoElectronico.Seguro/VotoElectronico.Seguro.csproj", "VotoElectronico.Seguro/"]
COPY ["ProyectoVotoElectronico/ProyectoVotoElectronico.csproj", "ProyectoVotoElectronico/"]

RUN dotnet restore "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj"
RUN dotnet restore "ProyectoVotoElectronico/ProyectoVotoElectronico.csproj"

COPY . .

WORKDIR "/src/ProyectoVotoElectronico"
RUN dotnet build -c Release

WORKDIR "/src/VotoElectronico.Seguro"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "VotoElectronico.Seguro.dll"]