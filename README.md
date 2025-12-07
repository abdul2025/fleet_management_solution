(**Fleet Management Solution**)

( - **Summary:**: A modular .NET 9 solution implementing a fleet/aircraft management system. It follows a layered architecture (Domain, Application, Infrastructure, Shared, Web) and uses Entity Framework Core with PostgreSQL.)
( - **Location:**: The codebase root is the repository root; the runnable web app is in `src/FleetManagement.Web`.)

(**Architecture**)

( - **Solution:**: `FleetManagement.sln` — groups the following projects.)
( - **Projects:**:) 
(  - `src/FleetManagement.Domain` : domain entities and value objects.)
(  - `src/FleetManagement.Application` : application services, commands/queries, DTOs.)
(  - `src/FleetManagement.Infrastructure` : EF Core `DbContext`, repositories, data migrations, and interceptors.)
(  - `src/FleetManagement.Shared` : shared helpers, constants and enums.)
(  - `src/FleetManagement.Web` : ASP.NET Core MVC web frontend and app startup.)

(**Tech Stack & Key Details**)

( - **Target framework:**: `net9.0` (all projects target .NET 9))
( - **ORM / DB provider:**: Entity Framework Core (EF Core) with `Npgsql.EntityFrameworkCore.PostgreSQL` (PostgreSQL provider).)
( - **EF Core Version:**: 9.x (see `FleetManagement.Infrastructure` package refs))
( - **Migrations assembly:**: Migrations are configured to be in `FleetManagement.Infrastructure` (see `Program.cs` and DbContext configuration).)
( - **Interceptors:**: `BaseEntityInterceptor` is registered with the DbContext and added to options (handles base-entity behaviors).)
( - **Auto-migrate on startup:**: `Program.cs` contains optional code to apply pending migrations on startup (it runs `context.Database.Migrate()` if there are pending migrations).)

(**Getting Started (local development)**)

( - **Prerequisites:**: Install the .NET 9 SDK and PostgreSQL (or use a Docker container). Ensure `dotnet` is on your `PATH`.)

( - **Clone & open:**:)
(  - `git clone <repo-url>`)
(  - `cd FleetManagement.Solution`)

( - **Restore & build:**:)
(  - `dotnet restore`) 
(  - `dotnet build -c Debug`)

( - **Set the database connection:**: Configure the connection string named `DefaultConnection` in `src/FleetManagement.Web/appsettings.json` or your environment. Example environment variable (macOS/zsh):)

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=fleetdb;Username=postgres;Password=postgres"
```

( - **Run the web app:**)
(  - From repository root: `dotnet run --project src/FleetManagement.Web -c Debug`)
(  - Or open the solution in Visual Studio / Rider and run the `FleetManagement.Web` project.)

(**Database Migrations (EF Core)**)

( - **Add a migration:**)
(  - `dotnet ef migrations add <Name> --project src/FleetManagement.Infrastructure --startup-project src/FleetManagement.Web --output-dir Data/Migrations`)

( - **Update database:**)
(  - `dotnet ef database update --project src/FleetManagement.Infrastructure --startup-project src/FleetManagement.Web`)

( - **Notes:**: Migrations are stored/run from `FleetManagement.Infrastructure` (the migrations assembly). `Program.cs` may auto-apply migrations on startup — use this carefully in production.)

(**Code Structure & Conventions**)

( - **Domain:**: Entities and value objects live under `src/FleetManagement.Domain` (e.g., `Aircraft` entity).)
( - **Application:**: Commands, Queries, DTOs and interfaces for app services are in `src/FleetManagement.Application`.)
( - **Infrastructure:**: `ApplicationDbContext` is in `src/FleetManagement.Infrastructure/Data/ApplicationDbContext.cs`. Repositories and data configuration live under `Infrastructure/Data` and `Infrastructure/Repositories`.)
( - **Web:**: MVC controllers, views, and static assets are in `src/FleetManagement.Web`.)

(**Development notes & tips**)

( - **MigrationsAssembly:**: The DB context is configured in `Program.cs` with `b => b.MigrationsAssembly("FleetManagement.Infrastructure")` so migrations are created in the Infrastructure project.)
( - **Interceptors:**: `BaseEntityInterceptor` is registered both in DI and in `OnConfiguring` to ensure it's applied.)
( - **Seeding:**: There is a commented placeholder in `Program.cs` for seeding initial data; implement `SeedData.Initialize` if needed.)

(**Contributing**)

( - **Branching:**: Use feature branches off `dev` and open PRs targeting `dev`.)
( - **Formatting & linting:**: Keep with project conventions; run `dotnet format` if available.)

(**Contact / Maintainer**)

( - **Repository owner:**: `abdul2025` (local workspace owner: `abdulwahab`))

(**Next steps I can help with**)

( - Add a `.env.example` with the recommended `ConnectionStrings__DefaultConnection` value.)
( - Add CI workflow to build and run EF migrations in CI.)
( - Scaffold a small README section for API endpoints or seed data.)

(---)

(If you'd like, I can now add a `.env.example`, create example `appsettings.Development.json`, or add a GitHub Actions workflow to run `dotnet build` and `dotnet ef database update` in CI. Which would you prefer next?)
## Need a contnet here