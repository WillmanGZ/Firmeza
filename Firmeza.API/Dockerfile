# Esta fase se usa cuando se ejecuta desde VS en modo rápido
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

# --- CAMBIOS REALIZADOS AQUÍ ---
# 1. Definimos la variable de entorno para que .NET 9 use el puerto 5152
ENV ASPNETCORE_HTTP_PORTS=5152

# 2. Exponemos el puerto 5152 (y quitamos el 8080/8081 por defecto)
EXPOSE 5152
# -------------------------------

# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Firmeza.API/Firmeza.API.csproj", "Firmeza.API/"]
RUN dotnet restore "./Firmeza.API/Firmeza.API.csproj"
COPY . .
WORKDIR "/src/Firmeza.API"
RUN dotnet build "./Firmeza.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Firmeza.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o modo normal
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Firmeza.API.dll"]