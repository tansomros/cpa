# AI Agent Guide

Read this before making any change. It tells you where things go and what not to do. For the "why", see [architecture.md](architecture.md) and [coding-rules.md](coding-rules.md). For current known problems, check [known-issues.md](known-issues.md) first — don't rediscover (or re-break) something already tracked there. Starting a specific task (new entity, new feature, tests)? [prompt-templates.md](prompt-templates.md) has a ready-to-paste starting prompt per stage.

## Where to add things

| I need to... | Goes in |
|---|---|
| Add a new domain entity | `src/Domain/Entities/{Entity}.cs` |
| Add a shared domain base type | `src/Domain/Common/` |
| Add a new command (create/update/delete) | `src/Application/Features/{Feature}/Commands/Create`, `Commands/Update`, or `Commands/Delete` — `{Verb}{Noun}Command.cs` (Command + Handler together) and `{Verb}{Noun}CommandValidator.cs` (Validator, its own file). The folder name is only `Create`, `Update`, or `Delete` |
| Add a new query | `src/Application/Features/{Feature}/Queries/Get/` — both the single query and the list query. `{Get}{Noun}Query.cs` and `{Get}{Noun}ListQuery.cs` (Query + Handler), plus a validator file when the query is validated |
| Add EF mapping for an entity | `src/Infrastructure/Persistence/Configurations/{Entity}Configuration.cs`, register the `DbSet<T>` in `CpaDatabaseContext` and `ICpaDatabaseContext` |
| Add a migration | Teerapol creates and removes migrations himself (from `src/Infrastructure`: `dotnet ef migrations add {Name} --startup-project ../API`). AI agents must not create, delete, or commit migration files, or run migrations against `cpathai`, without approval — see [dev-workflow.md](dev-workflow.md) ส่วนที่ 6 |
| Add an API endpoint | Add an action to the matching `src/API/Controllers/{Feature}Controller.cs` (or create one) — the action should only call `Mediator.Send(...)` |
| Add a frontend page for a feature | `src/vuewebui/src/pages/{feature}/{create,edit,view,list}/` — dedicated routes, not a dialog; use the schema-driven scaffold once it exists (roadmap Phase 3) |
| Add a shared frontend composable | `src/vuewebui/src/composables/` |
| Add cross-feature reference data used by many forms | A Pinia store (roadmap Phase 3), not a per-page fetch |

## MUST

- MUST put a new command in `Features/{Feature}/Commands/Create`, `Update`, or `Delete`, and a new query in `Features/{Feature}/Queries/Get`. The namespace must match that folder (`BigLion.CPA.Application.Features.{Feature}.Commands.Create` or `...Queries.Get`). Do not add a `{Context}` segment or an entity suffix to those folders.
- MUST give every new entity a `Guid ExternalId` and use it (never the internal `Id`) in any API route, request/response DTO, or frontend URL.
- MUST use `Ardalis.GuardClauses` (or an equivalent explicit check) to enforce entity invariants in the constructor/behavior methods — don't rely on FluentValidation alone.
- MUST model any running money balance (an amount of money remaining or outstanding) as derived from an append-only ledger table — never a single field that gets directly incremented/decremented (don't overwrite a money balance in place). This rule is about money only, and it is not optional for this project; see [coding-rules.md](coding-rules.md).
- MUST add a concurrency token to any new mutable entity.
- MUST wrap a multi-aggregate write (touches more than one entity type in one logical operation) in a single database transaction.
- MUST write a handler-level unit test and a functional test for any change touching money, quantity, or approval/workflow state before considering the work done.
- MUST keep controllers thin — no business logic, no direct DbContext access in `src/API`.
- MUST update [known-issues.md](known-issues.md) after completing a unit of work, so it reflects reality, not just intent.

## MUST NOT

- MUST NOT copy an existing feature's files and hand-edit field names as the way to start a new feature — copy-and-rename is how partial renames creep in (a namespace that no longer matches its folder, a typo'd route). Use the scaffolding template instead (below).
- MUST NOT give a domain entity property a bare public setter if an invalid value would break a business rule — gate the mutation through a validated method.
- MUST NOT put business logic in a controller, in a Vue page's inline script beyond simple UI state, or directly in a MediatR pipeline behaviour — it belongs in the domain entity or the handler.
- MUST NOT add a new `Roles`/`Policies` constant — use the `module.action` permission convention.
- MUST NOT introduce a second HTTP client pattern on the frontend, or a second exception-handling middleware/filter on the backend — there should be exactly one of each. (There were two of each before this restructuring; don't reintroduce the pattern.)
- MUST NOT leave dead/unregistered scaffolding in the codebase (an interceptor that's never registered, a middleware that's never wired up, an entity with no `DbSet`) — either finish wiring it up or delete it in the same change.
- MUST NOT expose the internal integer `Id` in any new API surface — use `ExternalId`.

## Scaffolding a new use case

Project-specific templates exist — install once per machine:
```
dotnet new install ./templates/biglion-templates
```

They only generate the CQRS slice (Command/Query + Validator + Handler) — not the EF configuration or the controller action, since those vary too much per feature to templatize safely without producing a wrong-looking scaffold that gets silently accepted. Write those two by hand or via [prompt-templates.md](prompt-templates.md).

Both templates place their output relative to the current directory using the proven `sourceName` rename mechanism (a symbol-driven `rename` modifier was tried and does **not** work reliably in this SDK version — don't reintroduce it). That means you `cd` into the exact target folder first:

```
cd src/Application/Features/{FeatureName}/Commands
dotnet new biglion-command -n CreateBank --featureName Banks --boundedContext Banks --returnType int

cd ../Queries
dotnet new biglion-query -n GetBank --featureName Banks --boundedContext Banks --returnType "BankViewModel"
```

The template still emits a `BoundedContext` namespace segment and a folder named after `-n` (`CreateBank`, `GetBank`). After generating, move the files into `Commands/Create`, `Commands/Update`, `Commands/Delete`, or `Queries/Get`, and set the namespace to `BigLion.CPA.Application.Features.{Feature}.Commands.Create` (or `.Update`, `.Delete`, `.Queries.Get`). Put both the single-item query and the list query in `Queries/Get`.

`--returnType` has no safe default for `biglion-query` (it's required) — a default embedding the literal word "Examples" would collide with the `--featureName` substitution and silently produce a wrong class name. `biglion-command` defaults `--returnType` to `int` since that default contains no substitutable tokens.

The primary templates are `biglion-templates` above. The generic upstream `ca-usecase` template is only an optional fallback for anything outside this project's own conventions:
```
dotnet new ca-usecase --name CreatePatient --feature-name Patients --usecase-type command --return-type int
```
(If not installed: `dotnet new install Clean.Architecture.Solution.Template::9.0.10`.)

## Before you start any change

1. Read [known-issues.md](known-issues.md) — is what you're about to touch already a tracked, open issue? Don't fix it twice or in a conflicting way.
2. Check [domain.md](domain.md) if the change touches pharmacy/patient/MTM vocabulary — use the established terms, don't invent new ones.
