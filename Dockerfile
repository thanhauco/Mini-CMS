FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY src/MiniCMS.Domain/MiniCMS.Domain.csproj src/MiniCMS.Domain/
COPY src/MiniCMS.Infrastructure/MiniCMS.Infrastructure.csproj src/MiniCMS.Infrastructure/
COPY src/MiniCMS.Api/MiniCMS.Api.csproj src/MiniCMS.Api/
RUN dotnet restore src/MiniCMS.Api/MiniCMS.Api.csproj

# Copy source code and build
COPY src/ src/
RUN dotnet build src/MiniCMS.Api/MiniCMS.Api.csproj -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish src/MiniCMS.Api/MiniCMS.Api.csproj -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

# Create uploads directory
RUN mkdir -p /app/uploads

EXPOSE 80
ENTRYPOINT ["dotnet", "MiniCMS.Api.dll"]
