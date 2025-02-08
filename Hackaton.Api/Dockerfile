FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Hackaton.Api/Hackaton.Api.csproj", "Hackaton.Api/"]
COPY ["Hackaton.Infra.Ioc/Hackaton.Infra.Ioc.csproj", "Hackaton.Infra.Ioc/"]
COPY ["Hackaton.Infra/Hackaton.Infra.csproj", "Hackaton.Infra/"]
COPY ["Hackaton.Domain/Hackaton.Domain.csproj", "Hackaton.Domain/"]
RUN dotnet restore "Hackaton.Api/Hackaton.Api.csproj"
COPY . .
WORKDIR "/src/Hackaton.Api"
RUN dotnet build "Hackaton.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Hackaton.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hackaton.Api.dll"]
