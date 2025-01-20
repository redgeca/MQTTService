# Consultez https://aka.ms/customizecontainer pour savoir comment personnaliser votre conteneur de débogage et comment Visual Studio utilise ce Dockerfile pour générer vos images afin d’accélérer le débogage.

# Cet index est utilisé lors de l’exécution à partir de VS en mode rapide (par défaut pour la configuration de débogage)
FROM mcr.microsoft.com/dotnet/aspnet:8.0.1-jammy-arm64v8 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Cette étape permet de publier le projet de service à copier dans la phase finale
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AzureIoTServer/AzureIoTServer.csproj", "AzureIoTServer/"]
RUN dotnet restore "./AzureIoTServer/AzureIoTServer.csproj"
COPY . .
WORKDIR "/src/AzureIoTServer"
RUN dotnet build "./AzureIoTServer.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "AzureIoTServer.csproj" -c Release -o /app/publish /p:UseAppHost=true --runtime linux-arm64 --self-contained

# Cette phase est utilisée pour générer le projet de service
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureIoTServer.dll"]

