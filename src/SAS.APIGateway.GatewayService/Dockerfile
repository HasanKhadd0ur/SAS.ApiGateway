#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/SAS.APIGateway.GatewayService/SAS.APIGateway.GatewayService.csproj", "src/SAS.APIGateway.GatewayService/"]
RUN dotnet restore "src/SAS.APIGateway.GatewayService/SAS.APIGateway.GatewayService.csproj"
COPY . .
WORKDIR "/src/src/SAS.APIGateway.GatewayService"
RUN dotnet build "SAS.APIGateway.GatewayService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SAS.APIGateway.GatewayService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SAS.APIGateway.GatewayService.dll"]