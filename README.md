# ForkPoint

ForkPoint is a restaurant management API built using Clean Architecture principles in ASP.NET Core. It provides a platform for managing restaurants, menu items, user authentication, and administrative operations. The application follows modern software development practices, including CQRS with MediatR, dependency injection, and validation.

Fully containerised with Docker and optimised for deployment on Coolify (self-hosted Vercel alternative).

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Installation and Setup](#installation-and-setup)
- [Configuration](#configuration)
- [Docker](#docker)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Testing](#testing)
- [Database](#database)
- [Authentication and Authorisation](#authentication-and-authorisation)

## Features

- **Restaurant Management**: Create, read, update, and delete restaurants with full CRUD operations.
- **Menu Item Management**: Manage menu items associated with restaurants.
- **User Authentication**: JWT-based authentication with support for local login and Google OAuth.
- **Role-Based Authorisation**: Admin and owner roles with specific permissions.
- **Email Services**: Email confirmation, password reset, and notifications using MailKit.
- **Validation**: FluentValidation for request validation.
- **Logging**: Structured logging with Serilog.
- **API Documentation**: Swagger/OpenAPI integration for interactive API exploration.
- **Database Seeding**: Automated seeding of initial data.
- **Middleware**: Custom middleware for error handling, elapsed time logging, and sensitive data protection.

## Architecture

ForkPoint follows Clean Architecture principles, separating concerns into distinct layers:

- **ForkPoint.API**: Presentation layer containing controllers, middleware, and API configuration.
- **ForkPoint.Application**: Application layer with business logic, handlers (CQRS), services, and validation.
- **ForkPoint.Domain**: Domain layer containing entities, value objects, and business rules.
- **ForkPoint.Infrastructure**: Infrastructure layer handling data persistence, external services, and cross-cutting concerns.

The architecture helps with:
- Separation of concerns
- Dependency inversion
- Testability
- Maintainability

## Technologies Used

- **Framework**: .NET 8.0
- **Web Framework**: ASP.NET Core Web API
- **Database**: PostgreSQL with Entity Framework Core
- **ORM**: Entity Framework Core 8.0
- **CQRS**: MediatR
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Authentication**: JWT Bearer Tokens, Google OAuth
- **Email**: MailKit
- **Logging**: Serilog
- **Documentation**: Swagger/OpenAPI
- **Testing**: xUnit (assumed based on test project structure)
- **Dependency Injection**: Built-in ASP.NET Core DI container

## Prerequisites

- .NET 8.0 SDK
- PostgreSQL (or Docker for containerised setup)
- Docker and Docker Compose (for containerised deployment)
- Visual Studio 2022 or Visual Studio Code with C# extensions
- Git

## Installation and Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Lucaas27/ForkPoint.git
   cd ForkPoint
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the solution**:
   ```bash
   dotnet build
   ```

## Configuration

The application uses `appsettings.json` for configuration. Key settings include:

- **Database Connection**: Configure the PostgreSQL connection string in `appsettings.json` or user secrets.
- **JWT Settings**: Configure JWT issuer, audience, and secret key.
- **Email Settings**: Configure SMTP server details for email services.
- **Google OAuth**: Configure client ID and secret for Google authentication.

Example `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ForkPoint;Username=postgres;Password=password"
  },
  "Jwt": {
    "Issuer": "ForkPoint",
    "Audience": "ForkPointUsers",
    "SecretKey": "your-secret-key-here"
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  },
  "Authentication": {
    "Google": {
      "ClientId": "your-google-client-id",
      "ClientSecret": "your-google-client-secret"
    }
  }
}
```

For sensitive data, use user secrets:
```bash
dotnet user-secrets set "Jwt:SecretKey" "your-secret-key"
```

## Docker

ForkPoint can be run using Docker and Docker Compose for easy deployment and development.

### Prerequisites for Docker
- Docker
- Docker Compose

### Running Locally with Docker Compose

1. **Build and start the services**:
   ```bash
   docker-compose up --build
   ```

2. **Run in detached mode** (background):
   ```bash
   docker-compose up -d --build
   ```

The API will be available at `http://localhost:8080`.

### Production Deployment with Coolify

ForkPoint is configured for production deployment using [Coolify](https://coolify.io) on a VPS.

#### Coolify Setup Steps:

1. **Connect your repository** to Coolify
2. **Create a new project** and select "Docker Compose" as the deployment method
3. **Use `docker-compose.prod.yml`** as your compose file
4. **Configure environment variables** in Coolify:
   - Copy the values from `.env.prod.example`
   - Set secure values for passwords and secrets
5. **Deploy** the application

#### Coolify Environment Variables:

Set these in your Coolify project settings:

```
POSTGRES_DB=ForkPoint
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_secure_db_password
JWT_KEY=your_256_bit_jwt_secret_key_here
JWT_ISSUER=https://your-coolify-domain.com
JWT_AUDIENCE=https://your-coolify-domain.com
EMAIL_FROM=noreply@yourdomain.com
EMAIL_SERVER=smtp.gmail.com
EMAIL_PORT=587
EMAIL_USERNAME=your-email@gmail.com
EMAIL_PASSWORD=your-app-password
```

#### Coolify Features Used:

- **Automatic SSL**: Coolify handles HTTPS certificates automatically
- **Reverse Proxy**: Built-in nginx with load balancing
- **Health Checks**: Automatic monitoring using the `/health` endpoint
- **Zero-downtime Deployments**: Rolling updates with health checks
- **Resource Management**: Memory and CPU limits configured
- **Logging**: Centralised logging through Coolify dashboard

### Docker Commands

- **Stop services**:
  ```bash
  docker-compose down
  ```

- **View logs**:
  ```bash
  docker-compose logs -f forkpoint-api
  ```

- **Rebuild after code changes**:
  ```bash
  docker-compose up --build --force-recreate
  ```

### Database Migrations in Docker

To apply EF Core migrations in the Docker environment:

```bash
docker-compose exec forkpoint-api dotnet ef database update
```

Note: The application has been configured to use PostgreSQL instead of SQL Server for better containerisation support.

## Running the Application

1. **Apply database migrations**:
   ```bash
   dotnet ef database update --project ForkPoint.Infrastructure --startup-project ForkPoint.API
   ```

2. **Run the application**:
   ```bash
   dotnet run --project ForkPoint.API
   ```

The API will be available at `https://localhost:7078` (configured in launchSettings.json).

## API Documentation

ForkPoint includes Swagger UI for interactive API documentation. When running the application, visit `https://localhost:7078` to explore and test the API endpoints.

Key endpoints include:
- `GET /api/restaurants` - Get all restaurants
- `POST /api/restaurants/create` - Create a new restaurant
- `GET /api/restaurants/{id}` - Get restaurant by ID
- `DELETE /api/restaurants/{id}` - Delete restaurant
- `PATCH /api/restaurants/{id}` - Update restaurant
- Authentication endpoints in `AuthController`
- User management in `AccountController` and `AdminController`

## Testing

Run unit tests using:
```bash
dotnet test
```

The test project is located in `tests/unit/ForkPoint.Application.Tests/`.

## Database

ForkPoint uses Entity Framework Core with PostgreSQL. The database includes tables for:
- Users
- Restaurants
- MenuItems
- Addresses (value object)

Migrations are located in `ForkPoint.Infrastructure/Migrations/`.

### Database Seeding

The application includes an `ApplicationSeeder` that automatically populates the database with initial data when the application starts. The seeder performs the following operations:

#### Roles Seeding
Creates three default user roles:
- **Admin**: Full administrative privileges
- **Owner**: Restaurant ownership capabilities
- **User**: Standard user permissions

#### Default Users
Creates two default user accounts for testing and development:
- **Admin User**: `forkpointadmin@gmail.com` / `AdminPassword1!`
- **Regular User**: `forkpointuser@gmail.com` / `UserPassword1!`

#### Restaurant Data
Imports a dataset of restaurants from `ForkPoint.Infrastructure/Seeders/restaurants.json`, including:
- Restaurant details (name, description, category, contact info)
- Complete addresses (street, city, county, country, postcode)
- Associated menu items with pricing and nutritional information
- Delivery availability flags

The seeding process is idempotent - it only adds data that doesn't already exist, making it safe to run multiple times.

**Note**: The restaurant dataset contains thousands of sample restaurants across various categories (Burgers, Italian, Chinese, Mexican, etc.) to provide realistic test data for development and demonstration purposes.

## Authentication and Authorisation

- **JWT Authentication**: Bearer token-based authentication for API access.
- **Google OAuth**: External authentication provider.
- **Roles**: Admin and Owner roles with specific policies.
- **Policies**:
  - `AdminOrOwnerPolicy`: Requires admin or restaurant owner role.
  - `OwnsRestaurantOrAdminPolicy`: Requires ownership of the restaurant or admin role.
