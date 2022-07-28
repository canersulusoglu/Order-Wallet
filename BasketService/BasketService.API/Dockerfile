#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BasketService/BasketService.API/BasketService.API.csproj", "BasketService/BasketService.API/"]
COPY ["EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "EventBus/EventBus.RabbitMQ/"]
RUN dotnet restore "BasketService/BasketService.API/BasketService.API.csproj"
COPY . .
WORKDIR "/src/BasketService/BasketService.API"
RUN dotnet build "BasketService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasketService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasketService.API.dll"]