# Use the official .NET 9.0 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Use the official .NET 9.0 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["HRMS.WebUI/HRMS.WebUI.csproj", "HRMS.WebUI/"]
COPY ["HRMS.Infrastructure/HRMS.Infrastructure.csproj", "HRMS.Infrastructure/"]
COPY ["HRMS.Application/HRMS.Application.csproj", "HRMS.Application/"]
COPY ["HRMS.Domain/HRMS.Domain.csproj", "HRMS.Domain/"]

# Restore dependencies
RUN dotnet restore "HRMS.WebUI/HRMS.WebUI.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/HRMS.WebUI"
RUN dotnet build "HRMS.WebUI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "HRMS.WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app

# Copy the published application
COPY --from=publish /app/publish .

# Create directory for uploaded documents
RUN mkdir -p /app/wwwroot/EmployeeDocs

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:5000/swagger/index.html || exit 1

ENTRYPOINT ["dotnet", "HRMS.WebUI.dll"]
