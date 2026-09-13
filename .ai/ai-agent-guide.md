# AI Agent Guide

Guide for AI agents working with the SUTH Checkup repository. Follow this to safely and correctly modify the system.

---

## Quick Reference

| Task | Where to Work |
|------|--------------|
| Add new API endpoint | `src/Application/Features/`, `src/API/Controllers/` |
| Add business logic | `src/Application/Features/{Feature}/Commands/` or `Queries/` |
| Add domain entity | `src/Domain/Entities/`, then `Application`, then `Infrastructure` |
| Add database model | `src/Domain/Entities/`, `src/Infrastructure/Persistence/` |
| Add background job | `src/HosxpWorkerService/Services/` |
| Add tests | `tests/Domain.UnitTests/`, `tests/Application.UnitTests/` |
| Add Vue page | `src/vuewebui/src/pages/` |
| Add Vue component | `src/vuewebui/src/components/` |

---

## Adding a New API Endpoint

### Step 1: Create Domain Entity (if new)
Location: `src/Domain/Entities/{EntityName}.cs`
```csharp
public class NewEntity : BaseEntity
{
    public string Name { get; set; }
    // ... properties
}
```

### Step 2: Add DbSet to Context Interface
Location: `src/Application/Common/Interfaces/ICheckupDatabaseContext.cs`
```csharp
DbSet<NewEntity> NewEntities { get; }
```

### Step 3: Add DbSet to Context Implementation
Location: `src/Infrastructure/Persistence/CheckupDatabaseContext.cs`
```csharp
public DbSet<NewEntity> NewEntities => Set<NewEntity>();
```

### Step 4: Create Feature (Command/Query)
Location: `src/Application/Features/NewEntities/`

Create the CQRS structure:
```
Features/NewEntities/
├── Commands/
│   └── Create/
│       ├── CreateNewEntityCommand.cs      (IRequest<int>)
│       ├── CreateNewEntityCommandHandler.cs
│       └── CreateNewEntityCommandValidator.cs
├── Queries/
│   ├── Get/
│   │   ├── GetNewEntityQuery.cs           (IRequest<NewEntityViewModel>)
│   │   └── GetNewEntityQueryHandler.cs
│   └── NewEntityViewModel.cs             (implements IMapFrom<NewEntity>)
```

### Step 5: Create Controller
Location: `src/API/Controllers/NewEntitiesController.cs`
```csharp
[Route("api/[controller]")]
public class NewEntitiesController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateNewEntityCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NewEntityViewModel>> Get(int id)
    {
        return await Mediator.Send(new GetNewEntityQuery { Id = id });
    }
}
```

### Step 6: Add EF Configuration (if needed)
Location: `src/Infrastructure/Persistence/Configurations/NewEntityConfiguration.cs`

### Step 7: Create Migration
```bash
dotnet ef migrations add AddNewEntity --project src/Infrastructure --startup-project src/API
```

### Step 8: Add Tests
- `tests/Domain.UnitTests/Entities/NewEntityTests.cs`
- `tests/Application.UnitTests/Features/NewEntities/CreateNewEntityCommandTests.cs`

---

## Adding Business Logic

All business logic goes in **Command/Query Handlers** in the Application layer.

**DO:**
- Put logic in handlers (`IRequestHandler<TRequest, TResponse>`)
- Use `ICheckupDatabaseContext` for database access
- Use FluentValidation for input validation
- Throw `ValidationException` for business rule violations
- Use AutoMapper for entity-to-ViewModel mapping

**DO NOT:**
- Put business logic in controllers (they only dispatch)
- Put business logic in entities (keep entities as data containers)
- Access DbContext directly from controllers
- Use raw SQL unless absolutely necessary

---

## Adding a Database Model

1. Create entity in `src/Domain/Entities/`
2. Inherit from `BaseEntity`
3. Add `DbSet` to `ICheckupDatabaseContext` and `CheckupDatabaseContext`
4. Create Fluent API configuration in `Infrastructure/Persistence/Configurations/` if needed
5. Add migration: `dotnet ef migrations add <Name> --project src/Infrastructure --startup-project src/API`

For JSON columns (complex nested data):
```csharp
// In entity
public MyValueObject MyData { get; set; }

// In configuration
builder.OwnsOne(e => e.MyData, b => b.ToJson());
```

---

## Adding a Background Job

Location: `src/HosxpWorkerService/Services/`

1. Create sync service implementing the sync pattern:
```csharp
public class NewSyncService
{
    public async Task DetectAndProcessChanges() { ... }
}
```

2. Create state service for tracking last sync position:
```csharp
public class NewSyncStateService { ... }
```

3. Add state JSON file in `src/HosxpWorkerService/State/`

4. Register in `Program.cs`:
   - Add as singleton service
   - Add to `WorkerCoordinator`

5. Follow the existing pattern: poll external API → compare with local → create/update via Checkup API

---

## Adding Tests

### Domain Unit Tests
Location: `tests/Domain.UnitTests/Entities/`
```csharp
[TestFixture]
public class NewEntityTests
{
    [Test]
    public void Constructor_Should_Set_Properties()
    {
        var entity = new NewEntity { Name = "Test" };
        entity.Name.Should().Be("Test");
    }
}
```

### Application Unit Tests
Location: `tests/Application.UnitTests/`
- Test handlers with mocked `ICheckupDatabaseContext`
- Verify validation rules with validator tests

### Functional Tests
Location: `tests/Application.FunctionalTests/`
- Uses `CustomWebApplicationFactory` for full API tests
- PostgreSQL
- Database reset via Respawn between tests

---

## Adding Vue.js Pages

### New Page
Create file in `src/vuewebui/src/pages/{page-name}.vue` — route is auto-generated.

### New API Call
Use `$api` from `src/vuewebui/src/utils/api.js`:
```javascript
import { $api } from '@/utils/api'
const data = await $api('/api/newentities')
```

### Add Navigation Menu Item
Edit `src/vuewebui/src/navigation/vertical/index.js`:
```javascript
{ title: 'New Feature', icon: { icon: 'tabler-icon' }, to: 'page-name' }
```

---

## Architectural Constraints

1. **Never bypass MediatR** — All operations go through the mediator pipeline
2. **Never reference Infrastructure from Domain or Application** — Use interfaces
3. **Never put UI logic in Application layer** — Keep it presentation-agnostic
4. **Never hard-delete records** — Use soft delete (`DeleteFlag = true`, `IsActive = false`)
5. **Never store secrets in code** — Use `appsettings.json` or environment variables
6. **Never modify auto-generated files** — `OpenAPIs/*.cs` files are regenerated
7. **Always add validators** — Every Command should have a FluentValidation validator
8. **Always add tests** — Domain entity tests are mandatory, handler tests recommended
9. **Always use async/await** — All database and HTTP operations must be async
10. **Always use cancellation tokens** — Pass `CancellationToken` through the call chain

---

## Common Pitfalls

- **Missing DbSet registration:** Adding an entity but forgetting to add `DbSet<T>` to both interface and implementation
- **Missing AutoMapper mapping:** Creating a ViewModel without implementing `IMapFrom<T>`
- **Missing DI registration:** Creating a service without registering it in `DependencyInjection.cs`
- **Circular dependencies:** Infrastructure referencing Application features directly instead of through interfaces
- **JSON column changes:** Modifying value objects used as JSON columns may require migration
- **Worker state files:** State JSON files track sync position — corrupting them causes re-sync of all data
