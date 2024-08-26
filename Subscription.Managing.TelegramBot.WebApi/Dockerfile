#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Subscription.Managing.TelegramBot.WebApi/Subscription.Managing.TelegramBot.WebApi.csproj", "Subscription.Managing.TelegramBot.WebApi/"]
COPY ["Subscription.Managing.TelegramBot.Application.Contracts/Subscription.Managing.TelegramBot.Application.Contracts.csproj", "Subscription.Managing.TelegramBot.Application.Contracts/"]
COPY ["Subscription.Managing.TelegramBot.Infrastructure/Subscription.Managing.TelegramBot.Infrastructure.csproj", "Subscription.Managing.TelegramBot.Infrastructure/"]
COPY ["Subscription.Managing.TelegramBot.Application/Subscription.Managing.TelegramBot.Application.csproj", "Subscription.Managing.TelegramBot.Application/"]
COPY ["Subscription.Managing.TelegramBot.Domain/Subscription.Managing.TelegramBot.Domain.csproj", "Subscription.Managing.TelegramBot.Domain/"]
RUN dotnet restore "./Subscription.Managing.TelegramBot.WebApi/Subscription.Managing.TelegramBot.WebApi.csproj"
COPY . .
WORKDIR "/src/Subscription.Managing.TelegramBot.WebApi"
RUN dotnet build "./Subscription.Managing.TelegramBot.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Subscription.Managing.TelegramBot.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Subscription.Managing.TelegramBot.WebApi.dll"]