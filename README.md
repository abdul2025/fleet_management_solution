# Fleet Management Solution

A modular .NET 9 solution implementing a comprehensive fleet and aircraft management system. Built with a clean, layered architecture following Domain-Driven Design principles.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Database Migrations](#database-migrations)
- [Project Structure](#project-structure)
- [Development](#development)
- [Contributing](#contributing)

## 🚀 Overview

The Fleet Management Solution is a enterprise-grade application designed to manage fleets and aircraft operations. The system follows clean architecture principles with clear separation of concerns across multiple layers, ensuring maintainability, scalability, and testability.

## 🏗️ Architecture

This solution follows a **layered architecture** pattern with the following projects:

### Solution Structure

```
FleetManagement.sln
├── src/
│   ├── FleetManagement.Domain/          # Domain entities and value objects
│   ├── FleetManagement.Application/     # Application services, commands/queries, DTOs
│   ├── FleetManagement.Infrastructure/  # EF Core DbContext, repositories, migrations
│   ├── FleetManagement.Shared/          # Shared helpers, constants, and enums
│   └── FleetManagement.Web/             # ASP.NET Core MVC web frontend
```

### Layer Responsibilities

- **Domain Layer**: Core business entities, value objects, and domain logic
- **Application Layer**: Use cases, commands, queries, DTOs, and application service interfaces
- **Infrastructure Layer**: Data access implementation using EF Core, repositories, and external services
- **Shared Layer**: Cross-cutting concerns, utilities, constants, and enums
- **Web Layer**: MVC controllers, views, static assets, and application entry point

## 🛠️ Tech Stack

- **Framework**: .NET 9.0
- **ORM**: Entity Framework Core 9.x
- **Database**: PostgreSQL (via Npgsql.EntityFrameworkCore.PostgreSQL)
- **Web Framework**: ASP.NET Core MVC
- **Architecture Pattern**: Clean Architecture / Layered Architecture
- **Design Pattern**: Domain-Driven Design (DDD)

### Key Features

- **Entity Framework Core** with PostgreSQL provider
- **Migration Management** in Infrastructure layer
- **Interceptors** for cross-cutting concerns (BaseEntityInterceptor)
- **Auto-migration** on startup (configurable)
- **Dependency Injection** throughout the application

## 🏁 Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/) (or use Docker)
- A code editor ([Visual Studio 2022](https://visualstudio.microsoft.com/), [Rider](https://www.jetbrains.com/rider/), or [VS Code](https://code.visualstudio.com/))

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/abdul2025/fleet_management_solution.git
   cd fleet_management_solution
   ```

2. **Checkout the dev branch**

   ```bash
   git checkout dev
   ```

3. **Restore dependencies**

   ```bash
   dotnet restore
   ```

4. **Build the solution**

   ```bash
   dotnet build -c Debug
   ```

### Database Configuration

Configure your PostgreSQL connection string in one of the following ways:

#### Option 1: Using appsettings.json

Edit `src/FleetManagement.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fleetdb;Username=postgres;Password=postgres"
  }
}
```

#### Option 2: Using Environment Variables

**macOS/Linux (bash/zsh):**
```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=fleetdb;Username=postgres;Password=postgres"
```

**Windows (PowerShell):**
```powershell
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=fleetdb;Username=postgres;Password=postgres"
```

**Windows (CMD):**
```cmd
set ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=fleetdb;Username=postgres;Password=postgres
```

#### Option 3: Using Docker for PostgreSQL

```bash
docker run --name fleet-postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=fleetdb -p 5432:5432 -d postgres:latest
```

### Running the Application

From the repository root:

```bash
dotnet run --project src/FleetManagement.Web -c Debug
```

Or using Visual Studio/Rider:
1. Open `FleetManagement.sln`
2. Set `FleetManagement.Web` as the startup project
3. Press F5 to run

The application will be available at `https://localhost:5001` (or `http://localhost:5000`)

## 🗄️ Database Migrations

The project uses Entity Framework Core migrations, which are stored in the `FleetManagement.Infrastructure` project.

### Add a New Migration

```bash
dotnet ef migrations add <MigrationName> \
  --project src/FleetManagement.Infrastructure \
  --startup-project src/FleetManagement.Web \
  --output-dir Data/Migrations
```

Example:
```bash
dotnet ef migrations add AddAircraftTable \
  --project src/FleetManagement.Infrastructure \
  --startup-project src/FleetManagement.Web \
  --output-dir Data/Migrations
```

### Apply Migrations

```bash
dotnet ef database update \
  --project src/FleetManagement.Infrastructure \
  --startup-project src/FleetManagement.Web
```

### Remove Last Migration

```bash
dotnet ef migrations remove \
  --project src/FleetManagement.Infrastructure \
  --startup-project src/FleetManagement.Web
```

### List All Migrations

```bash
dotnet ef migrations list \
  --project src/FleetManagement.Infrastructure \
  --startup-project src/FleetManagement.Web
```

### Auto-Migration on Startup

> ⚠️ **Note**: The application may include code in `Program.cs` to automatically apply pending migrations on startup. Use this feature carefully in production environments.

## 📁 Project Structure

### FleetManagement.Domain

Contains the core business logic and domain entities:
- **Entities**: Core business objects (e.g., `Aircraft`, `Fleet`)
- **Value Objects**: Immutable objects defined by their attributes
- **Domain Events**: Events that represent business occurrences
- **Interfaces**: Domain service contracts

### FleetManagement.Application

Contains application-specific business logic:
- **Commands**: Write operations (CQRS pattern)
- **Queries**: Read operations (CQRS pattern)
- **DTOs**: Data Transfer Objects for layer communication
- **Interfaces**: Application service contracts
- **Mappings**: Object mapping configurations

### FleetManagement.Infrastructure

Contains infrastructure concerns and external dependencies:
- **Data/ApplicationDbContext.cs**: EF Core database context
- **Data/Migrations/**: EF Core migration files
- **Repositories**: Data access implementations
- **Interceptors**: `BaseEntityInterceptor` for entity lifecycle management
- **Configurations**: EF Core entity configurations

### FleetManagement.Shared

Contains shared utilities and cross-cutting concerns:
- **Constants**: Application-wide constant values
- **Enums**: Shared enumeration types
- **Helpers**: Utility classes and extension methods

### FleetManagement.Web

Contains the web presentation layer:
- **Controllers**: MVC controllers
- **Views**: Razor views
- **wwwroot**: Static files (CSS, JavaScript, images)
- **Program.cs**: Application entry point and configuration

## 💻 Development

### Code Structure & Conventions

- **Migrations Assembly**: Configured in `Program.cs` with `MigrationsAssembly("FleetManagement.Infrastructure")`
- **Interceptors**: `BaseEntityInterceptor` is registered in both DI container and DbContext configuration
- **Seeding**: Placeholder in `Program.cs` for initial data seeding (implement `SeedData.Initialize` if needed)

### Development Tips

1. **Keep Layers Separate**: Respect the dependency flow: Web → Application → Domain
2. **Entity Configuration**: Define EF Core configurations in `Infrastructure/Data/Configurations`
3. **Use DTOs**: Never expose domain entities directly to the presentation layer
4. **Follow CQRS**: Separate commands (writes) from queries (reads) in the Application layer

### Code Formatting

Run code formatting:
```bash
dotnet format
```

### Build in Release Mode

```bash
dotnet build -c Release
```

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

### Branching Strategy

- Main branch: `main` (stable releases)
- Development branch: `dev` (active development)
- Feature branches: `feature/<feature-name>` (branched from `dev`)
- Bugfix branches: `bugfix/<bug-name>` (branched from `dev`)

### Workflow

1. Fork the repository
2. Create a feature branch from `dev`
   ```bash
   git checkout dev
   git pull origin dev
   git checkout -b feature/your-feature-name
   ```
3. Make your changes and commit with clear messages
   ```bash
   git commit -m "Add: Brief description of your changes"
   ```
4. Push to your fork
   ```bash
   git push origin feature/your-feature-name
   ```
5. Open a Pull Request targeting the `dev` branch

### Commit Message Guidelines

- `Add:` for new features
- `Fix:` for bug fixes
- `Update:` for updates to existing features
- `Refactor:` for code refactoring
- `Docs:` for documentation changes

### Code Review

All pull requests require review before merging. Please ensure:
- Code follows project conventions
- All tests pass (if applicable)
- Documentation is updated
- Migrations are included if schema changes

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 👤 Contact

**Repository Owner**: [@abdul2025](https://github.com/abdul2025)

For questions, issues, or suggestions:
- Open an issue on GitHub
- Create a pull request with improvements

## 🙏 Acknowledgments

Built with modern .NET technologies and best practices for enterprise application development.

---

**Status**: Active Development (dev branch)

Last Updated: December 2024
