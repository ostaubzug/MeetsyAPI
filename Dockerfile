FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8443

# Create ssl directory with correct permissions
RUN mkdir -p /app/ssl && \
    chown -R app:app /app/ssl && \
    chmod 755 /app/ssl

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MeetsyAPI/MeetsyAPI.csproj", "MeetsyAPI/"]
RUN dotnet restore "MeetsyAPI/MeetsyAPI.csproj"
COPY . .
WORKDIR "/src/MeetsyAPI"
RUN dotnet build "MeetsyAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MeetsyAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER app
ENTRYPOINT ["dotnet", "MeetsyAPI.dll"]