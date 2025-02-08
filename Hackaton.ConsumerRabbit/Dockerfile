FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Hackaton.ConsumerRabbit/Hackaton.ConsumerRabbit.csproj", "Hackaton.ConsumerRabbit/"]
RUN dotnet restore "Hackaton.ConsumerRabbit/Hackaton.ConsumerRabbit.csproj"
COPY . .
WORKDIR "/src/Hackaton.ConsumerRabbit"
RUN dotnet build "Hackaton.ConsumerRabbit.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Hackaton.ConsumerRabbit.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hackaton.ConsumerRabbit.dll"]
