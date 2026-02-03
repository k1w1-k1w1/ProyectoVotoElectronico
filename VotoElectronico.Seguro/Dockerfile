FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiamos los archivos de proyecto (.csproj) de AMBOS proyectos
COPY ["VotoElectronico.Seguro/VotoElectronico.Seguro.csproj", "VotoElectronico.Seguro/"]
COPY ["ProyectoVotoElectronico/ProyectoVotoElectronico.csproj", "ProyectoVotoElectronico/"]

# 2. RESTAURAMOS AMBOS (Paso crítico que faltaba)
RUN dotnet restore "ProyectoVotoElectronico/ProyectoVotoElectronico.csproj"
RUN dotnet restore "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj"

# 3. Ahora copiamos el resto del código fuente
COPY . .

# 4. Compilamos primero la biblioteca de clases para generar la DLL
WORKDIR "/src/ProyectoVotoElectronico"
RUN dotnet build "ProyectoVotoElectronico.csproj" -c Release

# 5. Publicamos el proyecto Web
WORKDIR "/src/VotoElectronico.Seguro"
RUN dotnet publish "VotoElectronico.Seguro.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Etapa Final ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "VotoElectronico.Seguro.dll"]