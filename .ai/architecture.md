# Architecture

## What this system is

CPA Thai Project is the foundation of a small web application for Community pharmacy Association (Thailand) (สมาคมเภสัชกรรมชุมชน (ประเทศไทย)). Current scope: master data (ระบบตั้งค่าข้อมูลพื้นฐาน) and pharmacy management (ระบบบริหารข้อมูลร้านยา). Planned scope: the full — see [domain.md](domain.md) and [roadmap.md](roadmap.md).

This is infrastructure a pharmacy depends on operationally. Data accuracy, consistency, and traceability are the top design priority — see the "Non-negotiable" section of [roadmap.md](roadmap.md) before making any change that touches money, quantity, or approval state.

## Stack

- **Backend**: C# / .NET 10, ASP.NET Core Web API, EF Core 10 + Npgsql (PostgreSQL), MediatR 14 (CQRS), FluentValidation, AutoMapper, Swashbuckle/NSwag (OpenAPI).
- **Frontend**: Vue 3 (Composition API, `<script setup>`, JavaScript not TypeScript), Vuetify 3 (Vuexy admin template), Pinia, vue-router 4 via file-based routing (`unplugin-vue-router`), CASL for permission-based UI.
- **Auth**: Internal provider.
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
- **Application** (`src/Application`): CQRS use cases under `Features/{Context}/{Feature}/{Commands|Queries}/{UseCase}/`, each folder containing a Command/Query record + FluentValidation validator + MediatR handler together. Talks to persistence only through `ICpaDbContext` (defined here, implemented in Infrastructure). Pipeline behaviours (`Common/Behaviours/`): `UnhandledExceptionBehaviour` → `AuthorizationBehaviour` → `ValidationBehaviour` → `PerformanceBehaviour` → `LoggingBehaviour`, wired in `Application/DependencyInjection.cs`.
- **Infrastructure** (`src/Infrastructure`): `CpaDbContext` (EF Core, PostgreSQL, snake_case naming via `EFCore.NamingConventions`), `IEntityTypeConfiguration<T>` classes under `Data/Configurations/`, migrations under `Data/Migrations/`, save-changes interceptors under `Data/Interceptors/`.
- **API** (`src/API`): thin controllers under `Controllers/` — inject `Mediator`, call `Send`, return the result. No business logic here. Global exception handling via `Filters/ApiExceptionFilterAttribute`.

## Bounded-context folder map

`Domain/Entities` and `Infrastructure/Data/Configurations` are organized by bounded context (landed in Phase 2). `Application/Features` was moved to match the same physical layout, but namespaces there were deliberately left as `BigLion.Cpa.Application.Features.{Feature}...` rather than rewritten to `...Features.{Context}.{Feature}...` — the physical move gets the folder legibility benefit without a several-hundred-file `using` ripple. A new feature's Domain entity and EF configuration should land under the same `{Context}/{Feature}` path; where the Application slice's namespace ends up is a separate, lower-stakes decision.

- `MasterData/` — Province/District/SubDistrict, Prefix 
- `Pharmacy/` — `Group` (single entity + `PharmacyGroup` discriminator + JSONB `Attributes` — see [domain.md](domain.md))
- `Patient/` — Contract, ContractItem, ContractType, ContractCollateral, ContractCommittee(Type), ContractVendor
- `Security/` — `Permission` (module.action catalog), `RolePermission` (role-name → permission grant; role name matches the external IdP's role claim, no local Role/User table)
- `Domain/Common/` — cross-cutting types with no single bounded context: `EntityBase`, `AuditLog`, `AuditAction`

## CQRS / MediatR conventions

- One folder per use case: `Features/{Context}/{Feature}/Commands/{VerbNoun}/` or `.../Queries/{GetNoun}/`.
- Command/Query, Validator, and Handler live together in that folder (this project's convention — co-located, not split across separate projects).
- Controllers call `Mediator.Send(...)` and nothing else.
- Use the scaffolding template (see [ai-agent-guide.md](ai-agent-guide.md)) to generate a new use case rather than hand-copying an existing one — it keeps the integrity mechanisms (concurrency token, audit log, guard clauses) consistent by default.

## Data model conventions

- **Dual key**: every entity has an internal `int Id` (joins/FKs, never exposed) and a `Guid ExternalId` (UUIDv7, the only identifier exposed via API routes/DTOs).
- **Audit fields**: `CreatedBy`, `LastModifiedBy`, `CreatedOn`, `LastModified`, `IsActive`, `IsDelete` on every entity via `EntityBase`, set automatically by `AuditableEntitySaveChangesInterceptors` — handlers should never set these manually.
- **Optimistic concurrency**: every entity carries the Postgres `xmin` system column as a shadow-property concurrency token (via `ConfigureEntityBase<T>()`, no extra column needed) — a stale write throws `DbUpdateConcurrencyException` rather than silently overwriting.
- **Permissions**: `module.action` codes (`Permission`) granted to a role name (`RolePermission`) — role name matches the claim issued by the external IdP, so there's no local Role/User table. Checked via `[RequirePermission("module.action")]` on a Command/Query, enforced by `AuthorizationBehaviour`; `HasAdminRole` bypasses the check. Currently rolled out to the Vendors feature only — see roadmap Phase 2.
- **Audit trail**: every insert/update/delete on an `EntityBase`-derived entity is recorded to the append-only `AuditLog` table (entity name, `ExternalId`, action, property-level `{old,new}` diff, actor, timestamp) by `AuditableEntitySaveChangesInterceptors` — this is in addition to, not instead of, the `CreatedBy`/`LastModifiedBy` fields, which only ever show the *latest* change.
- **Money/quantity fields are never mutated in place** — see the ledger pattern in [coding-rules.md](coding-rules.md) and the "Non-negotiable" section of [roadmap.md](roadmap.md).
- **Item/Catalog**: a single `Item` entity with an `ItemCategory` discriminator and a JSONB `Attributes` column for category-specific fields (e.g. drug registration number), instead of one class per category — new item categories are a data change, not a schema migration.

## Frontend architecture (target — see roadmap Phase 3)

- Dedicated `create/`, `edit/[id]`, `view/[id]`, `list/` routes per feature (not modal dialogs) — file-based routing under `src/pages/{feature}/`.
- A schema-driven CRUD scaffold (field schema → generic `EntityListPage`/`EntityFormPage`/`EntityViewPage`) instead of hand-written per-feature pages.
- One HTTP client, generated from the backend's OpenAPI spec — no hand-typed endpoint strings.
- Shared composables (`useCrud`, `useConfirmDelete`) instead of copy-pasted per-page state/logic.
- CASL permission rules driven by the backend's real permission system (`module.action`), not hardcoded.

## Where things live — quick index

| Concern | Path |
|---|---|
| Domain entities | `src/Domain/Entities/{Context}/` |
| Domain base types | `src/Domain/Common/` (`EntityBase`, `AuditLog`, `AuditAction`) |
| CQRS use cases | `src/Application/Features/{Context}/{Feature}/{Commands\|Queries}/{UseCase}/` |
| Pipeline behaviours | `src/Application/Common/Behaviours/` |
| DbContext + interface | `src/Infrastructure/Data/CpaDbContext.cs`, `src/Application/Common/Interfaces/ICpaDbContext.cs` |
| EF configurations | `src/Infrastructure/Data/Configurations/{Context}/` |
| Migrations | `src/Infrastructure/Data/Migrations/` |
| API controllers | `src/API/Controllers/` |
| Frontend pages | `src/vuewebui/src/pages/{feature}/{create,edit,view,list}/` |
| Frontend composables | `src/vuewebui/src/composables/` |
| Frontend API client | `src/vuewebui/src/utils/api.js` (target: generated client, see roadmap Phase 3) |

See also: [coding-rules.md](coding-rules.md), [ai-agent-guide.md](ai-agent-guide.md), [domain.md](domain.md), [known-issues.md](known-issues.md), [roadmap.md](roadmap.md).
