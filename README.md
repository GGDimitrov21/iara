# IARA Information System

Clean N-layer architecture with two branches: `API` and `UIWEB`.

**Architecture**
- **Domain**: Entities and core models (`api/src/Iara.Domain`).
- **Application**: Interfaces, use cases, DTOs, validators (`api/src/Iara.Application`).
- **Infrastructure**: EF Core + Npgsql, external services, implementations (`api/src/Iara.Infrastructure`).
- **API**: ASP.NET Core minimal hosting, controllers, Swagger (`api/src/Iara.Api`).
- **UIWEB**: React + TypeScript (Vite) web client scaffold location (`uiweb/`).
- **DB**: PostgreSQL schema and notes (`db/schema.sql`).

**Folder Map**
- `api/Iara.sln` — Visual Studio solution containing the 4 API projects.
- `api/src/Iara.*` — projects per layer (Domain, Application, Infrastructure, Api).
- `uiweb/` — React + TS + Vite app (to be scaffolded).
- `db/` — SQL scripts and database README.

**Prerequisites**
- .NET 9 SDK, Node.js LTS, PostgreSQL 15+.

**Database**
1. Create database `iara`.
2. Run `db/schema.sql`.

**Run API (Windows PowerShell)**
```
Set-Location "e:\projectsRepos\iara\iara\api\src\Iara.Api"
$env:ASPNETCORE_ENVIRONMENT = "Development"; dotnet run
```
Swagger: https://localhost:7243/swagger

To configure DB connection set `ConnectionStrings:Default` in `appsettings.Development.json` or env var `DATABASE_CONNECTION`.

**Scaffold UIWEB with Vite**
```
Set-Location "e:\projectsRepos\iara\iara\uiweb"
npm create vite@latest . -- --template react-ts
npm install
npm run dev
```

**Next Steps**
- Implement repositories/use cases in Application layer.
- Map remaining tables to EF Core entities.
- Add endpoints for reports required in the assignment.