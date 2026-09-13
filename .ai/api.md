# API Overview

## Base Configuration
- **Framework:** ASP.NET Core 9.0
- **Base Route:** `api/[controller]`
- **Authentication:** JWT Bearer (Identity Server)
- **Documentation:** Swagger/OpenAPI at `/swagger`
- **CORS:** AllowAnyOrigin, AllowAnyMethod, AllowAnyHeader
- **Content Type:** `application/json`

## Authorization Policies
| Policy | Description |
|--------|-------------|
| RequireAuthenticatedUser | Any authenticated user |
| RequireDoctor | Doctor role required |
| RequireNurse | Nurse role required |
| RequireAdmin | Admin role required |
| RequireEmployee | Employee role required |

> **Note:** Authorization enforcement is currently disabled in the MediatR pipeline.

---

## Endpoints

### Checkups (`/api/checkups`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/checkups` | Yes | Create a new checkup visit |
| PUT | `/api/checkups` | Yes | Update an existing checkup |
| PUT | `/api/checkups/final` | Yes | Save final checkup report |
| PUT | `/api/checkups/worker` | Yes | Update checkup from worker service |
| DELETE | `/api/checkups/{id}` | Yes | Delete a checkup |
| GET | `/api/checkups/{id}` | Yes | Get checkup by ID |
| GET | `/api/checkups/visit/{visitNumber}` | Yes | Get checkup by visit number |
| GET | `/api/checkups` | Yes | Get all checkups |
| GET | `/api/checkups/paginated` | Yes | Get paginated checkup list |
| GET | `/api/checkups/search` | Yes | Search checkups |

### Patients (`/api/patients`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/patients` | Yes | Create a new patient |
| POST | `/api/patients/worker` | Yes | Create patient from worker |
| PUT | `/api/patients` | Yes | Update patient |
| PUT | `/api/patients/worker` | Yes | Upsert patient from worker |
| GET | `/api/patients/{id}` | Yes | Get patient by ID |
| GET | `/api/patients/hn/{hospitalNumber}` | Yes | Get patient by hospital number |
| GET | `/api/patients` | Yes | Get all patients |
| GET | `/api/patients/search` | Yes | Search patients |

### Labs (`/api/labs`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/labs` | Yes | Create lab result |
| PUT | `/api/labs` | Yes | Update lab result |
| DELETE | `/api/labs/{id}` | Yes | Delete lab result |
| GET | `/api/labs/{id}` | Yes | Get lab by ID |
| GET | `/api/labs` | Yes | Get all labs |
| GET | `/api/labs/search` | Yes | Search labs |
| GET | `/api/labs/date` | Yes | Get labs by test date |

### X-rays (`/api/xrays`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/xrays` | Yes | Create x-ray result |
| PUT | `/api/xrays` | Yes | Update x-ray result |
| DELETE | `/api/xrays/{id}` | Yes | Delete x-ray |
| GET | `/api/xrays/{id}` | Yes | Get x-ray by ID |
| GET | `/api/xrays` | Yes | Get all x-rays |
| GET | `/api/xrays/search` | Yes | Search x-rays |

### Vision (`/api/vision`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/vision` | Yes | Create vision exam result |
| PUT | `/api/vision` | Yes | Update vision result |
| DELETE | `/api/vision/{id}` | Yes | Delete vision result |
| GET | `/api/vision/{id}` | Yes | Get vision by ID |
| GET | `/api/vision` | Yes | Get all vision results |
| GET | `/api/vision/search` | Yes | Search vision results |

### Audiograms (`/api/audiograms`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/audiograms` | Yes | Create audiogram |
| PUT | `/api/audiograms` | Yes | Update audiogram |
| DELETE | `/api/audiograms/{id}` | Yes | Delete audiogram |
| GET | `/api/audiograms/{id}` | Yes | Get audiogram by ID |
| GET | `/api/audiograms` | Yes | Get all audiograms |
| GET | `/api/audiograms/search` | Yes | Search audiograms |

### Lungs (`/api/lungs`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/lungs` | Yes | Create lung function result |
| PUT | `/api/lungs` | Yes | Update lung result |
| DELETE | `/api/lungs/{id}` | Yes | Delete lung result |
| GET | `/api/lungs/{id}` | Yes | Get lung by ID |
| GET | `/api/lungs` | Yes | Get all lung results |

### Physical Examinations (`/api/physicalexaminations`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/physicalexaminations` | Yes | Create physical exam |
| PUT | `/api/physicalexaminations` | Yes | Update physical exam |
| DELETE | `/api/physicalexaminations/{id}` | Yes | Delete physical exam |
| GET | `/api/physicalexaminations/{id}` | Yes | Get physical exam by ID |
| GET | `/api/physicalexaminations` | Yes | Get all physical exams |

### Special Tests (`/api/specialtests`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/specialtests` | Yes | Create special test |
| PUT | `/api/specialtests` | Yes | Update special test |
| DELETE | `/api/specialtests/{id}` | Yes | Delete special test |
| GET | `/api/specialtests/{id}` | Yes | Get special test by ID |
| GET | `/api/specialtests` | Yes | Get all special tests |

### Careproviders (`/api/careproviders`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/careproviders` | Yes | Create careprovider |
| PUT | `/api/careproviders` | Yes | Update careprovider |
| DELETE | `/api/careproviders/{id}` | Yes | Delete careprovider |
| GET | `/api/careproviders/{id}` | Yes | Get careprovider by ID |
| GET | `/api/careproviders` | Yes | Get all careproviders |
| GET | `/api/careproviders/type/{type}` | Yes | Get careproviders by type |

### Recommendations (`/api/recommendations`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/recommendations` | Yes | Create recommendation |
| PUT | `/api/recommendations` | Yes | Update recommendation |
| DELETE | `/api/recommendations/{id}` | Yes | Delete recommendation |
| GET | `/api/recommendations/{id}` | Yes | Get recommendation by ID |
| GET | `/api/recommendations` | Yes | Get all recommendations |

### Reports (`/api/reports`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| POST | `/api/reports` | Yes | Create report |
| PUT | `/api/reports` | Yes | Update report |
| DELETE | `/api/reports/{id}` | Yes | Delete report |
| GET | `/api/reports/{id}` | Yes | Get report by ID |
| GET | `/api/reports` | Yes | Get all reports |
| GET | `/api/reports/search` | Yes | Search reports |

### Reference Data

| Controller | Base Path | Purpose |
|-----------|-----------|---------|
| CheckupTypes | `/api/checkuptypes` | CRUD for checkup types |
| CheckupClasses | `/api/checkupclasses` | CRUD for checkup classes |
| CheckupGroups | `/api/checkupgroups` | CRUD for checkup groups |
| CheckupItems | `/api/checkupitems` | CRUD for checkup items |
| Companies | `/api/companies` | CRUD for companies |
| Hearing | `/api/hearing` | CRUD for hearing records |
| ReferenceGroups | `/api/referencegroups` | CRUD for reference groups |
| ReferenceValues | `/api/referencevalues` | CRUD for reference values |

### Lookup Options (`/api/options`) — Smart Enum Lookups

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| GET | `/api/options` | Yes | List all available lookup categories |
| GET | `/api/options/{category}?lang=` | Yes | Get lookup options for a category (optional language) |

**Categories:** `exam-results`, `lab-results`, `xray-results`, `bmd-results`, `abi-results`, `checkup-statuses`, `eye-results`, `hearing-loss-levels`

**Response format:**
```json
[
  { "value": "Normal", "displayName": "ปกติ" },
  { "value": "Abnormal", "displayName": "ผิดปกติ" }
]
```

With `?lang=en`:
```json
[
  { "value": "Normal", "displayName": "Normal" },
  { "value": "Abnormal", "displayName": "Abnormal" }
]
```

### Addresses (`/api/addresss`)

| Method | Path | Auth | Purpose |
|--------|------|------|---------|
| GET | `/api/addresss/provinces` | Yes | Get all Thai provinces |
| GET | `/api/addresss/districts/{provinceId}` | Yes | Get districts by province |
| GET | `/api/addresss/subdistricts/{districtId}` | Yes | Get sub-districts by district |

---

## Response Patterns

### Success Responses
- `200 OK` — Successful read or update
- `201 Created` — Successful creation (returns new entity ID)
- `204 No Content` — Successful deletion

### Error Responses
- `400 Bad Request` — Validation failure (returns `ProblemDetails`)
- `404 Not Found` — Entity not found
- `401 Unauthorized` — Missing/invalid JWT token
- `403 Forbidden` — Insufficient permissions
- `500 Internal Server Error` — Unhandled exception (returns `ProblemDetails`)

### Pagination Response Format
```json
{
  "items": [...],
  "pageNumber": 1,
  "totalPages": 5,
  "totalCount": 50,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```
