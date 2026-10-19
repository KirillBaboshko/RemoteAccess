FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files first for layer-cached restore
COPY ["nuget.config", "./"]
COPY ["RemoteAccess.Api/RemoteAccess.Api.csproj", "RemoteAccess.Api/"]
COPY ["RemoteAccess.Application/RemoteAccess.Application.csproj", "RemoteAccess.Application/"]
COPY ["RemoteAccess.Data/RemoteAccess.Data.csproj", "RemoteAccess.Data/"]
COPY ["RemoteAccess.Domain/RemoteAccess.Domain.csproj", "RemoteAccess.Domain/"]
COPY ["RemoteAccess.Infrastructure/RemoteAccess.Infrastructure.csproj", "RemoteAccess.Infrastructure/"]

RUN dotnet restore "RemoteAccess.Api/RemoteAccess.Api.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "RemoteAccess.Api/RemoteAccess.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# aspnet base image listens on 8080 by default (non-root port)
EXPOSE 8080
ENTRYPOINT ["dotnet", "RemoteAccess.Api.dll"]
