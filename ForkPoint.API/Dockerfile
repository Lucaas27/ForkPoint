# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["ForkPoint.API/ForkPoint.API.csproj", "ForkPoint.API/"]
COPY ["ForkPoint.Application/ForkPoint.Application.csproj", "ForkPoint.Application/"]
COPY ["ForkPoint.Domain/ForkPoint.Domain.csproj", "ForkPoint.Domain/"]
COPY ["ForkPoint.Infrastructure/ForkPoint.Infrastructure.csproj", "ForkPoint.Infrastructure/"]
RUN dotnet restore "ForkPoint.API/ForkPoint.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/ForkPoint.API"
RUN dotnet build "ForkPoint.API.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "ForkPoint.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install curl so healthchecks that call curl succeed
RUN apt-get update \
	&& apt-get install -y --no-install-recommends curl \
	&& rm -rf /var/lib/apt/lists/*

# Expose port 80
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "ForkPoint.API.dll"]