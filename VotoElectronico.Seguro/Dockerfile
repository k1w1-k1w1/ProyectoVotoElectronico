FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos todo el contenido del repositorio
COPY . .

# Restauramos y publicamos desde la raíz
RUN dotnet restore "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj"
RUN dotnet publish "VotoElectronico.Seguro/VotoElectronico.Seguro.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Imagen Final ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "VotoElectronico.Seguro.dll"]