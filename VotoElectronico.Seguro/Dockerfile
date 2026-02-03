FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiamos los archivos de proyecto
# Ponemos ambos al mismo nivel inicial para que se reconozcan
COPY ["VotoElectronico.Seguro/VotoElectronico.Seguro.csproj", "VotoElectronico.Seguro/"]
COPY ["ProyectoVotoElectronico/ProyectoVotoElectronico.csproj", "ProyectoVotoElectronico/"]

# 2. Restauramos
RUN dotnet restore "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj"

# 3. Copiamos TODO el código fuente del repositorio a /src
COPY . .

# 4. PASO CLAVE: Compilamos la solución completa desde la RAÍZ 
# En lugar de entrar a carpetas, compilamos desde donde se ve todo
RUN dotnet build "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj" -c Release

# 5. Publicamos
RUN dotnet publish "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Etapa Final ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "VotoElectronico.Seguro.dll"]