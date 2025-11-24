# Imagen base para construir
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Ubicamos la solucion completa (para resolver referencias)
WORKDIR /src

# Copiamos solo los csproj primero para optimizar cache
COPY Firmeza.API/*.csproj Firmeza.API/
COPY AdminManager.Web/*.csproj AdminManager.Web/
COPY Firmeza.Tests/*.csproj Firmeza.Tests/

# Restauramos dependencias
RUN dotnet restore Firmeza.Tests/Firmeza.Tests.csproj

# Ahora copiamos todo el código
COPY . .

# Compilamos
RUN dotnet build Firmeza.Tests/Firmeza.Tests.csproj -c Release --no-restore

# Imagen final para ejecutar pruebas
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS tests
WORKDIR /src

COPY --from=build /src .

# Ejecutar pruebas. Si alguna falla, el contenedor terminará con código distinto de 0.
ENTRYPOINT ["dotnet", "test", "Firmeza.Tests/Firmeza.Tests.csproj", "--logger:trx", "--results-directory:/testresults"]
