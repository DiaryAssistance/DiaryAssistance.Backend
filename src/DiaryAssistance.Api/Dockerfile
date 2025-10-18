FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DiaryAssistance.Api/DiaryAssistance.Api.csproj", "src/DiaryAssistance.Api/"]
COPY ["src/DiaryAssistance.Application/DiaryAssistance.Application.csproj", "src/DiaryAssistance.Application/"]
COPY ["src/DiaryAssistance.Core/DiaryAssistance.Core.csproj", "src/DiaryAssistance.Core/"]
COPY ["src/DiaryAssistance.Persistence/DiaryAssistance.Persistence.csproj", "src/DiaryAssistance.Persistence/"]
COPY ["src/DiaryAssistance.Infrastructure/DiaryAssistance.Infrastructure.csproj", "src/DiaryAssistance.Infrastructure/"]
RUN dotnet restore "src/DiaryAssistance.Api/DiaryAssistance.Api.csproj"
COPY . .
WORKDIR "/src/src/DiaryAssistance.Api"
RUN dotnet build "./DiaryAssistance.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DiaryAssistance.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiaryAssistance.Api.dll"]
