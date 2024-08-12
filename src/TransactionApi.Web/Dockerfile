FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["src/TransactionApi.Web/TransactionApi.Web.csproj", "src/TransactionApi.Web/"]
COPY ["src/TransactionApi.Infrastructure/TransactionApi.Infrastructure.csproj", "src/TransactionApi.Infrastructure/"]
COPY ["src/TransactionApi.Domain/TransactionApi.Domain.csproj", "src/TransactionApi.Domain/"]
COPY ["src/TransactionApi.Application/TransactionApi.Application.csproj", "src/TransactionApi.Application/"]
COPY ["src/TransactionApi.Presentation/TransactionApi.Presentation.csproj", "src/TransactionApi.Presentation/"]
RUN dotnet restore "src/TransactionApi.Web/TransactionApi.Web.csproj"

COPY . .
WORKDIR "/src/src/TransactionApi.Web"
RUN dotnet build "TransactionApi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "TransactionApi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /app/build/TransactionWeb.Presentation.xml /app/
ENTRYPOINT ["dotnet", "TransactionApi.Web.dll"]


