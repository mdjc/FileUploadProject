# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY FileUploadApi/*.csproj ./FileUploadApi/
RUN dotnet restore ./FileUploadApi/FileUploadApi.csproj

# Copiar el resto del código y compilar
COPY FileUploadApi/ ./FileUploadApi/
WORKDIR /src/FileUploadApi
RUN dotnet publish -c Release -o /app/publish

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FileUploadApi.dll"]
