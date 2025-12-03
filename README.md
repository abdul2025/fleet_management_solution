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
├─ Application Layer (FleetManagement.Application)
│   ├─ Entities, Enums, Events, Exceptions
│   ├─ Interfaces, Specifications, ValueObjects
│
├─ Infrastructure (FleetManagement.Infrastructure)
│   ├─ Repositories, Data, Migrations
│   ├─ External Services, Background Jobs
│
├─ Domain Layer (FleetManagement.Domain)
│   ├─ Core business rules and entities
│
└─ Shared Layer (FleetManagement.Shared)
    ├─ Constants, Extensions, Helpers, Resources
```

> **Rule:** Inner layers never depend on outer layers.

---

## Project Structure

### 1️⃣ Domain Layer (`FleetManagement.Domain`)

* **Purpose:** Core business entities and rules
* **Dependencies:** None (pure domain)
* **Contents:** Entities, ValueObjects, Enums, Events, Exceptions

### 2️⃣ Application Layer (`FleetManagement.Application`)

* **Purpose:** Application services and business logic
* **Dependencies:** Domain, Shared
* **NuGet:** AutoMapper, FluentValidation
* **Contents:** Entities, Events, Interfaces, Specifications, ValueObjects

### 3️⃣ Infrastructure Layer (`FleetManagement.Infrastructure`)

* **Purpose:** Data access, external services, background jobs
* **Dependencies:** Application, Domain, Shared
* **NuGet:** EF Core Tools
* **Contents:** Data (Configurations & Migrations), Repositories, ExternalServices, BackgroundJobs

### 4️⃣ Shared Layer (`FleetManagement.Shared`)

* **Purpose:** Cross-cutting utilities
* **NuGet:** EF Core Design (v9), Newtonsoft.Json
* **Contents:** Constants, Extensions, Helpers, Resources

### 5️⃣ Web Layer (`FleetManagement.Web`)

* **Purpose:** MVC presentation layer
* **Dependencies:** Application, Infrastructure, Shared
* **NuGet:** Serilog, AutoMapper, EF Core Design
* **Contents:** Controllers, Middleware, Filters, Models, Views, Program.cs, appsettings.json

---

## Test Projects

* **UnitTests:** Test components in isolation 🧪
* **IntegrationTests:** Test multiple components together 🧪
* **FunctionalTests:** End-to-end user workflow tests 🧪

---

## Technology Stack

| Component        | Version | Purpose          |
| ---------------- | ------- | ---------------- |
| .NET             | 9.0     | Runtime          |
| ASP.NET Core     | 9.0     | Web Framework    |
| EF Core          | 9.x     | ORM              |
| AutoMapper       | 15.x    | Object Mapping   |
| FluentValidation | 12.x    | Input Validation |
| Serilog          | Latest  | Logging          |
| Newtonsoft.Json  | Latest  | JSON Processing  |

---

## Key Principles

* **Clean Architecture:** Separation of concerns, testable, framework-independent
* **Dependency Flow:**
  `Web → Application → Domain + Shared`
* **Patterns Used:** Repository, Specification, Event Sourcing, Middleware, Filters, Dependency Injection

---

## Startup & Configuration

* **Entry Point:** `/Program.cs` (Web)
* **Config Files:** `appsettings.json`, `appsettings.Development.json`
* **Dependency Injection:** Built-in ASP.NET Core IoC
* **Logging:** Serilog (console + file)

---

## Build & Run

```bash
# Navigate to Web project
cd src/FleetManagement.Web

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run
```

> App runs on `http://localhost:5000` (default)

---

## Notes

* Domain layer has **no external dependencies**
* Infrastructure layer contains **all external integrations**
* Web layer handles **HTTP and UI concerns only**
* Application layer contains **business logic and workflows**
