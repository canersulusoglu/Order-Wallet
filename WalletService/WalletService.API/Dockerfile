#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WalletService/WalletService.API/WalletService.API.csproj", "WalletService/WalletService.API/"]
RUN dotnet restore "WalletService/WalletService.API/WalletService.API.csproj"
COPY . .
WORKDIR "/src/WalletService/WalletService.API"
RUN dotnet build "WalletService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WalletService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WalletService.API.dll"]