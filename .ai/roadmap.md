# Roadmap

Living document. Tick items as they actually land (not when planned) and keep the "Status" line per phase current — this is the single file a new session should read to know exactly what's done, in progress, and next. Update this file at the end of every work session that completes something from it. Full analysis and rationale: see the original plan at `/Users/et/.claude/plans/hey-please-analyze-this-nifty-church.md` (local to the machine this was planned on — the content below is the durable, in-repo copy).

## Decisions already made (don't re-litigate without new information)

- **PPR/PR/PO** = Procurement Plan → Purchase Requisition → Purchase Order, Contract alongside/after PO. See [domain.md](domain.md).
- Frontend CRUD: **dedicated create/edit/view routes** everywhere, not modal dialogs.
- Item/Catalog: **single `Item` entity + JSONB attributes**, not class-per-category.
- **Dual-key pattern** adopted now: internal `int Id` + `Guid ExternalId`.
- **API routes are lower-kebab-case** end to end (backend route segments and every frontend call) — see [coding-rules.md](coding-rules.md).
- **During this development phase, EF migrations are disposable**: when the model changes, drop the dev database, delete existing migrations, regenerate a fresh `InitialCreate`, and `dotnet ef database update` — no production data exists yet, so preserving migration history isn't a concern until the project is deployed somewhere real. Re-evaluate this once there's real data to protect.
- **Tests never depend on Docker**: functional/integration tests use a local, dedicated PostgreSQL database (`inventory-test`), not Testcontainers. See the Testing strategy section below.

## Non-negotiable: data integrity, consistency, traceability

This system underlies hospital procurement/budget/stock decisions. A plausible-looking but wrong number is worse than an outage. These apply across every phase below, as blocking requirements:

1. Money/quantity fields are never mutated in place — always derived from an append-only ledger.
2. Every write to a tracked entity is attributable; approvals are immutable once recorded (corrections are new reversing entries, never edits).
3. Optimistic concurrency on every mutable entity — conflicting writes fail loudly, never silently overwrite.
4. Multi-aggregate state changes are transactional.
5. Validation is layered: FluentValidation + domain guard clauses + DB constraints.
6. Handler-level unit tests + functional tests are merge-blocking for anything touching money/quantity/approval state.
7. Materialized totals must be reconcilable against their ledger on demand.

## Phase 0 — Bootstrap `.ai/` documentation

**Status: complete (2026-08-17).**

- [x] `.ai/architecture.md`
- [x] `.ai/coding-rules.md`
- [x] `.ai/ai-agent-guide.md`
- [x] `.ai/domain.md`
- [x] `.ai/known-issues.md`
- [x] `.ai/roadmap.md` (this file)
- [x] Root `CLAUDE.md` dispatcher

**Exit criteria**: all files above exist and are populated (not stubs). `known-issues.md` and this file are treated as living documents from here on. **Phase 0 complete as of 2026-08-17.**

## Phase 0b — Team development workflow + AI prompt templates

**Status: complete (2026-08-17).** Requested 2026-08-17, landed before Phase 2 feature work starts so the team has it from day one rather than retrofitting it later.

**Why**: the team will use AI agents to build features, and re-explaining project context (conventions, layering, testing setup) from scratch every session is wasted effort and produces inconsistent results. Modeled on the `cwie` reference project's `.ai/dev-workflow.md` + `.ai/remaining-work-prompts.md` pattern (a shared "preamble" block establishing context/rules, plus one reusable template per task type that a developer appends their specific requirement to) — but restructured so the templates are **forward-looking and reusable per task type**, not a historical log of one-off completed work items like cwie's version.

- [x] `.ai/dev-workflow.md` — human-facing runbook: prerequisites, install, run, test, build, migrations, scaffolding. Deliberately **excludes deployment** (out of scope for now, per instruction — add a follow-up once deploy is designed).
- [x] `.ai/prompt-templates.md` — shared preamble + one reusable template per pipeline stage (entity+EF config, CQRS feature, domain unit test, application functional test, frontend feature, Playwright e2e spec).
- [x] Cross-linked both new files from `CLAUDE.md`'s "read first" list and from `.ai/ai-agent-guide.md`.

**Exit criteria**: a team member (or a fresh AI session) can open `.ai/prompt-templates.md`, copy the preamble + the one template matching their task, append a one-paragraph requirement, and get a correctly-scoped, convention-following result without re-deriving project context. Verify with one dry run per template stage during Phase 2's Vendors pilot.

## Phase 1 — Stabilize

**Status: complete (2026-08-17).**

- [x] Fix `suth-inventory.sln`/`.slnx` stale `src/Web/Web.csproj` reference (also fixed a `VueWebUI`/`vuewebui` casing mismatch)
- [x] Fix `Domain.UnitTests` compile errors (renamed entities/properties, stale constructor)
- [x] Fix `Application.UnitTests`/`Application.FunctionalTests` restore failures (`Directory.Packages.props`, redundant `System.Text.Json` reference)
- [x] Finish `ContractProducts` → `ContractItems` rename (folder + namespace + class + route all agree) — also split Validators into their own files, added explicit EF relationship/key/constraint configuration, fixed a `CatalogItemId = request.ContractId` copy-paste bug in the Create handler
- [x] Fix swapped 401/403 handling in `ApiExceptionFilterAttribute`; rename the file (trailing space); delete dead `ExceptionMiddleware`
- [x] Wire up Serilog (console + rolling file sinks, `UseSerilogRequestLogging`)
- [x] Remove dead scaffolding: `Colour`/`UnsupportedColourException`, unused `IUser`, duplicate `AddDbContext` call, duplicate `QueryableExtensions.FilterJsonb`
- [x] Clean up `ICurrentUserService`/`AuthorizationBehaviour` leftover "queue system" claims/message
- [x] Fix `/Venders` → `/Vendors` typo (plus response-shape and action-name fixes)
- [x] Remove frontend dead code (16 unused dialogs, dead `eslint.config.js`, `dashboards/test.vue`, nonexistent `fake-api` aliases; `navigation/horizontal/*` deliberately kept — see known-issues.md)
- [x] Update `known-issues.md` to mark each resolved

**Not originally scoped, found and fixed while stabilizing** (see [known-issues.md](known-issues.md) "Critical runtime bugs" for full detail — these were more serious than the mechanical fixes above):
- [x] `IUser` had zero DI registration, meaning every MediatR request would have thrown at runtime — consolidated onto `ICurrentUserService`.
- [x] `AuthorizationBehaviour`'s auth check was permanently dead (`EmployeeId` defaulted to the string `"null"`, not an actual null) — fixed by making `ICurrentUserService` properties genuinely nullable.
- [x] `UpdateContractItemCommandHandler` silently reactivated/undeleted every item it touched — fixed.
- [x] 13 ViewModels' AutoMapper config silently dropped status/audit/postal-code fields across most of the master-data UI — fixed.
- [x] `pages/sectors/list/index.vue` had completely non-functional delete/pagination (undefined variables from a copy-pasted "doctors" template) — rewritten against the real API.
- [x] Frontend `npm install` was fully broken (chain of peer-dependency version conflicts: apexcharts, video.js, vite-plugin-vue-devtools, vite-plugin-vuetify) — `dotnet build` on the solution could never have succeeded until these were fixed.
- [x] `pnpm run lint`/`npm run lint` was silently broken by the vestigial `eslint.config.js` hijacking ESLint into flat-config mode — fixed by deleting it; the 44 real lint errors this uncovered are now also fixed.

**Exit criteria — met**: `dotnet build suth-inventory.sln` succeeds (0 errors); `dotnet test suth-inventory.sln` succeeds (21 Domain.UnitTests + 4 Application.UnitTests passing, 0 failures); `npm run lint` and `npm run build` both succeed clean in `src/vuewebui`. This was a floor, not a finish line — Phase 2 is next.

## Phase 1.5 — Pre-Phase-2 prep (routing, migrations, test infrastructure)

**Status: complete (2026-08-17).** Requested ahead of Phase 2 proper — small, mechanical, but each item would have compounded into a bigger problem once Phase 2's bounded-context reorganization and dual-key migration landed on top of it.

- [x] Remove `src/Web/` entirely (the original Clean-Architecture-template project, superseded by `API`) rather than just unreferencing it.
- [x] Standardize API routing to lower-kebab-case everywhere: fixed hand-written literal route segments that the existing `SlugifyParameterTransformer` doesn't reach (`VendorsController`'s `SapCode`/`GetVendorList` → `sap-code`/`get-vendor-list`); audited and fixed **every** frontend API call across every page to use the real, correct, kebab-case path — many were previously calling nonexistent sub-paths entirely (e.g. `/Building/GetAllBuildingList`, `/ContractBonds/...` for a controller actually named `ContractCollaterals`), not just wrong casing. See [known-issues.md](known-issues.md) for the full per-page list.
- [x] Removed the orphaned `CatalogItem`/`PhysicalItem`/`ServiceItem` entities (never mapped, and found to actively block `dotnet ef migrations add` by making EF try to instantiate an abstract type) — `ContractItem.CatalogItemId` is now a plain `int` pending the real `Item` entity in Phase 2.
- [x] Dropped the dev database, deleted the stale `InitialCreate` migration, regenerated a fresh one against the current model, and applied it — verified all 19 tables and the Phase 1 FK/index/delete-behavior additions on `contract_items` landed correctly.
- [x] Created dedicated local PostgreSQL databases: `inventory` (dev), `inventory-test` (functional tests).
- [x] Removed `PostgreSQLTestcontainersTestDatabase.cs` and the `Testcontainers.PostgreSql`/`Microsoft.Data.SqlClient` package references entirely — local Postgres only, no Docker dependency for tests, per project convention.
- [x] Added Playwright e2e scaffolding (`src/vuewebui/e2e/`) — config, global setup, session-injection auth helper, base page object, empty `specs/` — no test cases yet. See the Testing strategy section below.

## Testing strategy

This is what "tests" means in this repo, and where each kind lives. See [coding-rules.md](coding-rules.md) for the binding conventions this section summarizes.

**Backend (`tests/`)**
- `Domain.UnitTests` — NUnit, one class per entity, covers constructor/guard-clause invariants. No external dependencies. **21 tests passing.**
- `Application.UnitTests` — NUnit, covers cross-cutting Application-layer concerns (AutoMapper config validity, pipeline behaviours, mapping tests, and — as of 2026-08-19 — FluentValidation validator logic against a mocked `IInventoryDbContext` via `TestDbSetMock`, see `UnitOfMeasures/` and `ServiceGroups/`). No database. **17 tests passing.**
- `Application.FunctionalTests` — NUnit + `Microsoft.AspNetCore.Mvc.Testing`, exercises real MediatR handlers end-to-end through a `WebApplicationFactory`, against the **local dedicated `inventory-test` PostgreSQL database** (not Testcontainers — see the decision above). `PostgreSQLTestDatabase` drops/recreates the schema per test run and resets rows between tests via Respawn. **5 tests, `Vendors/` only, as of 2026-08-18** — first real coverage (see roadmap.md Phase 2/3 pilot), establishing the pattern: `CreateVendorCommandTests`, `VendorUpdateCommandTests` (including the concurrency-conflict test). Adding functional tests for the rest (one file per command/query, mirroring `Application/Features/`) is expected to happen alongside each feature's own ExternalId/pilot rollout, and is **merge-blocking for anything touching money, quantity, or approval/workflow state** per the Non-negotiable section above.
- `Infrastructure.IntegrationTests` — exists, builds, has zero test files. No clear scope defined yet; decide during Phase 2 whether this project earns real content (e.g. EF configuration/constraint tests against a real Postgres) or gets folded into `Application.FunctionalTests`.

**Frontend (`src/vuewebui/`)**
- Unit tests: the `.esproj` declares `JavaScriptTestFramework=Vitest`, but Vitest is not actually installed or configured — no frontend unit tests exist. Not addressed in this pass; revisit if/when the schema-driven CRUD scaffold (Phase 3) introduces enough shared logic (composables, schema validation) to be worth unit testing in isolation from the browser.
- E2E: **Playwright**, scaffolded at `src/vuewebui/e2e/` (`playwright.config.ts`, `global-setup.ts`, `helpers/`, `pages/base.page.ts`, empty `specs/`) — modeled on the `cwie` reference project's e2e setup. **No test cases yet, by design.** Two things need to happen before real specs can be written:
  1. **Authenticated sessions**: this project uses external OIDC (Authorization Code flow), not a local login endpoint like `cwie` has — there's no simple token to script. `e2e/helpers/auth.ts#obtainE2eTestUser()`'s real-credentials path is still an unimplemented placeholder pending the identity team provisioning a dedicated E2E test client/grant. **Update (2026-08-19)**: a local-only fake-auth path now exists (`E2E_USE_FAKE_AUTH=true`, paired with the API's `Identity:UseFakeAuth` and the UI's `VITE_USE_FAKE_AUTH`) — see known-issues.md — so specs can actually run against a developer's own machine today; it is not a substitute for the real credentials CI will eventually need.
  2. **`data-testid` attributes**: none of the current pages have them. Add `{feature}-{element}`-named test IDs as each page gets its first spec (see `e2e/README.md`'s "Adding a new screen" section) — don't do a blanket pass across all pages up front, since Phase 3 will rewrite most of them onto the schema-driven scaffold anyway.

**When to add real tests**: as part of the Phase 2/3 pilot (re-implementing Vendors end-to-end, see that phase's exit criteria) is the natural first place for both a real functional test and a real Playwright spec to exist, establishing the pattern before it's copied across the other ~16 features.

## Phase 2 — Backend foundational restructure

**Status: mostly complete (2026-08-18).** One item deliberately deferred (see below) — everything else landed and is verified via `dotnet build`/`dotnet test` (21 Domain.UnitTests + 4 Application.UnitTests passing) plus live smoke tests against the local `inventory` dev database for the parts that only show up at runtime (audit interceptor, permission seed).

- [x] Reorganize `Domain/Entities`, `Application/Features`, `Infrastructure/Data/Configurations` by bounded context (`ReferenceData`/`Catalog`/`Contracts`/`Procurement`/`Security`) — `Inventory/` still empty, pending Phase 5. Domain entities got real namespace changes; Application/Features and Infrastructure/Configurations were moved physically with namespaces left as-is, to avoid a multi-hundred-file `using` ripple with no functional benefit.
- [x] Add `Guid ExternalId` (UUIDv7 via `Guid.CreateVersion7()`) to `EntityBase`, unique-indexed on every table.
- [x] Migrate routes/DTOs/controllers from `int Id` to `Guid ExternalId` — done for **Vendors only** (2026-08-18), as the pilot; the other ~18 entities are unchanged and still deliberately on `int Id`. `VendorsController`'s `{id}` routes are now `{id:guid}`; `CreateVendorCommand`/`GetVendorQuery` return/accept `Guid`; `VendorViewModel.Id` is `Guid`, mapped from `Vendor.ExternalId` (not `Vendor.Id` — the internal `int Id` is never touched by any of this and stays exactly as-is for every FK/join). Verified: build+tests green, a direct EF smoke test confirming `WHERE ExternalId == guid` round-trips correctly against Postgres. **Not yet done**: the other 18 features, and (per the exit criteria below) the frontend/functional-test/Playwright parts of the pilot.
- **Design note for the rollout to the other 18**: the DTO/route parameter stays *named* `Id` (not renamed to `ExternalId`) — its type just changes from `int` to `Guid`, carrying the entity's `ExternalId` value. This keeps `route.params.id`, `item.id`, and every existing frontend reference working unchanged; only the value shape changes (a number becomes a UUID string), which plain JS doesn't care about. Confirmed via the Vendors list page (dialog-based CRUD, `item.id` used opaquely everywhere, no `parseInt`/numeric assumptions) needing **zero** frontend changes for this part.
- [x] Build the `Item` entity (`Catalog` context — single entity + `ItemCategory` discriminator + JSONB `Attributes`), wired `ContractItem.CatalogItemId` → `ItemId` with a real FK (`Restrict`) + EF config. Frontend `catalogItemId` field renamed to `itemId` to match (`pages/contract-products/list/index.vue`).
- [x] Centralized EF audit-field/dual-key/concurrency configuration into `EntityBaseConfigurationExtensions.ConfigureEntityBase<T>()`, applied to all 20 EF configurations (was hand-repeated and inconsistently done across ~18 files before — see known-issues.md for the real bugs this surfaced).
- [x] Fixed `AuditableEntitySaveChangesInterceptors` to actually set `CreatedBy`/`LastModifiedBy` from `ICurrentUserService` (previously never set despite the columns being `NOT NULL` — see known-issues.md).
- [x] Real permission system: `Permission` + `RolePermission` entities (`Security` context, `module.action` codes, role-name join against the external IdP's role claims — no local Role/User table), `[RequirePermission]` attribute wired into `AuthorizationBehaviour` (Admin bypass via `HasAdminRole`), seed catalog + Administrator grant wired into `InventoryDbContextInitialiser` (also fixed: `Program.cs` never called `SeedAsync()` at all before this — seed data would never have run). Applied to the **Vendors** feature only (`vendors.view`/`.create`/`.update`) as the pilot; every other feature is still authorization-open pending the same rollout.
- [x] Custom `dotnet new` scaffolding templates: `suth-command` and `suth-query` (`templates/suth-templates/`), bounded-context- and feature-aware via `--boundedContext`/`--featureName`/`--returnType` parameters. Verified end-to-end (installed, generated, inspected output) — see [ai-agent-guide.md](ai-agent-guide.md) for usage. Deliberately scoped to the CQRS slice only, not Controller/EF config too — those vary too much per feature to templatize safely; the prompt-templates.md workflow covers them.
- [x] Guard-clause invariants (`Ardalis.GuardClauses`) added to all 19 entity constructors (Domain project didn't even reference the package before this). `Contract` and `ContractItem` — the two entities carrying money/quantity — additionally got real `Update(...)` behavior methods replacing handler-side `entity.Property = value` mutation, plus a domain-level `EndDate > StartDate` invariant. The remaining 17 entities still take direct property mutation in their Update handlers; extending behavior methods to those is not yet done (lower risk — none carry money/quantity).
- [x] Optimistic concurrency: every entity now carries the Postgres `xmin` system column as a shadow-property concurrency token (`builder.Property<uint>("xmin").IsRowVersion()` inside `ConfigureEntityBase`) — no extra column needed.
- [x] Generic `AuditLog` table + interceptor: append-only, captures Created/Updated/Deleted with a property-level `{old,new}` diff (bookkeeping fields and the temp pre-insert `Id` excluded from the diff), keyed by `ExternalId`. Verified with a live smoke test (insert → update → delete → confirmed 3 correct log rows → cleaned up).
- [x] Database-level `CHECK` constraints + explicit FK delete-behavior: `contracts` (`end_date > start_date`, all money fields `>= 0`), `contract_items` (`quantity > 0`, `unit_price >= 0`); every FK across ReferenceData/Contracts that previously relied on EF's silent default now has an explicit `Restrict` (lookups/hierarchy) or `Cascade` (true child records: `ContractItem`/`ContractCommittee`/`ContractVendor` → `Contract`) decision.
- [x] **Superseded `EntityBase`'s bare `IsDelete`/`IsActive` bools with a real soft-delete model + a global EF query filter (2026-08-21).** `EntityBase` no longer carries either; `SoftDeletableEntityBase` (`IsDeleted`/`DeletedOn`/`DeletedBy` + a `MarkAsDeleted()` behavior method) and `IHasActiveFlag` (opt-in, entity-declared) replace them, informed by the `cwie` reference project's own split. A reflection-based global query filter (`InventoryDbContext.OnModelCreating`) now excludes soft-deleted rows from every query automatically — the single biggest structural gap this closed, since only 4 of 36 `Get*Query` handlers filtered `IsDelete` manually before this. Full design rationale, per-entity decisions (18 get both flags, 2 get soft-delete only, 5 stay hard-delete-only), and verification are in [known-issues.md](known-issues.md)'s Resolved section — this bullet is the pointer, not the detail, since the full writeup is long.
- [ ] Update `architecture.md` bounded-context map — mostly already accurate (it was written forward-looking); needs the `Security` context and `AuditLog`/`Permission`/`RolePermission` added.

**Real bugs found and fixed along the way** (not originally scoped, discovered while doing the above — see [known-issues.md](known-issues.md) for full detail):

- `DistrictConfiguration`/`ProvinceConfiguration` were fully cross-wired: the class named `DistrictConfiguration` configured `Province`-shaped data into a table literally named `districts`, and vice versa — confirmed live in the dev DB (`districts` table had a `region` column and no `district_id`). Both were swapped correctly.
- `DivisionConfiguration` had its entire audit-field block duplicated verbatim.
- `SubDistrictConfiguration` was missing `IsDelete`/`CreatedBy`/`LastModifiedBy`/`CreatedOn`/`LastModified` configuration entirely.
- `CreateContractsCommandHandler` constructed `Contract` via its constructor *and* redundantly re-set every property again via object initializer.
- `UpdateContractsCommandHandler` silently set `IsActive = true; IsDelete = false;` on every edit — an update endpoint un-deleting a record as a side effect. Removed.
- The migration folder itself drifted: regenerating `InitialCreate` without an explicit `--output-dir` landed it in `src/Infrastructure/Migrations/` instead of the project's actual `Data/Migrations/` convention, with a mismatched namespace. Caught and fixed before it became a second migrations folder nobody noticed.
- `Program.cs` called `InventoryDbContextInitialiser.InitialiseAsync()` (migrate) but never `SeedAsync()` — any future seed data would have silently never run in the real app despite `TrySeedAsync` looking like it worked.

**Exit criteria**: see the Phase 2/3 pilot verification below.

## Phase 3 — Frontend foundational restructure

**Status: not started as a formal phase, but its core pattern (dedicated routes + shared form component, no modals) now has two real reference implementations — `unit-of-measures` and `service-groups`, both 2026-08-19.** Requested directly ahead of the formal phase, as "the easiest feature, showing dedicated create/edit/list/view routes + a shared form + a first e2e showcase" — `UnitOfMeasure` chosen first because it has no entity dependencies; `ServiceGroup` picked next because it already had dedicated routes and only needed the shared-form/delete/e2e pieces to reach parity — except investigating it first surfaced that its Update flow was **completely broken** (route mismatch + a validator bug rejecting every deactivation + a missing form field — see known-issues.md), so it ended up needing the same full treatment. Not a schema-driven scaffold (that's still the Phase 3 goal below) — each page is hand-written, matching this project's own existing conventions (hardcoded Thai text, direct `ProgressDialog`/`ConfirmProgressDialog` usage, no i18n/CASL), not `cwie`'s composable library (`useCrud`/`useProgressAction`/etc. don't exist here yet and weren't introduced for these two features).

- [x] `unit-of-measures` rebuilt: dedicated `create`/`edit/[id]`/`view/[id]`/`list` routes (previously `create`/`edit`/`view` were dead 5-line redirect stubs and `list` did everything through two `VDialog`s); new `_components/UnitOfMeasureForm.vue` shared by create+edit; `data-testid="{feature}-{element}"` attributes added throughout (first feature to have them, per `e2e/README.md`'s documented-but-previously-unused convention).
- [x] `service-groups` rebuilt the same way: new `_components/ServiceGroupForm.vue`; `view/[id].vue` taken from a half-finished stub (commented-out edit button, no delete) to a full detail page; `list/index.vue` gained a delete action (previously had none). Three real, independent bugs fixed along the way that had made ServiceGroup editing 100% non-functional — see known-issues.md for the full detail.
- [x] First two real Playwright specs: `e2e/specs/unit-of-measures/crud.spec.ts` and `e2e/specs/service-groups/crud.spec.ts` (`@unit-of-measures`/`@service-groups` tags, `pnpm test:e2e:unit-of-measures`/`pnpm test:e2e:service-groups`) — full create → view → edit → delete flow each, with matching Page Object Model files under `e2e/pages/`. Both **actually run and pass** (not just skip) locally via the fake-auth path (`E2E_USE_FAKE_AUTH=true`) — see known-issues.md. This is the pattern the next feature's spec should copy.
- [ ] Consolidate to one HTTP client (`ofetch`-based `$api`; delete `composables/useApi.js`)
- [ ] Generate a typed API client from the backend OpenAPI spec
- [ ] Schema-driven CRUD scaffold (`EntityListPage`/`EntityFormPage`/`EntityViewPage` + field-schema config) — `unit-of-measures`/`service-groups` show the target *routing* shape but are still hand-written per-field, not schema-driven
- [ ] Shared composables (`useCrud`, `useConfirmDelete`, `useHighlightRow`)
- [ ] Wire CASL to the real backend permission system
- [ ] Pinia stores for shared reference/lookup data
- [ ] Remove remaining dead template code; consolidate Buddhist-year date formatting into one util
- [ ] Roll the same dedicated-routes + shared-form + e2e pattern out to the other ~16 features (`unit-of-measures`/`service-groups` are the templates to copy, not one-offs) — worth budgeting time to *investigate* each one before assuming it's a clean rebuild, given what turned up in `service-groups`

**Exit criteria (Phase 2/3 pilot)**: re-implement **Vendors** end-to-end through the new patterns before rolling out to the rest. Confirm: `ExternalId`-based routes ✅ (2026-08-18), permission-gated endpoints ✅ (Phase 2), schema-driven frontend pages ⬜, generated API client in use ⬜, no hand-typed endpoint strings ⬜. Manual browser smoke test of the full CRUD flow ⬜ (Vendors specifically — still dialog-based on the frontend). Concurrency-conflict test passes (two simultaneous conflicting writes → one success, one detected conflict) ✅ (2026-08-18). This is also the natural point to add the **first real functional test** ✅ (`Application.FunctionalTests/Vendors/`, against `inventory-test`, 2026-08-18) and **first real Playwright spec** — landed ✅ but via **`unit-of-measures`, not Vendors** (2026-08-19, see Phase 3 above) since it was requested directly as the dependency-free starting point; Vendors' own frontend/API-client/Playwright rollout is still pending.

**First functional tests landed (2026-08-18)** — `CreateVendorCommandTests`, `VendorUpdateCommandTests` (5 tests, all passing against the local `inventory-test` database). Two real, previously-undiscovered bugs found and fixed in the test scaffolding itself along the way — the scaffolding had never actually been exercised before this:
- `Testing.cs`'s `RunAsUserAsync` (and commented-out `RunAsDefaultUserAsync`/`RunAsAdministratorAsync`) called `GetRequiredService<UserManager<ApplicationUser>>()`, but this project registers no ASP.NET Identity anywhere (auth is external OIDC — see the `IdentityService` note in known-issues.md) — calling it would have thrown `InvalidOperationException` immediately. Removed; `CustomWebApplicationFactory`'s mocked `ICurrentUserService` now defaults every test to running as an admin (`HasAdminRole = true`, non-null `Claims`) instead, since there's no local user store to provision a real test user into.
- `PostgreSQLTestDatabase.InitialiseAsync()`'s migration setup didn't configure `.MigrationsHistoryTable("__ef_migrations_history")`/`.UseSnakeCaseNamingConvention()` to match `Infrastructure/DependencyInjection.cs`'s real configuration — so it tracked the `InitialCreate` migration in EF's default-named history table, while the actual app host (booted moments later by `CustomWebApplicationFactory` against the same database) checked the custom-named one, saw it empty, and tried to re-run `InitialCreate` from scratch against tables that already existed. Fixed by matching the real app's configuration exactly.
- The concurrency-conflict test bypasses `SendAsync` for both sides deliberately (two independent `InventoryDbContext` instances from two DI scopes, both reading before either writes) — going through `SendAsync` twice in sequence can't reproduce a real race, since the first call's handler reads-updates-saves as one atomic unit before the second call's read even happens.

## Phase 4 — Procurement domain (PPR → PR → PO)

**Status: directional design only — needs a stakeholder session before implementation starts.** See [domain.md](domain.md) for entity/workflow detail.

- [ ] Stakeholder session: approval/delegation-of-authority limits, budget-checking rules, HOSXP integration contract
- [ ] `ProcurementPlan` entity + states
- [ ] `PurchaseRequisition` entity + states
- [ ] `PurchaseOrder` entity + states, linkable to `Contract` as a call-off order
- [ ] `GoodsReceipt` entity
- [ ] Append-only `BudgetEvent`/`BudgetLedger` (budget-remaining derived, never a mutable field)
- [ ] State-transition audit trail (who/when/from/to/why) for PPR/PR/PO
- [ ] Idempotency guard against double-posting a PO/GoodsReceipt
- [ ] Reconciliation test: budget-remaining always equals the sum of its ledger

## Phase 5 — Inventory/Stock foundation

**Status: foundation landed early (2026-08-18), rest still deferred.** Pulled forward out of turn mid-Phase-2 after a real design flaw was spotted: `Location` (the Building/Floor/Department reference-data hierarchy) had a `ContractId` field with no FK and no real meaning — a physical location doesn't belong to one contract, and a single contract line's stock can legitimately be split across multiple physical storage points (e.g. a 10-box pack of Paracetamol received as 5 boxes into Pharmacy 1 and 5 into the central warehouse). That gap couldn't be fixed without at least the core of what Phase 5 was always going to need.

- [x] `Location.ContractId` removed (dead field, no FK, wrong concept) — `CreateLocationCommand`/`UpdateLocationCommand`/`LocationViewModel` updated to match.
- [x] Fixed two real, previously-undocumented bugs found while investigating: `GetLocationByContractIdQueryHandler` queried `_context.Sectors` instead of `_context.Locations` (returned the wrong entity type entirely); `GetLocationQuery` was a dead empty stub and the working `GetLocationsQuery` (list) existed but was never wired into `LocationsController`. Both replaced with a real `GetLocationQuery` (get by id) and `GetLocationsQuery` (list) properly exposed on the controller, both eager-loading `Building`/`Floor`/`Department` so `LocationViewModel`'s `BuildingName`/`FloorName`/`DepartmentName` (AutoMapper flattening) actually populate.
- [x] `Warehouse` entity (`Inventory` context) — a physical stock storage location (e.g. "Pharmacy 1", "Central Warehouse"), distinct from `Location` (org/building hierarchy). Optionally references a `Location` for "where is this warehouse physically" without being a business dependency.
- [x] `StockItem` (Item × Warehouse × on-hand quantity, materialized, unique index, `CHECK (quantity_on_hand >= 0)`) — mutated only through `ApplyMovement(StockMovement)`, never a raw property set.
- [x] Append-only `StockMovement` ledger (`CHECK (quantity <> 0)`) — signed quantity (positive = into the warehouse, negative = out), built via `Receipt`/`Issue`/`Transfer`/`Adjustment` factory methods that enforce the sign convention per type (a `Transfer` returns a linked pair via `TransferGroupId` rather than one row with from/to columns — mirrors double-entry ledger discipline, keeps reconciliation a plain `SUM(quantity)` query). Optional `ContractItemId` for traceability back to the contract/PO line that sourced the stock (`Restrict` on delete — a Contract with real stock movement history can't be deleted out from under its audit trail).
- [x] `ContractItem.LocationId`/`Warehouse` nav **removed** — a contract line item no longer has a single storage location; where its stock physically ends up is captured by `StockMovement` rows (each optionally pointing back to the `ContractItem`), which is what actually allows one line to be split across warehouses.
- [x] One real command, `ReceiveStockCommand` (`POST /stock-movements/receive`), proving the pattern end-to-end: writes the `StockMovement` and updates/creates the matching `StockItem` in one `SaveChangesAsync` call (one DB transaction). Verified live: 5+5 receipt into two warehouses, reconciliation (`materialized == SUM(ledger)`) confirmed, and an over-issue attempt correctly thrown by `StockItem.ApplyMovement`'s negative-stock guard.
- [x] Minimal `Warehouse` CRUD (`CreateWarehouseCommand`, `GetWarehousesQuery`, `WarehousesController`) so the model is actually usable via the API, not inert scaffolding.
- [ ] **Deferred**: `Issue`/`Transfer`/`Adjustment` commands+controller actions (the domain factory methods exist and are covered by the smoke test, but only `Receipt` has an Application-layer command/endpoint — the other three are a mechanical repeat of the same pattern once needed).
- [ ] **Deferred**: concurrency-safe upsert when two concurrent `ReceiveStock` calls race to create the *first* `StockItem` row for a given Item+Warehouse pair (the unique index will correctly reject the second INSERT, but the handler doesn't yet retry/handle that gracefully — it'll surface as a raw DB exception). Existing `StockItem` rows are already protected by the standard `xmin` optimistic-concurrency token from `ConfigureEntityBase()`.
- [ ] Design session: lot/batch/expiry tracking requirement.
- [ ] Scheduled/on-demand reconciliation job (today reconciliation is provable ad hoc via a `SUM(quantity)` query per Item+Warehouse, as demonstrated in the smoke test — not yet automated as a recurring check).
- [ ] Wire `GoodsReceipt` (Phase 4, once PPR/PR/PO exists) as the real trigger for `StockMovement.Receipt`, instead of `ReceiveStockCommand` being called directly.

## Phase 6 — Observability & audit-trail scaling

**Status: not started — needs its own design pass, flagged 2026-08-19.** Raised after first running the app locally and noticing operational logs are only reachable by opening a file on disk (`logs/suth-inventory-*.log`) — no access control, no way for someone other than whoever has server/file access to see what happened. Two related but distinct problems, both deliberately **not implemented yet**:

- [ ] **Centralized, permission-gated log viewing.** Today, tracing an incident means finding and opening a rolling log file directly — no audit trail of *who* looked at logs, no way to grant a non-engineer (e.g. an auditor, IT support staff) visibility without giving them server access. Needs a real design pass before building, but candidate directions: (a) a self-hosted structured-log server with first-class Serilog sink support (e.g. Seq) sitting behind its own auth; (b) a custom in-app "Logs" screen backed by a queryable Serilog sink (e.g. `Serilog.Sinks.PostgreSQL`) and gated by a new `logs.view` permission through the `module.action` system already built in Phase 2; (c) a full ELK/Grafana Loki stack — heavier, more ops overhead than this project likely needs at its current size. Whichever direction, access should be gated by the existing `[RequirePermission]`/`RolePermission` mechanism, not a separate ad-hoc auth story.
- [ ] **Move `AuditLog` to a dedicated database.** `AuditLog` (Phase 2) captures a row for every Created/Updated/Deleted across every `EntityBase`-derived entity — by design, this grows faster than any single business table, and every future feature adds more write volume to it. Keeping it in the same `inventory` database as operational data means: (a) high-write audit traffic competing with low-latency operational queries for the same I/O/cache/connection pool, (b) no way to apply a different backup/retention policy to audit data than to operational data (hospital compliance requirements for audit trails are often a different retention horizon than operational records), (c) awkward to reason about database size/performance as one growing blob instead of two independently-scalable concerns. Plan: a dedicated audit database (its own connection string, likely its own `DbContext`), with `AuditableEntitySaveChangesInterceptors` writing to it instead of the same `InventoryDbContext.ChangeTracker`. **Open design question, not yet answered**: whether the audit write needs to be strictly atomic with the business write it's recording (in which case cross-database transactions or an outbox pattern are needed) or whether "best-effort, reconciled after the fact" is acceptable for audit data specifically — this determines how much complexity the fix actually needs, and shouldn't be assumed either way without a real design session.

## Open questions (not blocking current phase, need answers before their phase starts)

- Exact approval/delegation-of-authority rules for PR/PO (Phase 4) — needs procurement/finance stakeholders.
- Integration contract with legacy system / HOSXP (Phase 4) — what flows which direction, real-time vs. batch.
- Lot/batch/expiry tracking requirement for Stock (Phase 5) — likely yes given drugs are in scope.
- Log-viewing tool choice — Seq vs. a custom in-app screen vs. ELK/Loki (Phase 6) — needs a decision on ops overhead vs. how much control we want over access permissions.
- Whether the audit database write must be strictly atomic with its business write, or can be best-effort/reconciled (Phase 6) — determines whether the fix needs cross-database transactions/an outbox pattern or something much simpler.
