# Development Workflow

## Prerequisites

- **.NET SDK 9.0** (pinned in `global.json`)
- **Node.js** (for Vue.js frontend)
- **PostgreSQL** database
- **Identity Server** instance (for authentication)

## Install Dependencies

### Backend (.NET)
```bash
dotnet restore SUTH-Checkup.sln
```

### Frontend (Vue.js)
```bash
cd src/vuewebui
npm install
```

## Run Locally

### API Server
```bash
cd src/API
dotnet run
```
The API launches on the profile configured in `src/API/Properties/launchSettings.json`.

### Vue.js Frontend
```bash
cd src/vuewebui
npm run dev
```
Starts Vite dev server with HTTPS (uses .NET dev certs).

### WinForms Desktop
```bash
cd src/WinFormsUI
dotnet run
```
Or open in Visual Studio and run directly.

### Worker Service
```bash
cd src/HosxpWorkerService
dotnet run
```

### Configuration
- API: `src/API/appsettings.Development.json`
- Worker: `src/HosxpWorkerService/appsettings.Development.json`
- WinForms: `src/WinFormsUI/appsettings.Development.json`
- Vue: `.env` file in `src/vuewebui/` (see `.env.example` for variables)

Required environment variables for Vue:
- `VITE_API_BASE_URL` — Backend API endpoint
- `VITE_OIDC_AUTHORITY` — Identity Server URL
- `VITE_OIDC_CLIENT_ID` — OIDC client ID
- `VITE_OIDC_REDIRECT_URI` — Post-login redirect
- `VITE_OIDC_SCOPE` — Token scopes

Required configuration for API:
- `ConnectionStrings:CheckupDatabase` — PostgreSQL connection string
- `Identity:Authority` — Identity Server URL (mandatory, throws on missing)

## Run Tests

### All Tests
```bash
dotnet test SUTH-Checkup.sln
```

### Individual Test Projects
```bash
# Domain unit tests (33 test files)
dotnet test tests/Domain.UnitTests

# Application unit tests
dotnet test tests/Application.UnitTests

# Functional/integration tests (requires PostgreSQL)
dotnet test tests/Application.FunctionalTests
```

### Test Frameworks
- **NUnit 3** — Test framework
- **FluentAssertions** — Assertion library
- **Moq** — Mocking framework
- **Respawn** — Database reset between tests

## Build Process

### Backend
```bash
dotnet build SUTH-Checkup.sln
```

### Frontend
```bash
cd src/vuewebui
npm run build
```
Output goes to `dist/` directory.

### Publish (Production)
```bash
dotnet publish src/API -c Release -o publish/api
dotnet publish src/HosxpWorkerService -c Release -o publish/worker
```

## Database Migrations

### Add a New Migration
```bash
# From solution root or Infrastructure project
dotnet ef migrations add <MigrationName> --project src/Infrastructure --startup-project src/API
```

### Apply Migrations
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/API
```
Migrations are also auto-applied on API startup via `CheckupDatabaseContextInitializer`.

### Drop Database
```bash
dotnet ef database drop --project src/Infrastructure --startup-project src/API
```

## Code Scaffolding

The project supports Clean Architecture templates:
```bash
# Create a new Command
dotnet new ca-usecase --name CreateFoo --feature-name Foos --usecase-type command --return-type int

# Create a new Query
dotnet new ca-usecase --name GetFoo --feature-name Foos --usecase-type query --return-type FooViewModel
```

## API Client Generation

NSwag-generated API clients are used by the Worker Service and WinForms app:
- Swagger specs: `OpenAPIs/swagger.json`, `OpenAPIs/swagger1.json`
- Generated clients: `OpenAPIs/CheckupApiClient.cs`, `OpenAPIs/HosxpApiClient.cs`

Regenerate after API changes using NSwag tooling or `dotnet build` (configured in .csproj).

## CI/CD

No dedicated CI/CD pipeline configuration is currently present. Build and deployment are manual.

## Git Workflow

- **Main branch:** `master`
- **Commit messages:** Thai or English, meaningful descriptions
- **Branch naming:** Feature-based or descriptive
- **PR strategy:** Rebase before merge
- **Code review:** Required before merge
