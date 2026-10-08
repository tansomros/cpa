# AI Prompt Templates

Reusable prompts for kicking off each stage of feature work with an AI agent, so nobody has to retype project context every session. Paste the **shared preamble** once at the start of a chat, then paste the template matching your task, and fill in the requirement at the bottom — usually by pointing at (or pasting) a file from [`requirements/`](../requirements/), which is where feature specs written by the team live (see [`requirements/README.md`](../requirements/README.md)). Modeled on the `cwie` reference project's `.ai/dev-workflow.md` + `.ai/remaining-work-prompts.md` pattern, restructured here as reusable-per-task-type templates rather than a log of one-off completed work.

If a template's assumptions turn out to be wrong or stale (a file moved, a convention changed), fix the template — don't just work around it once and move on. See "Keeping these templates current" at the bottom.

---

## Shared preamble (paste at the top of every session)

```
You are working on CPA Thai , a Database System for Health Promotion and Medication Management Services Provided by Community Pharmacists.

Before making any change:
1. Read .ai/known-issues.md (is what I'm about to touch already a tracked
   problem?).
2. Read .ai/coding-rules.md and .ai/ai-agent-guide.md — these are binding
   conventions, not suggestions. Follow them exactly.
3. If the task touches pharmacy/patient/MTM vocabulary, check
   .ai/domain.md for the established terms.
4. If a requirements/{Context}/{feature}.md file is referenced below, read
   it in full before doing anything else — it's the primary spec (goal,
   scope, fields, business rules, acceptance criteria, test cases) written
   by the team in Thai. Treat its "Out of scope" section as a hard
   boundary, and its "Money & Quantity Impact" answer as authoritative on
   whether the ledger rule below applies.

Non-negotiable (see .ai/coding-rules.md) — this system holds real pharmacy,
patient, and health-service data:
- Never overwrite a money balance in place — derive it from an append-only
  ledger if one exists for that concept.
- Every mutable entity needs a concurrency token; conflicting writes must
  fail loudly, never silently overwrite.
- Validation is layered: FluentValidation + domain guard clauses
  (Ardalis.GuardClauses) + DB constraints — don't rely on just one.

When done:
- Run `dotnet build CPA.sln` and `dotnet test CPA.sln`
  (backend), `pnpm run lint` and `pnpm run build` (frontend, from
  src/vuewebui) — report the results, don't just claim success.
- Update .ai/known-issues.md only if something you did actually changes
  its status — don't let it drift from reality, and don't rewrite
  sections that are still accurate.
- Stay within the scope of the task below. If you notice something else
  that's broken, note it in .ai/known-issues.md rather than fixing it
  unasked, unless it's blocking the task itself.
```

---

## 1. Domain entity + EF configuration

```
Design a new domain entity: {EntityName}.

Fields: {list fields, types, required/optional, and any business rules
  — e.g. "EndDate must be after StartDate", "Quantity must be positive"}
Relationships: {e.g. "belongs to Patient (required)", "has many {Entity}Items"}
Feature folder: {Feature} under src/Application/Features
  (for example Banks, Pharmacy, Patients — see .ai/architecture.md)

Do:
- Entity class in src/Domain/Entities/{EntityName}.cs, inheriting
  the project's entity base. Constructor takes required fields; enforce invariants with
  Ardalis.GuardClauses (Guard.Against.NullOrEmpty, .NegativeOrZero, etc.) —
  don't leave a property with a bare public setter if an invalid value would
  break a business rule.
- EF configuration in src/Infrastructure/Persistence/Configurations/
  {EntityName}Configuration.cs: explicit HasOne/HasForeignKey/OnDelete for
  every relationship (Restrict unless cascade is deliberately correct),
  explicit IsRequired()/HasMaxLength() for every scalar — nothing left to
  convention. Register the DbSet in CpaDatabaseContext and
  ICpaDatabaseContext.
- If this entity represents a running money balance, it must be derived
  from an append-only ledger, not a mutable column — ask me if
  you're not sure whether that applies here before implementing.

Don't generate a migration — I'll run `dotnet ef migrations add` myself
(see .ai/dev-workflow.md) after reviewing the model.
```

---

## 2. CQRS feature (Application + API)

```
Implement a {command|query} for {EntityName}: {CreateX | UpdateX | GetX |
  GetXs | ...}.

Behavior: {what it does, required/optional inputs, validation rules beyond
  "field is required" — e.g. cross-field checks, DB-backed uniqueness}

Do:
- Commands: src/Application/Features/{Feature}/Commands/Create,
  Commands/Update, or Commands/Delete.
  Queries: src/Application/Features/{Feature}/Queries/Get
  (single-item and list queries share this folder).
  Folder names are only Create, Update, Delete, and Get — do not append
  the entity. Class names still include it (CreateBankCommand,
  GetBankQuery, GetBankListQuery). Namespace matches the folder:
  BigLion.CPA.Application.Features.{Feature}.Commands.Create
  or ...Queries.Get.
- {Verb}{Noun}Command.cs or Get{Noun}Query.cs — the IRequest<T> record + the
  IRequestHandler in the same file.
- {Request}Validator.cs in the same folder — AbstractValidator<T>, its own
  file (this project's convention — Validator is always separate,
  Command+Handler may share a file).
- Controller action on the matching {Feature}Controller (create one if it
  doesn't exist yet) — thin, only Mediator.Send + return. Route follows the
  existing [Route("[controller]")] + kebab-case convention automatically for
  the controller root; if you add a literal sub-path segment
  ([HttpGet("...")]), write it in kebab-case yourself — the slugify
  transformer doesn't reach hand-written literals.
- If this maps to/from a ViewModel, add explicit .ForMember(...) mappings
  for anything whose name doesn't match the entity exactly by convention
  (AutoMapper silently drops fields whose names don't match — check that
  every field actually maps).

Requirement: {fill in the specific feature}
```

---

## 3. Domain unit test

```
Write Domain.UnitTests coverage for {EntityName}.

Cover:
- The constructor with all required fields set correctly.
- Every guard-clause invariant (what happens when a required field is
  null/empty, when a numeric field is negative/zero if that's invalid, when
  a date ordering rule is violated) — one test case per invariant, not one
  giant test.

File: tests/Domain.UnitTests/Entities/{EntityName}Tests.cs, following the
existing test files in that folder for style (NUnit, FluentAssertions).
Run `dotnet test tests/Domain.UnitTests/` and confirm they pass before
reporting done.
```

---

## 4. Application functional test

```
Write an Application.FunctionalTests coverage for {UseCase} ({EntityName}).

This runs against the real local cpathai-test PostgreSQL database (not
Testcontainers — see .ai/coding-rules.md) via WebApplicationFactory. Cover:
- The successful path.
- Validation failure (missing required field, or the specific business rule
  this use case enforces).
- NotFound, if this is a query/command that looks up an existing entity by
  id and it doesn't exist.

File: tests/Application.FunctionalTests/Features/{Feature}/Commands/
or .../Queries/ (mirror the Application feature; the test class name keeps
the use case, for example CreateBankTests). Follow the existing pattern in
Testing.cs/BaseTestFixture.cs for sending commands/queries through the real
pipeline. Run `dotnet test tests/Application.FunctionalTests/` and confirm
they pass before reporting done — the cpathai-test database must exist
locally first (see .ai/dev-workflow.md if it doesn't).

Requirement: {fill in anything beyond the standard happy/validation/notfound
  cases — e.g. a specific concurrency scenario}
```

---

## 5. Frontend feature (Vue)

```
Build the frontend pages for {feature}: {create | edit | view | list |
  all four}.

Do:
- Dedicated routes under src/vuewebui/src/pages/{feature}/
  {create,edit,view,list}/ — no dialog-based CRUD, no dead stub routes that
  just redirect to list (this project's confirmed convention, see
  .ai/architecture.md "Frontend architecture").
- API calls in lower-kebab-case matching the real backend route (e.g.
  /lab-results, not /LabResults) — verify the real route and response
  shape (PaginatedList<T> → { items: [...] }, not a feature-specific field
  name) against the actual controller before wiring up the call; don't
  guess the endpoint shape.
- Reuse existing shared components (ConfirmProgressDialog, ProgressDialog,
  TablePagination, AppTextField/AppSelect/etc.) rather than inventing new
  ones.

Requirement: {fields, list columns, any lookups/dropdowns needed from other
  entities}

When done: run `pnpm run lint` and `pnpm run build` from src/vuewebui and
confirm both pass clean.
```

---

## 6. Playwright e2e spec

```
Write a Playwright e2e spec for {feature}: {flow to cover, e.g. "create →
  appears in list → edit → delete"}.

Do:
- src/vuewebui/e2e/pages/{feature}/{screen}.page.ts extending BasePage
  (e2e/pages/base.page.ts) — Page Object Model, one file per screen.
- src/vuewebui/e2e/specs/{feature}/{screen}.spec.ts, tagged @{feature} so it
  can be run in isolation (pnpm exec playwright test --grep "@{feature}").
- Add data-testid="{feature}-{element}" (lowercase, hyphen-separated) to any
  Vue component you need to target that doesn't have one yet.
- If the spec needs an authenticated session: check whether
  e2e/helpers/auth.ts#obtainE2eTestUser() is implemented yet. If not (it's a
  documented TODO as of this writing — this project's OIDC auth has no
  local login endpoint to script against), don't fake it — tell me what's
  blocking instead of working around it, or write the spec against a route
  that doesn't require sign-in.

See src/vuewebui/e2e/README.md for the full convention. Run
`pnpm test:e2e --grep "@{feature}"` and confirm it passes before reporting
done — flag clearly if it can't run yet because of the auth gap above.
```

---

## Keeping these templates current

These templates encode this project's conventions as they exist right now. When a convention changes (a new scaffolding tool lands in Phase 2, the frontend moves to the schema-driven scaffold in Phase 3, the OIDC test-auth gap gets resolved, etc.), **update the template in the same change** — don't leave it describing a workflow that no longer matches the code. This file drifting from reality is exactly the failure mode `.ai/known-issues.md` is meant to avoid elsewhere in this project.
