FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiamos los archivos de proyecto (.csproj)
COPY ["VotoElectronico.Seguro/VotoElectronico.Seguro.csproj", "VotoElectronico.Seguro/"]
COPY ["ProyectoVotoElectronico/ProyectoVotoElectronico.csproj", "ProyectoVotoElectronico/"]

# 2. Restauramos las dependencias
RUN dotnet restore "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj"

# 3. Copiamos TODO el código fuente
COPY . .

# 4. COMPILAMOS LA SOLUCIÓN COMPLETA (Sin entrar a subcarpetas)
# Esto obliga a .NET a resolver las referencias desde el nivel superior
RUN dotnet build "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj" -c Release

# 5. Publicamos
RUN dotnet publish "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Etapa Final (Runtime) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "VotoElectronico.Seguro.dll"]