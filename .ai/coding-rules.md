# Coding Rules

Rules and conventions detected in this repository. Future AI agents must follow these rules when modifying code.

---

## Architecture Rules

### Clean Architecture (Mandatory)
1. **Domain layer** has ZERO external framework dependencies (only MediatR for events)
2. **Application layer** depends only on Domain
3. **Infrastructure layer** implements Application interfaces
4. **Presentation layers** (API, WinForms, Vue) depend on Application, never directly on Infrastructure
5. Dependencies always point inward: Presentation → Application → Domain ← Infrastructure

### CQRS Pattern (Mandatory)
1. Every state-changing operation is a **Command** (`IRequest<T>`)
2. Every read operation is a **Query** (`IRequest<T>`)
3. Commands and Queries have dedicated **Handlers** (`IRequestHandler<TRequest, TResponse>`)
4. Dispatch through **MediatR** (never call handlers directly)
5. Each Command/Query lives in its own folder under `Features/{FeatureName}/Commands/{OperationName}/` or `Queries/{OperationName}/`

### Feature Organization
```
Features/{FeatureName}/
├── Commands/
│   ├── Create/
│   │   ├── Create{Entity}Command.cs
│   │   ├── Create{Entity}CommandHandler.cs
│   │   └── Create{Entity}CommandValidator.cs
│   ├── Update/
│   └── Delete/
├── Queries/
│   ├── Get/
│   ├── GetList/
│   ├── Search/
│   └── {FeatureName}ViewModel.cs
└── (ViewModels at feature root or in Queries folder)
```

---

## Naming Conventions

### C# (.NET)
| Element | Convention | Example |
|---------|-----------|---------|
| Classes | PascalCase | `CheckupService` |
| Interfaces | `I` + PascalCase | `ICheckupDatabaseContext` |
| Methods | PascalCase | `GetCheckupByIdAsync` |
| Properties | PascalCase | `HospitalNumber` |
| Constants | PascalCase | `RequireDoctor` |
| Private fields | `_camelCase` | `_context` |
| Private static fields | `s_camelCase` | `s_instance` |
| Parameters | camelCase | `cancellationToken` |
| Local variables | camelCase | `checkupList` |
| Type parameters | `T` + PascalCase | `TResponse` |
| Enums | PascalCase | `PriorityLevel` |
| Namespaces | PascalCase | `Application.Features.Checkups` |

### File Naming
| Type | Pattern | Example |
|------|---------|---------|
| Entity | `{EntityName}.cs` | `Checkup.cs` |
| Command | `{Action}{Entity}Command.cs` | `CreateCheckupCommand.cs` |
| Handler | `{Action}{Entity}CommandHandler.cs` | `CreateCheckupCommandHandler.cs` |
| Validator | `{Action}{Entity}CommandValidator.cs` | `CreateCheckupCommandValidator.cs` |
| Query | `{Action}{Entity}Query.cs` | `GetCheckupQuery.cs` |
| ViewModel | `{Entity}ViewModel.cs` | `CheckupViewModel.cs` |
| Controller | `{Entity}sController.cs` | `CheckupsController.cs` |
| Configuration | `{Entity}Configuration.cs` | `CheckupConfiguration.cs` |
| Test | `{Entity}Tests.cs` | `CheckupTests.cs` |

### Vue.js / JavaScript
| Element | Convention | Example |
|---------|-----------|---------|
| Components | PascalCase `.vue` | `NavSearchBar.vue` |
| Pages | kebab-case `.vue` | `signin-oidc.vue` |
| Composables | `use` + PascalCase | `useApi.js` |
| Stores | camelCase | `config.js` |
| Utils | camelCase | `formatters.js` |
| Constants | UPPER_SNAKE_CASE | `VITE_API_BASE_URL` |

---

## Code Style Rules (from .editorconfig)

### C#
- **Indentation:** 4 spaces
- **Namespaces:** File-scoped (`namespace X;`)
- **var usage:** Explicit types for built-in types, `var` for apparent types
- **Expression bodies:** Use for accessors and lambdas
- **Pattern matching:** Preferred over type checks
- **Null operators:** Use `?.`, `??`, `??=`
- **Primary constructors:** Preferred (C# 12)
- **Readonly fields:** Mark fields readonly when possible (`:warning`)
- **Static local functions:** Preferred when possible (`:warning`)

### XML/JSON
- **Indentation:** 2 spaces

### General
- **Line endings:** LF
- **Final newline:** Required
- **Trailing whitespace:** Trim

---

## Controller Rules

1. All controllers inherit from `BaseController` (which provides `Mediator` property)
2. Route: `[Route("api/[controller]")]`
3. Controllers only dispatch MediatR requests — no business logic
4. Use `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]` attributes
5. Return appropriate status codes: `Ok()`, `Created()`, `NoContent()`
6. Controller methods are `async Task<ActionResult<T>>`

---

## Entity Rules

1. All entities inherit from `BaseEntity`
2. `BaseEntity` provides: `Id`, `DeleteFlag`, `IsActive`, `CreatedOn`, `LastModified`
3. Use soft delete (`DeleteFlag = true`) — never hard delete from code
4. Audit fields are auto-populated by `AuditableEntitySaveChangesInterceptors`
5. Complex nested data uses value objects stored as JSON columns
6. Collections use `List<T>` properties (e.g., `List<Lab> Labs`)

---

## Validation Rules

1. Use **FluentValidation** for all Commands
2. Validator class name: `{CommandName}Validator`
3. Validators are auto-discovered and run via `ValidationBehaviour` in the MediatR pipeline
4. Throw `ValidationException` for business rule violations
5. Throw `ForbiddenAccessException` for authorization failures

---

## Mapping Rules

1. Use **AutoMapper** for entity-to-ViewModel mapping
2. ViewModels implement `IMapFrom<TEntity>` interface
3. Custom mappings override `Mapping(Profile profile)` method
4. Mapping profiles are auto-registered from the Application assembly

---

## Database Rules

1. Use **EF Core** for data access — no raw SQL unless necessary
2. Access database through `ICheckupDatabaseContext` interface (never concrete context)
3. Entity configurations go in `Infrastructure/Persistence/Configurations/`
4. Use Fluent API for configurations (not data annotations on entities)
5. JSON columns for complex value objects (via Npgsql JSON support)
6. Migrations are code-first: define entity → add migration → apply

---

## Testing Rules

1. **Domain tests:** Cover entity constructors and behavior
2. **Application tests:** Cover command/query handlers — test success, validation failure, not found
3. **Functional tests:** Use `CustomWebApplicationFactory` with PostgreSQL
4. **Framework:** NUnit 3 + FluentAssertions + Moq
5. Test class naming: `{Entity}Tests`
6. Test method naming: descriptive (e.g., `Should_Create_Checkup_With_Valid_Data`)

---

## Vue.js Rules

1. **Composition API** only (no Options API)
2. **Pinia** for state management
3. **ofetch** for HTTP calls (not axios)
4. **Auto-imports:** Composables and utilities are auto-imported via unplugin
5. **File-based routing:** Pages in `pages/` directory auto-generate routes
6. **Plugin numbering:** Plugins prefixed with numbers for load order (e.g., `1.router`, `2.pinia`)
7. **Cookie-based persistence:** Use `cookieRef()` for persistent state
8. **i18n:** All user-facing text should use `$t()` translation function
