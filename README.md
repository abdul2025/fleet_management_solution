# Fleet Management Solution

## Overview

FleetManagement is a **.NET 9 MVC Web application** following **Clean Architecture**, with clear separation of concerns and layered design.

---

## Architecture

```
Web (FleetManagement.Web)
│
├─ Controllers / Middleware / Filters
│
Explanation: update README to reflect current repo scan, required EF/Postgres packages, commands and migration notes.

# Fleet Management Solution

## Overview

FleetManagement is a .NET 9.0 solution following Clean Architecture (Presentation → Application → Domain + Shared, Infrastructure isolated for external concerns).

This README reflects the current repository scan (Dec 2025): which packages are present/missing, recommended package layout for EF Core + PostgreSQL, exact CLI commands, and migration guidance.

---

## Compact Project Diagram

```
FleetManagement.sln
├─ src/
│  ├─ FleetManagement.Web/                 (Presentation / Startup)
│  ├─ FleetManagement.Application/         (Application / Use-cases)
│  ├─ FleetManagement.Infrastructure/      (Infrastructure / Persistence)
│  ├─ FleetManagement.Domain/              (Domain / Core)
│  └─ FleetManagement.Shared/              (Cross-cutting)
```

---

## Current scan (what I found)

- All projects target `net9.0`.
- No explicit EF Core or Npgsql (Postgres) provider package references were found in `Infrastructure` or `Web`.
- Test projects do not contain `Microsoft.EntityFrameworkCore.InMemory` or `Npgsql` references.
- `src/FleetManagement.Shared/FleetManagement.Shared.csproj` contains a malformed `PackageReference` block that should be cleaned.

Because EF provider and design packages are not present, no migrations or runtime DB access will work until packages are added and DbContext is implemented/registered.

---

## Recommended packages (aligned with .NET 9)

Use EF Core 9.x packages to match `net9.0` projects. Recommended placements:

- `src/FleetManagement.Infrastructure` (where DbContext and EF types should live):
  - `Npgsql.EntityFrameworkCore.PostgreSQL` (Postgres provider)
  - `Microsoft.EntityFrameworkCore` (optional explicit reference)
  - `Microsoft.EntityFrameworkCore.Design` (design-time helpers) — `PrivateAssets=all`
  - `Microsoft.EntityFrameworkCore.Tools` (Tools) — `PrivateAssets=all`

- `src/FleetManagement.Web` (startup project used for migrations):
  - `Npgsql.EntityFrameworkCore.PostgreSQL` (recommended for runtime if Web resolves DbContext)
  - `Microsoft.EntityFrameworkCore.Design` — `PrivateAssets=all`

- Tests:
  - `tests/FleetManagement.UnitTests`: `Microsoft.EntityFrameworkCore.InMemory` (for unit tests)
  - `tests/FleetManagement.IntegrationTests` & `FunctionalTests`: `Npgsql.EntityFrameworkCore.PostgreSQL` or use Testcontainers (`DotNet.Testcontainers`) to run ephemeral Postgres instances.

---

## Exact dotnet CLI commands (example using EF Core 9.0.0)

Run from repository root. Adjust versions if you choose a different EF Core minor version.

```bash
# Infrastructure (DbContext + EF code)
dotnet add src/FleetManagement.Infrastructure/FleetManagement.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.0
dotnet add src/FleetManagement.Infrastructure/FleetManagement.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design --version 9.0.0
dotnet add src/FleetManagement.Infrastructure/FleetManagement.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools --version 9.0.0

# Web (startup project) — design/runtime support for migrations
dotnet add src/FleetManagement.Web/FleetManagement.Web.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.0
dotnet add src/FleetManagement.Web/FleetManagement.Web.csproj package Microsoft.EntityFrameworkCore.Design --version 9.0.0

# Tests
dotnet add tests/FleetManagement.UnitTests/FleetManagement.UnitTests.csproj package Microsoft.EntityFrameworkCore.InMemory --version 9.0.0
dotnet add tests/FleetManagement.IntegrationTests/FleetManagement.IntegrationTests.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.0

# Optional: install dotnet-ef global tool matching EF version
dotnet tool install --global dotnet-ef --version 9.0.0
```

Notes:
- Prefer matching EF package major version with runtime target (`net9.0` → EF Core 9.x).
- Use `--no-restore` if you plan to run restore in a separate step.

---

## Migrations (common commands)

Typical workflow: create migrations in `Infrastructure` project while using `Web` as the startup project (so DI and configuration are available):

```bash
# Add migration (creates files under Infrastructure project's Migrations folder)
dotnet ef migrations add InitialCreate \
  --project src/FleetManagement.Infrastructure \
  --startup-project src/FleetManagement.Web \
  --output-dir Data/Migrations

# Apply migrations to the database (uses startup project's configuration)
dotnet ef database update \
  --project src/FleetManagement.Infrastructure \
  --startup-project src/FleetManagement.Web
```

If you prefer to run migrations directly from `Infrastructure`, implement a `IDesignTimeDbContextFactory<TContext>` in `Infrastructure` to provide a design-time DbContext with a connection string.

---

## Fixes & housekeeping

- Fix the malformed `PackageReference` block in `src/FleetManagement.Shared/FleetManagement.Shared.csproj` (there's an orphaned `IncludeAssets` / `PrivateAssets` snippet present).
- Add the EF/Postgres packages to `Infrastructure` and `Web` as shown above.
- Add/confirm `DbContext` implementation (likely `ApplicationDbContext` or similar) and register it with DI in `FleetManagement.Web/Program.cs`:

```csharp
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
```

---

## Quick verification steps

```bash
dotnet restore
dotnet build
dotnet ef migrations list --project src/FleetManagement.Infrastructure --startup-project src/FleetManagement.Web
```

If you see errors during `dotnet ef` about design-time services or DbContext resolution, add a `DesignTimeDbContextFactory` to `Infrastructure`.

---

If you want, I can apply the package changes to the `.csproj` files and fix the malformed `Shared` csproj now; tell me whether you want EF Core 9 (recommended) or to upgrade the solution to `net10.0` and use EF Core 10.
