# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app

# Copy csproj and restore
COPY GestionOrdinateurs.API/GestionOrdinateurs.API.csproj ./GestionOrdinateurs.API/
COPY GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj ./GestionOrdinateurs.Shared/

RUN dotnet restore ./GestionOrdinateurs.API/GestionOrdinateurs.API.csproj

# Copy source code
COPY GestionOrdinateurs.API/ ./GestionOrdinateurs.API/
COPY GestionOrdinateurs.Shared/ ./GestionOrdinateurs.Shared/

RUN dotnet publish ./GestionOrdinateurs.API/GestionOrdinateurs.API.csproj \
    -c Release \
    -o /app/out

# =========================
# Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/out ./

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5187

EXPOSE 5187

ENTRYPOINT ["dotnet", "GestionOrdinateurs.API.dll"]