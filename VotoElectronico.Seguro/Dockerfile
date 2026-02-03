# 1. Etapa de Construcción (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["VotoElectronico.Seguro/VotoElectronico.Seguro.csproj", "VotoElectronico.Seguro/"]
COPY ["ProyectoVotoElectronico/ProyectoVotoElectronico.csproj", "ProyectoVotoElectronico/"]

# Restauramos las dependencias de ambos proyectos
RUN dotnet restore "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj"
RUN dotnet restore "ProyectoVotoElectronico/ProyectoVotoElectronico.csproj"

# Copiamos el resto de los archivos del código fuente
COPY . .

# Construimos la biblioteca de modelos primero para asegurar que la DLL exista
WORKDIR "/src/ProyectoVotoElectronico"
RUN dotnet build "ProyectoVotoElectronico.csproj" -c Release

# Publicamos el proyecto Web (el Front-end)
WORKDIR "/src/VotoElectronico.Seguro"
RUN dotnet publish "VotoElectronico.Seguro.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Etapa Final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiamos solo los archivos publicados desde la etapa de construcción
COPY --from=build /app/publish .

# Configuramos el puerto que usa Render (10000)
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Iniciamos la aplicación usando el DLL del proyecto Web
ENTRYPOINT ["dotnet", "VotoElectronico.Seguro.dll"]