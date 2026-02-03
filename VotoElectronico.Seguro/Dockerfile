FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiamos la solución y los proyectos para restaurar dependencias
# Esto asegura que todas las rutas relativas funcionen
COPY ["VotoElectronico.Seguro/VotoElectronico.Seguro.csproj", "VotoElectronico.Seguro/"]
COPY ["ProyectoVotoElectronico/ProyectoVotoElectronico.csproj", "ProyectoVotoElectronico/"]

# 2. Restauramos el proyecto principal (esto restaura automáticamente las dependencias locales)
RUN dotnet restore "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj"

# 3. Copiamos TODO el contenido del repositorio
COPY . .

# 4. Publicamos directamente el proyecto Web
# Al estar en la raíz (/src), .NET encontrará "ProyectoVotoElectronico" sin problemas
WORKDIR "/src/VotoElectronico.Seguro"
RUN dotnet publish "VotoElectronico.Seguro.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Imagen de Ejecución ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Render usa el puerto 10000 por defecto
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "VotoElectronico.Seguro.dll"]