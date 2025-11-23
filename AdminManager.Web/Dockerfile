# ETAPA 1: Base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
# Configuramos el puerto 5041 internamente
ENV ASPNETCORE_HTTP_PORTS=5041
EXPOSE 5041

# ETAPA 2: Build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- OJO: Reemplaza 'TuProyectoRazor' con el nombre real de tu carpeta/proyecto ---
COPY ["AdminManager.Web/AdminManager.Web.csproj", "AdminManager.Web/"]
RUN dotnet restore "AdminManager.Web/AdminManager.Web.csproj"

COPY . .
WORKDIR "/src/AdminManager.Web"
RUN dotnet build "AdminManager.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ETAPA 3: Publicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AdminManager.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ETAPA 4: Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminManager.Web.dll"]