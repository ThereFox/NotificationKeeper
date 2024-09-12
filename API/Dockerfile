# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API/API.csproj", "API/"]
COPY ["App/App.csproj", "App/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure.Logging.InfluexDB/Persistense.Logging.InfluxDB.csproj", "Infrastructure.Logging.InfluexDB/"]
COPY ["Infrastructure.MessageBrocker.ConsumerService/Infrastructure.Brocker.Kafka.Consumer.Service.csproj", "Infrastructure.MessageBrocker.ConsumerService/"]
COPY ["Infrastructure.MessageBrocker.Consumer/Infrastructure.Brocker.Kafka.Consumer.csproj", "Infrastructure.MessageBrocker.Consumer/"]
COPY ["Infrastructure.MessageBrocker/Infrastructure.Brocker.Kafka.Producer.csproj", "Infrastructure.MessageBrocker/"]
COPY ["Persistense.Cache.Notifications/Persistense.Cache.Notifications.csproj", "Persistense.Cache.Notifications/"]
COPY ["Persistense/Persistense.EF.Notifications.csproj", "Persistense/"]
RUN dotnet restore "./API/API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "./API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]