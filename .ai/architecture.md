# System Architecture

## Overview
SUTH Checkup follows **Clean Architecture** with **CQRS** (Command Query Responsibility Segregation) pattern. The system comprises multiple deployment units communicating through REST APIs and shared database access.

## Architecture Diagram

```mermaid
graph TB
    subgraph Clients
        VUE[Vue.js Web UI<br/>Vuetify + Pinia]
        WIN[WinForms Desktop<br/>DevExpress + WebView2]
    end

    subgraph Backend
        API[ASP.NET Core API<br/>REST Controllers]
        WORKER[HosxpWorkerService<br/>Background Sync]
    end

    subgraph External
        IDS[Identity Server<br/>OAuth2/JWT]
        HOSXP[HOSxP API<br/>Hospital System]
    end

    subgraph Data
        PG[(PostgreSQL<br/>Checkup Database)]
    end

    VUE -->|REST + JWT| API
    WIN -->|REST + JWT| API
    VUE -->|OIDC| IDS
    WIN -->|OIDC| IDS
    API -->|Validate JWT| IDS
    API -->|EF Core| PG
    WORKER -->|REST| API
    WORKER -->|REST| HOSXP
    WORKER -->|OAuth2 Client Credentials| IDS
```

## Clean Architecture Layers

```mermaid
graph LR
    subgraph Presentation
        A[API Controllers]
        W[WinForms UI]
        V[Vue.js Web UI]
    end

    subgraph Application
        B[Commands & Queries<br/>Handlers, Validators<br/>Interfaces, DTOs]
    end

    subgraph Domain
        C[Entities<br/>Value Objects<br/>Smart Enums, Constants]
    end

    subgraph Infrastructure
        D[EF Core DbContext<br/>Configurations<br/>External Services]
    end

    A --> B
    W --> B
    W -.->|Smart Enums| C
    B --> C
    D --> B
    D --> C
```

**Dependency Rule:** Dependencies point inward. Domain has zero external dependencies. Application depends only on Domain. Infrastructure implements Application interfaces.

## Major Modules

### 1. Domain (`src/Domain/`)
Pure business logic layer. Contains entities, value objects, enums, and domain events. No framework dependencies except MediatR for domain events.

### 2. Application (`src/Application/`)
Use cases organized as **Features**. Each feature contains Commands (writes), Queries (reads), ViewModels (DTOs), and Validators. Uses MediatR pipeline with behaviors for cross-cutting concerns (validation, logging, performance, authorization).

### 3. Infrastructure (`src/Infrastructure/`)
Data access via EF Core with PostgreSQL. Contains DbContext, entity configurations, migrations, interceptors (audit trail), and external service implementations.

### 4. API (`src/API/`)
ASP.NET Core REST API. Controllers dispatch requests via MediatR. Includes JWT authentication, Swagger documentation, exception middleware, and CORS configuration.

### 5. HosxpWorkerService (`src/HosxpWorkerService/`)
Background service that polls HOSxP API for changes and syncs data into the Checkup database via the Checkup API. Handles: patients, checkup visits, lab results, x-ray results, and doctor/careprovider records.

### 6. WinFormsUI (`src/WinFormsUI/`)
Desktop client for power users (doctors/nurses). Provides detailed checkup forms, cumulative comparison views, and DevExpress-based report printing. Directly references the Domain project for compile-time access to Smart Enums and localized display names.

### 7. Vue Web UI (`src/vuewebui/`)
Modern SPA built with Vue 3 + Vuetify 3. Provides dashboard views with OIDC authentication, i18n (EN/TH), and dark/light theming.

## Data Flow

### Checkup Creation Flow
```mermaid
sequenceDiagram
    participant C as Client (Vue/WinForms)
    participant API as Checkup API
    participant MED as MediatR Pipeline
    participant DB as PostgreSQL

    C->>API: POST /api/checkups
    API->>MED: Send CreateCheckupCommand
    MED->>MED: ValidationBehaviour
    MED->>MED: LoggingBehaviour
    MED->>DB: Save Checkup Entity
    DB-->>MED: Entity Created
    MED-->>API: Return ID
    API-->>C: 201 Created
```

### HOSxP Sync Flow
```mermaid
sequenceDiagram
    participant WC as WorkerCoordinator
    participant SYNC as SyncService
    participant HOSXP as HOSxP API
    participant API as Checkup API
    participant STATE as State File (JSON)

    WC->>SYNC: Trigger sync cycle
    SYNC->>STATE: Read last sync timestamp
    SYNC->>HOSXP: GET changes since timestamp
    HOSXP-->>SYNC: Changed records
    loop Each changed record
        SYNC->>API: GET existing record
        alt New Record
            SYNC->>API: POST create
        else Updated Record
            SYNC->>API: PUT update
        end
    end
    SYNC->>STATE: Update timestamp
```

## Authentication & Authorization

- **Protocol:** OAuth2 / OpenID Connect
- **Provider:** Identity Server (Duende)
- **API Auth:** JWT Bearer tokens validated against Identity Server authority
- **Client Auth:** OIDC redirect flow (web), OIDC with WebView2 (desktop)
- **Worker Auth:** OAuth2 Client Credentials flow
- **Roles:** Doctor, Nurse, Admin, Employee
- **Policies:** RequireAuthenticatedUser, RequireDoctor, RequireNurse, RequireAdmin, RequireEmployee

> **Note:** Authorization enforcement in the MediatR pipeline (`AuthorizationBehaviour`) is currently disabled (commented out). See [known-issues.md](known-issues.md).

## External Services

| Service | Purpose | Integration |
|---------|---------|-------------|
| **Identity Server** | Authentication & authorization | JWT validation, OIDC flows |
| **HOSxP API** | Hospital Information System | REST API polling via Worker Service |
| **PostgreSQL** | Primary database | EF Core with Npgsql |
