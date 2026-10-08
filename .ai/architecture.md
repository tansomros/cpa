# Architecture

## What this system is

CPA Thai Project is the foundation of a small web application for Community pharmacy Association (Thailand) (สมาคมเภสัชกรรมชุมชน (ประเทศไทย)). Current scope: master data (ระบบตั้งค่าข้อมูลพื้นฐาน) and pharmacy management (ระบบบริหารข้อมูลร้านยา). Planned scope: pharmacy services — patients, MTM (medication therapy management) services, and lab results — see [domain.md](domain.md).

This is infrastructure a pharmacy depends on operationally. Data accuracy, consistency, and traceability are the top design priority — read [coding-rules.md](coding-rules.md) (money ledger, concurrency, state-transition rules) before making any change that touches money or approval/status state.

## Stack

- **Backend**: C# / .NET 10, ASP.NET Core Web API, EF Core 10 + Npgsql (PostgreSQL), MediatR 14 (CQRS), FluentValidation, AutoMapper, Swashbuckle/NSwag (OpenAPI).
- **Frontend**: Vue 3 (Composition API, `<script setup>`, JavaScript not TypeScript), Vuetify 3 (Vuexy admin template), Pinia, vue-router 4 via file-based routing (`unplugin-vue-router`), CASL for permission-based UI.
- **Auth**: Internal provider — the API issues its own JWT; there is no external identity provider. Login (`POST /users/login` → `LoginCommand` → `IdentityService.LoginAsync` in `src/Infrastructure/Identity/IdentityService.cs`) loads the user by username together with its `Role`, verifies the password against `Users.PasswordHash` with ASP.NET Core `PasswordHasher<User>` (wrapped by `src/Infrastructure/Identity/PasswordHasher.cs`), then `JwtTokenService` creates an HMAC-SHA256 token carrying `sub` (user id), `unique_name`, `name`, `jti`, and one role claim (`Role.Name`). `Program.cs` validates incoming tokens with `AddJwtBearer` (issuer, audience, lifetime, and signing key from the `Jwt` settings section). `src/API/Middlewares/DevAuthenticationMiddleware.cs` exists to create a fake development user (role `Admin` by default), but `Program.cs` does not register it yet (`UseDevAuthentication()` is never called), so today it does not run.
- **Solution layout**: `CPA.sln` / `.slnx`, central package management (`Directory.Packages.props`), `src/Domain`, `src/Application`, `src/Infrastructure`, `src/API`, `src/vuewebui` (frontend), `tests/*`.

## Clean Architecture layering

Dependency direction (enforced by `.csproj` references — do not violate):

```
Domain          → no project dependencies
Application     → Domain
Infrastructure  → Application, Domain
API             → Application, Infrastructure
```

- **Domain** (`src/Domain`): entities, enums, value objects, domain exceptions. No EF, no MediatR request types (only `BaseEvent : INotification` for domain events). Entities should enforce their own invariants (guard clauses via `Ardalis.GuardClauses`), not just be property bags — see [coding-rules.md](coding-rules.md).
- **Application** (`src/Application`): CQRS use cases under `Features/{Feature}/`. Commands go in `Commands/Create`, `Commands/Update`, and `Commands/Delete`. Queries, both the single-item query and the list query, go in `Queries/Get`. Those folder names stay exactly that — do not append the entity. Class names still include the entity (`CreateBankCommand`, `GetBankQuery`, `GetBankListQuery`). The namespace matches the folder, for example `BigLion.CPA.Application.Features.Banks.Commands.Create` and `BigLion.CPA.Application.Features.Banks.Queries.Get`. Each of those folders holds the Command/Query record, its FluentValidation validator, and its MediatR handler. Persistence is reached only through `ICpaDatabaseContext` (defined here, implemented in Infrastructure). Pipeline behaviours (`Common/Behaviours/`): `UnhandledExceptionBehaviour` → `AuthorizationBehaviour` → `ValidationBehaviour` → `PerformanceBehaviour` → `LoggingBehaviour`, wired in `Application/DependencyInjection.cs`.
- **Infrastructure** (`src/Infrastructure`): `CpaDatabaseContext` (EF Core, PostgreSQL), `IEntityTypeConfiguration<T>` classes under `Persistence/Configurations/`, migrations under `Persistence/Migrations/`, save-changes interceptors under `Persistence/Interceptors/`.
- **API** (`src/API`): thin controllers under `Controllers/` — inject `Mediator`, call `Send`, return the result. No business logic here. Global exception handling via `Filters/ApiExceptionFilterAttribute`.

## Feature folder map

Physical layout is one folder per feature. There is no `{Context}/{Feature}` nest and no `{VerbNoun}` use-case folder.

- Domain entities: `src/Domain/Entities/{Entity}.cs`
- EF configurations: `src/Infrastructure/Persistence/Configurations/{Entity}Configuration.cs`
- CQRS: `src/Application/Features/{Feature}/`
  - `Commands/Create`, `Commands/Update`, `Commands/Delete`
  - `Queries/Get` — the single-item query and the list query live here together
  - `ViewModels/`
- Namespace follows that path: `BigLion.CPA.Application.Features.{Feature}.Commands.Create` (and `.Update`, `.Delete`, `.Queries.Get`)

The feature folder is usually the plural name (`Banks`, `Patients`). A few names differ from the entity so they do not clash with a type (`Pharmacy`, `ServiceRecords`, `NewsArticles`, `UserRoleAssignments`).

## CQRS / MediatR conventions

- Command folders are only `Create`, `Update`, and `Delete`. Query folders are only `Get`. Do not append the entity name to those folders.
- Class names keep the entity: `CreateBankCommand`, `UpdateBankCommand`, `DeleteBankCommand`, `GetBankQuery`, `GetBankListQuery`.
- Command/Query, Validator, and Handler live in that folder (co-located, not split across projects).
- Controllers call `Mediator.Send(...)` and nothing else.
- Use the scaffolding template (see [ai-agent-guide.md](ai-agent-guide.md)) to generate a new use case rather than hand-copying an existing one — it keeps the integrity mechanisms (concurrency token, audit log, guard clauses) consistent by default.

## Data model conventions

- **Dual key**: every entity has an internal `int Id` (joins/FKs, never exposed) and a `Guid ExternalId` (UUIDv7, the only identifier exposed via API routes/DTOs).
- **Audit fields**: `CreatedBy`, `LastModifiedBy`, `CreatedOn`, `LastModified`, `IsActive`, `IsDelete` on every entity via `EntityBase`, set automatically by `AuditableEntitySaveChangesInterceptors` — handlers should never set these manually.
- **Optimistic concurrency**: every entity carries the Postgres `xmin` system column as a shadow-property concurrency token (via `ConfigureEntityBase<T>()`, no extra column needed) — a stale write throws `DbUpdateConcurrencyException` rather than silently overwriting.
- **Users and roles**: users are local. `User` (`src/Domain/Entities/User.cs`, table `Users`) holds `PasswordHash` and a single `RoleId` → `Role` (`src/Domain/Entities/Role.cs`, table `Roles`). The roles are seeded by `RoleDataInitializerCommand` (5 roles: 1 ร้านยา, 2 สิทธิ์ดูรายงาน, 3 ผู้จัดการโครงการ, 8 Admin, 9 ผู้ดูแลระบบ (Host)). A `UserRoles` join entity (`src/Domain/Entities/UserRoles.cs`, key `RoleID` + `UserID`, managed through `UserRoleAssignments`) also exists, but login and authorization do not read it — the token's role comes from `User.RoleId` only.
- **Permissions**: `module.action` codes declared as constants in `src/Application/Common/Security/Permissions.cs` (today only `pharmacies.view`, `pharmacies.create`, `pharmacies.update`), put on a Command/Query with `[RequirePermission(...)]` and enforced by `AuthorizationBehaviour`: the user must be authenticated, a user whose role **name** is `Admin` (`HasAdminRole`) passes every check, and anyone else needs a `permission` claim with that exact code. There is no `RolePermission` entity/table yet, and nothing issues `permission` claims (the JWT carries only the role), so right now only `Admin` users pass a `[RequirePermission]` check — mapping roles to permissions is still to be designed. Currently rolled out to the Pharmacy feature only.
- **Audit trail**: every insert/update/delete on an `EntityBase`-derived entity is recorded to the append-only `AuditLog` table (entity name, `ExternalId`, action, property-level `{old,new}` diff, actor, timestamp) by `AuditableEntitySaveChangesInterceptors` — this is in addition to, not instead of, the `CreatedBy`/`LastModifiedBy` fields, which only ever show the *latest* change.
- **Money balances are never mutated in place** — see the ledger pattern in [coding-rules.md](coding-rules.md).

## Frontend architecture (target — see roadmap Phase 3)

- Dedicated `create/`, `edit/[id]`, `view/[id]`, `list/` routes per feature (not modal dialogs) — file-based routing under `src/pages/{feature}/`.
- A schema-driven CRUD scaffold (field schema → generic `EntityListPage`/`EntityFormPage`/`EntityViewPage`) instead of hand-written per-feature pages.
- One HTTP client, generated from the backend's OpenAPI spec — no hand-typed endpoint strings.
- Shared composables (`useCrud`, `useConfirmDelete`) instead of copy-pasted per-page state/logic.
- CASL permission rules driven by the backend's real permission system (`module.action`), not hardcoded.

## Where things live — quick index

| Concern | Path |
|---|---|
| Domain entities | `src/Domain/Entities/` |
| Domain base types | `src/Domain/Common/` |
| CQRS use cases | `src/Application/Features/{Feature}/Commands/{Create\|Update\|Delete}/` and `.../Queries/Get/` |
| Pipeline behaviours | `src/Application/Common/Behaviours/` |
| DbContext + interface | `src/Infrastructure/Persistence/CpaDatabaseContext.cs`, `src/Application/Common/Interfaces/ICpaDatabaseContext.cs` |
| EF configurations | `src/Infrastructure/Persistence/Configurations/` |
| Migrations | `src/Infrastructure/Persistence/Migrations/` |
| API controllers | `src/API/Controllers/` |
| Frontend pages | `src/vuewebui/src/pages/{feature}/{create,edit,view,list}/` |
| Frontend composables | `src/vuewebui/src/composables/` |
| Frontend API client | `src/vuewebui/src/utils/api.js` (target: generated client, see roadmap Phase 3) |

See also: [coding-rules.md](coding-rules.md), [ai-agent-guide.md](ai-agent-guide.md), [domain.md](domain.md), [known-issues.md](known-issues.md).
