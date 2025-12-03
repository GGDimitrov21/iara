Database schema and usage

- Engine: PostgreSQL
- Tooling: pgAdmin 4 or psql

Setup

1. Create database `iara`.
2. Run `schema.sql` to create tables and indexes.

Connection string examples

- ASP.NET Core: set `ConnectionStrings:Default` in `appsettings.Development.json` or `DATABASE_CONNECTION` env var.
- Format: `Host=localhost;Database=iara;Username=postgres;Password=postgres`.
