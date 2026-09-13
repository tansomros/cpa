# Code Structure Map

## Root Directory

```
/
├── .ai/                          → AI documentation (this folder)
├── .claude/                      → Claude Code configuration
├── src/                          → Source code (all projects)
├── tests/                        → Test projects
├── SUTH-Checkup.sln              → Visual Studio solution file
├── Directory.Build.props         → Shared MSBuild properties (target framework, nullable, implicit usings)
├── Directory.Packages.props      → Central Package Management (shared NuGet versions)
├── global.json                   → .NET SDK version pin (9.0)
├── .editorconfig                 → Code style rules (383 lines, comprehensive)
├── .gitignore                    → Git ignore rules
├── README.md                     → Project documentation & dev guidelines
├── Checkup.xlsx                  → Checkup reference spreadsheet
```

## Source Projects (`src/`)

### `src/Domain/` — Domain Layer (Pure Business Logic)
```
Domain/
├── Common/
│   ├── BaseEntity.cs             → Base class: Id, DeleteFlag, IsActive, CreatedOn, LastModified
│   ├── BaseEvent.cs              → MediatR INotification base
│   └── ValueObject.cs            → Value object equality base
├── Constants/
│   ├── HealthCheckupPolicies.cs  → Authorization policy names
│   └── HealthCheckupRoles.cs     → Role constants (Doctor, Nurse, Admin, Employee)
├── Entities/                     → 24 domain entity classes
│   ├── Patient.cs, Checkup.cs, Lab.cs, Xray.cs, Vision.cs
│   ├── Audiogram.cs, Hearing.cs, HearingHertz.cs
│   ├── Lung.cs, PhysicalExamination.cs, SpecialTest.cs
│   ├── Careprovider.cs, Company.cs, Report.cs
│   ├── Recommendation.cs, RecommendationTemplate.cs
│   ├── CheckupType.cs, CheckupClass.cs, CheckupGroup.cs, CheckupItem.cs
│   ├── ReferenceGroup.cs, ReferenceValue.cs
│   └── Province.cs, District.cs, SubDistrict.cs
├── Common/
│   ├── ...
│   └── SmartEnum.cs              → Generic base class for Smart Enums (with .resx localization)
├── Enums/
│   ├── PriorityLevel.cs          → Priority classification enum
│   ├── ExamResult.cs             → General exam results (Normal/Abnormal/NotExamined)
│   ├── LabResult.cs              → Lab results (Normal/Abnormal)
│   ├── XrayResult.cs             → X-ray results (Normal/Abnormal/WaitForSpecialist)
│   ├── BmdResult.cs              → Bone density results
│   ├── AbiResult.cs              → Ankle-brachial index results
│   ├── CheckupStatus.cs          → Checkup workflow status (Pending/InProgress/Reported)
│   ├── EyeResult.cs              → Eye exam results
│   └── HearingLossLevel.cs       → Hearing loss severity levels
├── Resources/                     → .resx localization for Smart Enums (Thai default + English)
│   ├── ExamResult.resx / ExamResult.en.resx
│   ├── LabResult.resx / LabResult.en.resx
│   └── ... (16 files total, 8 pairs)
├── Exceptions/
│   └── UnsupportedColourException.cs
└── ValueObjects/                 → 16 value object classes
    ├── Gender.cs, Colour.cs, StatusResult.cs, StatusFlag.cs
    ├── FinalReport.cs, FinalLab.cs, FinalVision.cs
    ├── LabReport.cs, LabStructure.cs, LungValue.cs
    ├── CompareRule.cs, Ear.cs, Eye.cs
    └── CareproviderType.cs
```

### `src/Application/` — Application Layer (Use Cases)
```
Application/
├── Common/
│   ├── Behaviours/               → MediatR pipeline behaviors
│   │   ├── AuthorizationBehaviour.cs   → Auth checks (currently DISABLED)
│   │   ├── ValidationBehaviour.cs      → FluentValidation integration
│   │   ├── LoggingBehaviour.cs         → Request logging
│   │   ├── PerformanceBehaviour.cs     → Slow request detection
│   │   └── UnhandledExceptionBehaviour.cs → Exception logging
│   ├── Exceptions/
│   │   ├── ValidationException.cs
│   │   └── ForbiddenAccessException.cs
│   ├── Extensions/
│   │   └── QueryableExtensions.cs      → EF Core query helpers
│   ├── Interfaces/               → Service contracts
│   │   ├── ICheckupDatabaseContext.cs  → Main DbContext interface
│   │   ├── IHosxpDatabaseContext.cs    → HOSxP DbContext interface
│   │   ├── ICurrentUserService.cs
│   │   ├── IDateTime.cs
│   │   ├── IIdentityService.cs
│   │   ├── INotificationService.cs
│   │   ├── IPublicCheckupService.cs
│   │   └── ISuthAppService.cs
│   ├── Mappings/
│   │   ├── IMapFrom.cs                → AutoMapper mapping interface
│   │   └── MappingProfile.cs          → Profile registration
│   ├── Models/
│   │   ├── Result.cs                   → Operation result pattern
│   │   ├── PaginatedList.cs            → Pagination wrapper
│   │   └── LookupDto.cs               → Key-value lookup
│   └── Security/
│       └── AuthorizeAttribute.cs       → Custom [Authorize] attribute
├── Features/                     → 25 feature modules (CQRS)
│   ├── Checkups/
│   │   ├── Commands/
│   │   │   ├── Create/          → CreateCheckupCommand + Handler + Validator
│   │   │   ├── Update/          → UpdateCheckupCommand + Handler
│   │   │   ├── Delete/          → DeleteCheckupCommand + Handler
│   │   │   ├── SaveFinalReport/ → SaveCheckupFinalReportCommand + Handler
│   │   │   └── UpdateFromWorker/→ UpdateCheckupFromWorkerCommand + Handler
│   │   ├── Queries/
│   │   │   ├── Get/             → GetCheckupQuery + Handler
│   │   │   ├── GetByVisitNumber/
│   │   │   ├── GetList/
│   │   │   ├── GetPaginatedList/
│   │   │   └── Search/
│   │   └── Queries/CheckupViewModel.cs
│   ├── Patients/                 → Similar CQRS structure
│   ├── Labs/, Xrays/, Visions/, Audiograms/
│   ├── Lungs/, PhysicalExaminations/, SpecialTests/
│   ├── Careproviders/, Companies/, Addresss/
│   ├── CheckupTypes/, CheckupClasses/, CheckupGroups/, CheckupItems/
│   ├── Recommendations/, RecommendationTemplates/
│   ├── Reports/, ReferenceGroups/, ReferenceValues/
│   ├── Hearings/, HearingHertzs/
│   ├── Lookups/
│   │   ├── LookupOptionDto.cs         → DTO for smart enum lookup options
│   │   ├── LookupRegistry.cs          → Registry mapping category names to Smart Enums
│   │   ├── GetLookupOptionsQuery.cs   → Query: get options by category (with lang)
│   │   └── GetLookupCategoriesQuery.cs→ Query: list available categories
│   └── Systems/                  → Seed data initializer commands
│       └── Commands/
│           ├── ThaiProvinceDataInitializerCommand.cs
│           ├── CheckupClassDataInitializerCommand.cs
│           └── ... (9 more initializers)
└── DependencyInjection.cs        → Application DI registration
```

### `src/Infrastructure/` — Infrastructure Layer (Data Access)
```
Infrastructure/
├── Identity/
│   ├── ApplicationUser.cs        → Identity user model
│   └── IdentityResultExtensions.cs
├── Persistence/
│   ├── CheckupDatabaseContext.cs         → Main EF Core DbContext
│   ├── CheckupDatabaseContextInitializer.cs → Migration + seed runner
│   ├── Configurations/                   → EF Fluent API entity configs
│   │   ├── AudiogramConfiguration.cs
│   │   ├── CareproviderConfiguration.cs
│   │   ├── CheckupConfiguration.cs
│   │   ├── CheckupItemConfiguration.cs
│   │   ├── DistrictConfiguration.cs
│   │   ├── PatientConfiguration.cs
│   │   ├── ProvinceConfiguration.cs
│   │   ├── RecommendationTemplateConfiguration.cs
│   │   ├── ReportConfiguration.cs
│   │   └── SubDistrictConfiguration.cs
│   ├── HosxpDatabase/
│   │   └── HosxpDatabaseContext.cs       → External HOSxP database
│   ├── Interceptors/
│   │   └── AuditableEntitySaveChangesInterceptors.cs → Auto-set audit fields
│   └── Migrations/
│       └── CheckupDatabaseContextModelSnapshot.cs
├── Services/
│   ├── DateTimeService.cs
│   └── SuthAppService.cs
└── DependencyInjection.cs        → Infrastructure DI registration
```

### `src/API/` — REST API (Presentation Layer)
```
API/
├── Program.cs                    → App configuration (DI, Auth, Swagger, CORS)
├── Controllers/                  → 15+ REST controllers
│   ├── CheckupsController.cs
│   ├── PatientsController.cs
│   ├── CareprovidersController.cs
│   ├── LabsController.cs
│   ├── XraysController.cs
│   ├── VisionController.cs
│   ├── AudiogramsController.cs
│   ├── LungsController.cs
│   ├── PhysicalExaminationsController.cs
│   ├── RecommendationsController.cs
│   ├── ReportsController.cs
│   ├── SpecialTestsController.cs
│   ├── CompaniesController.cs
│   ├── CheckupTypesController.cs
│   ├── CheckupClassesController.cs
│   ├── CheckupItemController.cs
│   ├── HearingController.cs
│   └── AddresssController.cs
├── Endpoints/
│   └── BaseController.cs         → ControllerBase + MediatR mediator
├── Converters/
│   ├── JsonDateOnlyConverter.cs  → DateOnly JSON serialization
│   └── JsonTimeOnlyConverter.cs  → TimeOnly JSON serialization
├── Middlewares/
│   └── ExceptionMiddleware.cs    → Global exception handler
├── Routing/
│   └── RouteTokenTransformer.cs  → URL slug transformer
├── Services/
│   └── CurrentUserService.cs     → ICurrentUserService implementation
├── appsettings.json
└── appsettings.Development.json
```

### `src/HosxpWorkerService/` — Background Sync Service
```
HosxpWorkerService/
├── Program.cs                    → Worker host setup with Serilog
├── Functions/                    → Utility functions
├── HttpClients/                  → HTTP client services
│   └── IdentityTokenHandler.cs   → OAuth2 token management
├── OpenAPIs/                     → NSwag-generated API clients
│   ├── CheckupApiClient.cs       → Checkup API client (~16K LOC)
│   ├── HosxpApiClient.cs         → HOSxP API client (~15K LOC)
│   ├── swagger.json              → HOSxP API spec
│   └── swagger1.json             → Checkup API spec
├── Services/
│   ├── WorkerCoordinator.cs      → Orchestrates all sync services
│   ├── CheckupPatientSyncService.cs → Patient + checkup sync
│   ├── LabSyncService.cs         → Lab results sync
│   ├── XraySyncService.cs        → X-ray results sync
│   ├── DoctorProviderSyncService.cs → Doctor/provider sync
│   ├── CheckupPatientSyncStateService.cs → State tracking
│   ├── LabSyncStateService.cs
│   ├── XraySyncStateService.cs
│   └── DoctorProviderSyncStateService.cs
├── State/                        → JSON state persistence files
│   ├── checkup_patient_change_state.json
│   ├── lab_change_state.json
│   ├── xray_change_state.json
│   └── doctor_provider_change_state.json
├── appsettings.json
└── appsettings.Development.json
```

### `src/WinFormsUI/` — Desktop Application
```
WinFormsUI/
├── Program.cs                    → DI setup, OIDC client, culture config
├── Forms/                        → WinForms forms
│   ├── LoginForm.cs              → OAuth2 login via WebView2
│   ├── MainForm.cs               → Main application window
│   ├── CheckUpListForm.cs        → Checkup list view
│   ├── CheckupForm.cs            → Detailed checkup view (~6.6K LOC designer)
│   └── CheckupPersonal.cs        → Personal info tab
├── Controllers/                  → Business logic (legacy, large files)
│   ├── BaseGlobalClass.cs        → Global static utilities (~1.9K LOC)
│   ├── BaseClass.cs              → Base controller (~1.8K LOC)
│   ├── PatientController.cs
│   ├── AppointmentController.cs
│   ├── AssessmentController.cs
│   └── LocationController.cs
├── Helpers/
│   └── SmartEnumBindingHelper.cs → Converts Smart Enums to DataSource for DevExpress LookUpEdit
├── Constants/                    → Enums and constants (ReferenceGroup.cs marked [Obsolete])
├── Models/                       → Data models and DTOs
├── Reports/                      → DevExpress XtraReports
├── Services/                     → Auth, config services
├── Factories/                    → Form factory (DI)
├── Interfaces/                   → Service interfaces
├── Functions/                    → Utility functions (~2.4K LOC)
├── DLL/                          → Third-party DLLs
├── OpenAPIs/                     → NSwag-generated API clients
├── appsettings.Development.json
└── appsettings.Production.json
```

### `src/vuewebui/` — Vue.js Web Frontend
```
vuewebui/
├── src/
│   ├── @core/                    → Core framework utilities
│   │   ├── components/           → Reusable UI components (forms, cards)
│   │   ├── composable/           → Vue composables
│   │   ├── libs/                 → Library integrations
│   │   ├── scss/                 → Core SCSS styles
│   │   ├── stores/               → Config store
│   │   └── utils/                → Formatters, validators, helpers
│   ├── @layouts/                 → Layout system
│   │   ├── components/           → Nav, footer, layout components
│   │   └── stores/               → Layout config store
│   ├── assets/                   → Fonts, images, styles
│   ├── components/               → App-specific components, dialogs
│   ├── composables/              → App composables
│   ├── layouts/                  → Page layouts (default.vue, blank.vue)
│   ├── navigation/               → Menu definitions (vertical/horizontal)
│   ├── pages/                    → Route pages (auto-generated routes)
│   │   ├── dashboards/           → Dashboard views
│   │   ├── signin-oidc.vue       → OIDC callback
│   │   ├── signout.vue           → Logout page
│   │   └── silent-renew.vue      → Token refresh
│   ├── plugins/                  → Vue plugins (numbered for load order)
│   │   ├── 1.router/             → Vue Router + guards
│   │   ├── 2.pinia.js            → Pinia store
│   │   ├── auth/                 → OIDC authentication
│   │   ├── casl/                 → Authorization (CASL)
│   │   ├── i18n/                 → Internationalization (EN/TH)
│   │   ├── iconify/              → Icon system
│   │   └── vuetify/              → Vuetify theme + defaults
│   ├── stores/
│   │   └── useLookupStore.js     → Pinia store: caches API-fetched lookup options with lang support
│   ├── utils/                    → API client, constants, pagination
│   │   ├── lookups.js            → Object.freeze constants mirroring backend Smart Enum values
│   ├── App.vue                   → Root component
│   └── main.js                   → Entry point
├── themeConfig.js                → Theme configuration
├── vite.config.js                → Vite build configuration
├── package.json                  → Dependencies & scripts
└── jsconfig.json                 → Path aliases
```

## Test Projects (`tests/`)

```
tests/
├── Domain.UnitTests/             → Entity & value object tests (33 test files)
├── Application.UnitTests/        → Command/query handler tests (limited)
├── Application.FunctionalTests/  → API integration tests (PostgreSQL + Respawn)
└── Infrastructure.IntegrationTests/ → Empty (no tests implemented)
```
