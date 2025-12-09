# ForkPoint

ForkPoint is a restaurant management API built using Clean Architecture principles in ASP.NET Core.
It provides a platform for managing restaurants, menu items, user authentication, and admin
operations. The application follows modern software development practices, including CQRS with
MediatR, dependency injection, and validation.

Fully containerised with Docker and optimised for deployment on Coolify (self-hosted Vercel
alternative).

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Technologies Used](#technologies-used)
- [Installation and Setup](#installation-and-setup)
- [Configuration](#configuration)
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
- **Middleware**: Custom middleware for error handling, elapsed time logging, and sensitive data
  protection.
- **External API Caching**: The Foursquare client caches successful search responses in memory
  to reduce requests and improve performance. Cache entries expire after 60 minutes.

## Architecture

ForkPoint follows Clean Architecture principles, separating concerns into distinct layers:

- **ForkPoint.API**: Presentation layer containing controllers, middleware, and API configuration.
- **ForkPoint.Application**: Application layer with business logic, handlers (CQRS), services, and
  validation.
- **ForkPoint.Domain**: Domain layer containing entities, Data models and rules.
- **ForkPoint.Infrastructure**: Infrastructure layer handling data persistence, external services.

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
- **Authentication**: JWT Bearer Tokens, Google OAuth (supported but not implemented in client)
- **Email**: MailKit and Resend
- **Logging**: Serilog
- **Documentation**: Swagger/OpenAPI
- **Testing**: xUnit and Moq
- **Dependency Injection**: Built-in ASP.NET Core DI container

### Client

A frontend client lives in the `client/` folder. It provides the public UI for registration, login,
restaurants browsing and account management and is built with a modern frontend toolchain:

- Framework: React + TypeScript
- Bundler and Dev server: Vite
- Styling: Tailwind CSS
- Routing: TanStack Router
- Data fetching and caching: React Query (TanStack Query)
- Testing: Vitest + react testing library
- Linting & formatting: Biome

The client talks to the API under `/api/*` (same-origin when served together).

## Installation and Setup

1. Clone the repo:

  ```bash
  git clone https://github.com/Lucaas27/ForkPoint.git
  cd ForkPoint
  ```

2. Restore and build:

  ```bash
  dotnet restore
  dotnet build
  ```

3. Apply database migrations:

  ```bash
  dotnet ef database update --project ForkPoint.Infrastructure --startup-project ForkPoint.API
  ```

4. Run the API:

  ```bash
  dotnet run --project ForkPoint.API
  ```

The API will be available at the URL shown in the console (`https://localhost:7078`).

## Configuration

The application uses `appsettings.json` for configuration. Key settings include:

- **Database Connection**: Configure the PostgreSQL connection string in `appsettings.json` or user
  secrets.
- **JWT Settings**: Configure JWT issuer, audience, and secret key.
- **Email Settings**: Configure SMTP server details for email services.
- **Google OAuth**: Configure client ID and secret for Google authentication.

## Database

The project uses EF Core with PostgreSQL. Migrations are in `ForkPoint.Infrastructure/Migrations/`.

On startup the app can seed sample data (users, roles, and restaurants) for development. The seed
runs only if the data is not present.

## Testing

Run unit tests:

```bash
dotnet test
```

Tests are in `tests/unit/`.

## Authentication and Authorisation

The app uses JWT tokens for API authentication and also supports Google sign-in for external logins.

- Users can sign up and sign in using the endpoints implemented in the `AuthController` and
  `AccountController`.
- After signing in the API returns a JWT access token.
- Roles: the app includes `Admin`, `User`, `Owner` roles. Role checks are applied to controller
  actions.
- Policies: the project defines policies such as `AdminOrOwnerPolicy` and
  `OwnsRestaurantOrAdminPolicy` to control access to sensitive operations.
- To refresh the access token, clients call the `RefreshToken` endpoint, which reads the refresh
  token from the HttpOnly cookie and issues a new access token if valid.
- Refresh tokens have a longer expiry (30 days) compared to access tokens (e.g. 15 minutes).
- The refresh token is rotated on each use, meaning a new refresh token is issued along with the new
  access token. This limits the window of opportunity if a refresh token is compromised.
