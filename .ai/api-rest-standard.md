# SUTH Health Checkup API REST Standard

When developing or modifying API Controllers in the SUTH Health Checkup microservices, all endpoints **must strictly** conform to modern RESTful architecture. This ensures seamless NSwag auto-generation and consistent front-end integrations.

## 1. Route Paths & Naming
Never embed verbs into route URLs. 
- ❌ **BAD**: `[HttpPost("CreateLab")]`, `[HttpGet("[action]")]`
- ✅ **GOOD**: `POST /api/Labs`, `GET /api/Labs/visits/1234`

### CRUD Standard Matrix
| Operation         | HTTP Method | Route Syntax (Attribute) |
|-------------------|-------------|--------------------------|
| **Create**        | `POST`      | `[HttpPost]`             |
| **Read (All)**    | `GET`       | `[HttpGet]`              |
| **Read (Single)** | `GET`       | `[HttpGet("{id}")]`      |
| **Update**        | `PUT`       | `[HttpPut("{id}")]`      |
| **Delete**        | `DELETE`    | `[HttpDelete("{id}")]`   |

### Sub-Resources & Batches
If an operation requires special routing distinct from generic CRUD:
- **Search Operations**: `[HttpGet("search")]` (Not `SearchLabList`)
- **Batch Updates**: `[HttpPut("batch")]` (Not `UpdateCreateLabList`)
- **Cross-Service Syncs**: `[HttpPut("sync/worker")]` (Not `UpsertLatestLabForWorker`)
- **Foreign Key Retrieval**: `[HttpGet("visits/{visitNumber}")]`

## 2. Parameter Definitions
1. **Identifier Bindings**: Never bind single identifiers via query strings or bodies if they identify a specific resource. Use `{id}` or `{visitNumber}` in the URL.
2. **Search Criteria**: Massive querying criteria (`StartDate`, `SearchTerm`, `Page`, `Length`) **must** be extracted into a POCO and bound using `[FromQuery]`.
3. **Data Mutations**: `CreateCommand` and `UpdateCommand` payloads **must** be bound using `[FromBody]`.

## 3. Swagger Responses & Return Types
NSwag automatically parses return types to generate TypeScript/C# client schemas. Use `ActionResult<T>` to implicitly declare the HTTP 200 OK schema:

- ❌ **BAD**: `public async Task<IActionResult> GetCheckup(int id)`
- ✅ **GOOD**: `public async Task<ActionResult<CheckupViewModel>> GetCheckup(int id)`

### Error Documentation
You **must** continue explicitly declaring failure paths for Swagger to generate proper `ApiException` classes using `ProducesResponseType`:
```csharp
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
```
