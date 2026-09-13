# Feature Map

Maps system features to their code locations across all layers.

---

## Patient Management

| Layer | Location |
|-------|----------|
| Entity | `src/Domain/Entities/Patient.cs` |
| Commands | `src/Application/Features/Patients/Commands/Create/CreatePatientCommand.cs` |
| | `src/Application/Features/Patients/Commands/CreateFromWorker/CreatePatientFromWorkerCommand.cs` |
| | `src/Application/Features/Patients/Commands/Update/UpdatePatientCommand.cs` |
| | `src/Application/Features/Patients/Commands/UpsertFromWorker/UpsertPatientFromWorkerCommand.cs` |
| Queries | `src/Application/Features/Patients/Queries/Get/GetPatientQuery.cs` |
| | `src/Application/Features/Patients/Queries/GetByHospitalNumber/GetPatientByHospitalNumberQuery.cs` |
| | `src/Application/Features/Patients/Queries/GetList/GetPatientListQuery.cs` |
| | `src/Application/Features/Patients/Queries/Search/SearchPatientListQuery.cs` |
| ViewModel | `src/Application/Features/Patients/Queries/PatientViewModel.cs` |
| Controller | `src/API/Controllers/PatientsController.cs` |
| DB Config | `src/Infrastructure/Persistence/Configurations/PatientConfiguration.cs` |
| Sync | `src/HosxpWorkerService/Services/CheckupPatientSyncService.cs` |
| Tests | `tests/Domain.UnitTests/Entities/PatientTests.cs` |

---

## Checkup Visit Management

| Layer | Location |
|-------|----------|
| Entity | `src/Domain/Entities/Checkup.cs` |
| Commands | `src/Application/Features/Checkups/Commands/Create/CreateCheckupCommand.cs` |
| | `src/Application/Features/Checkups/Commands/Update/UpdateCheckupCommand.cs` |
| | `src/Application/Features/Checkups/Commands/Delete/DeleteCheckupCommand.cs` |
| | `src/Application/Features/Checkups/Commands/SaveFinalReport/SaveCheckupFinalReportCommand.cs` |
| | `src/Application/Features/Checkups/Commands/UpdateFromWorker/UpdateCheckupFromWorkerCommand.cs` |
| Queries | `src/Application/Features/Checkups/Queries/Get/GetCheckupQuery.cs` |
| | `src/Application/Features/Checkups/Queries/GetByVisitNumber/GetCheckupByVisitNumberQuery.cs` |
| | `src/Application/Features/Checkups/Queries/GetList/GetCheckupListQuery.cs` |
| | `src/Application/Features/Checkups/Queries/GetPaginatedList/GetCheckupPaginatedListQuery.cs` |
| | `src/Application/Features/Checkups/Queries/Search/SearchCheckupListQuery.cs` |
| Controller | `src/API/Controllers/CheckupsController.cs` |
| DB Config | `src/Infrastructure/Persistence/Configurations/CheckupConfiguration.cs` |
| Sync | `src/HosxpWorkerService/Services/CheckupPatientSyncService.cs` |
| Desktop UI | `src/WinFormsUI/Forms/CheckupForm.cs` |
| | `src/WinFormsUI/Forms/CheckUpListForm.cs` |
| Tests | `tests/Domain.UnitTests/Entities/CheckupTests.cs` |

---

## Laboratory Results

| Layer | Location |
|-------|----------|
| Entity | `src/Domain/Entities/Lab.cs` |
| Value Objects | `src/Domain/ValueObjects/LabReport.cs`, `LabStructure.cs` |
| Commands | `src/Application/Features/Labs/Commands/` |
| Queries | `src/Application/Features/Labs/Queries/` |
| Controller | `src/API/Controllers/LabsController.cs` |
| Sync | `src/HosxpWorkerService/Services/LabSyncService.cs` |
| Tests | `tests/Domain.UnitTests/Entities/LabTests.cs` |

---

## X-ray Results

| Layer | Location |
|-------|----------|
| Entity | `src/Domain/Entities/Xray.cs` |
| Commands | `src/Application/Features/Xrays/Commands/` |
| Queries | `src/Application/Features/Xrays/Queries/` |
| Controller | `src/API/Controllers/XraysController.cs` |
| Sync | `src/HosxpWorkerService/Services/XraySyncService.cs` |
| Tests | `tests/Domain.UnitTests/Entities/XrayTests.cs` |

---

## Vision/Eye Examination

| Layer | Location |
|-------|----------|
| Entity | `src/Domain/Entities/Vision.cs` |
| Value Objects | `src/Domain/ValueObjects/Eye.cs`, `FinalVision.cs` |
| Commands | `src/Application/Features/Visions/Commands/` |
| Queries | `src/Application/Features/Visions/Queries/` |
| Controller | `src/API/Controllers/VisionController.cs` |
| Tests | `tests/Domain.UnitTests/Entities/VisionTests.cs` |

---

## Audiogram / Hearing Tests

| Layer | Location |
|-------|----------|
| Entities | `src/Domain/Entities/Audiogram.cs` |
| | `src/Domain/Entities/Hearing.cs` |
| | `src/Domain/Entities/HearingHertz.cs` |
| Value Objects | `src/Domain/ValueObjects/Ear.cs` |
| Commands/Queries | `src/Application/Features/Audiograms/` |
| | `src/Application/Features/Hearings/` |
| | `src/Application/Features/HearingHertzs/` |
| Controller | `src/API/Controllers/AudiogramsController.cs` |
| | `src/API/Controllers/HearingController.cs` |
| DB Config | `src/Infrastructure/Persistence/Configurations/AudiogramConfiguration.cs` |
| Tests | `tests/Domain.UnitTests/Entities/AudiogramTests.cs` |

---

## Lung Function / Spirometry

| Layer | Location |
|-------|----------|
| Entity | `src/Domain/Entities/Lung.cs` |
| Value Objects | `src/Domain/ValueObjects/LungValue.cs` |
| Commands/Queries | `src/Application/Features/Lungs/` |
| Controller | `src/API/Controllers/LungsController.cs` |
| Tests | `tests/Domain.UnitTests/Entities/LungTests.cs` |

---

## Physical Examination

| Layer | Location |
|-------|----------|
| Entity | `src/Domain/Entities/PhysicalExamination.cs` |
| Commands/Queries | `src/Application/Features/PhysicalExaminations/` |
| Controller | `src/API/Controllers/PhysicalExaminationsController.cs` |
| Tests | `tests/Domain.UnitTests/Entities/PhysicalExaminationTests.cs` |

---

## Recommendations

| Layer | Location |
|-------|----------|
| Entity | `src/Domain/Entities/Recommendation.cs` |
| Template | `src/Domain/Entities/RecommendationTemplate.cs` |
| Commands/Queries | `src/Application/Features/Recommendations/` |
| | `src/Application/Features/RecommendationTemplates/` |
| Controller | `src/API/Controllers/RecommendationsController.cs` |

---

## Reports

| Layer | Location |
|-------|----------|
| Entity | `src/Domain/Entities/Report.cs` |
| Value Objects | `src/Domain/ValueObjects/FinalReport.cs`, `FinalLab.cs`, `FinalVision.cs` |
| Commands/Queries | `src/Application/Features/Reports/` |
| Controller | `src/API/Controllers/ReportsController.cs` |
| DB Config | `src/Infrastructure/Persistence/Configurations/ReportConfiguration.cs` |
| Desktop Reports | `src/WinFormsUI/Reports/` (DevExpress XtraReports) |

---

## Careprovider Management

| Layer | Location |
|-------|----------|
| Entity | `src/Domain/Entities/Careprovider.cs` |
| Value Objects | `src/Domain/ValueObjects/CareproviderType.cs` |
| Commands/Queries | `src/Application/Features/Careproviders/` |
| Controller | `src/API/Controllers/CareprovidersController.cs` |
| DB Config | `src/Infrastructure/Persistence/Configurations/CareproviderConfiguration.cs` |
| Sync | `src/HosxpWorkerService/Services/DoctorProviderSyncService.cs` |

---

## Smart Enum Lookups

| Layer | Location |
|-------|----------|
| Base Class | `src/Domain/Common/SmartEnum.cs` |
| Enums | `src/Domain/Enums/ExamResult.cs`, `LabResult.cs`, `XrayResult.cs`, `BmdResult.cs`, `AbiResult.cs`, `CheckupStatus.cs`, `EyeResult.cs`, `HearingLossLevel.cs` |
| Localization (.resx) | `src/Domain/Resources/*.resx` (16 files: Thai default + English) |
| Lookup Registry | `src/Application/Features/Lookups/LookupRegistry.cs` |
| Queries | `src/Application/Features/Lookups/GetLookupOptionsQuery.cs` |
| | `src/Application/Features/Lookups/GetLookupCategoriesQuery.cs` |
| DTO | `src/Application/Features/Lookups/LookupOptionDto.cs` |
| Controller | `src/API/Controllers/OptionsController.cs` |
| WinForms Helper | `src/WinFormsUI/Helpers/SmartEnumBindingHelper.cs` |
| Vue Constants | `src/vuewebui/src/utils/lookups.js` |
| Vue Store | `src/vuewebui/src/stores/useLookupStore.js` |
| Vue i18n | `src/vuewebui/src/plugins/i18n/locales/en.json` → `lookups.*` |
| | `src/vuewebui/src/plugins/i18n/locales/th.json` → `lookups.*` |
| Tests | `tests/Application.FunctionalTests/Features/Lookups/GetLookupOptionsTests.cs` |

---

## Reference Data (Master Data)

| Feature | Location |
|---------|----------|
| Checkup Types | `src/Application/Features/CheckupTypes/` |
| Checkup Classes | `src/Application/Features/CheckupClasses/` |
| Checkup Groups | `src/Application/Features/CheckupGroups/` |
| Checkup Items | `src/Application/Features/CheckupItems/` |
| Reference Groups | `src/Application/Features/ReferenceGroups/` |
| Reference Values | `src/Application/Features/ReferenceValues/` |
| Companies | `src/Application/Features/Companies/` |
| Addresses | `src/Application/Features/Addresss/` |
| Seed Initializers | `src/Application/Features/Systems/Commands/` |

---

## HOSxP Integration (Background Sync)

| Component | Location |
|-----------|----------|
| Coordinator | `src/HosxpWorkerService/Services/WorkerCoordinator.cs` |
| Patient/Checkup Sync | `src/HosxpWorkerService/Services/CheckupPatientSyncService.cs` |
| Lab Sync | `src/HosxpWorkerService/Services/LabSyncService.cs` |
| X-ray Sync | `src/HosxpWorkerService/Services/XraySyncService.cs` |
| Doctor Sync | `src/HosxpWorkerService/Services/DoctorProviderSyncService.cs` |
| State Files | `src/HosxpWorkerService/State/*.json` |
| API Clients | `src/HosxpWorkerService/OpenAPIs/` (NSwag-generated) |
| Auth Handler | `src/HosxpWorkerService/HttpClients/IdentityTokenHandler.cs` |

---

## Authentication & Authorization

| Component | Location |
|-----------|----------|
| JWT Setup | `src/API/Program.cs` (lines ~60-100) |
| Current User | `src/API/Services/CurrentUserService.cs` |
| Policies | `src/Domain/Constants/HealthCheckupPolicies.cs` |
| Roles | `src/Domain/Constants/HealthCheckupRoles.cs` |
| Auth Behavior | `src/Application/Common/Behaviours/AuthorizationBehaviour.cs` |
| OIDC (Vue) | `src/vuewebui/src/plugins/auth/` |
| OIDC (WinForms) | `src/WinFormsUI/Services/AuthenticationService.cs` |
| CASL (Vue) | `src/vuewebui/src/plugins/casl/` |

---

## Web Frontend (Vue.js)

| Feature | Location |
|---------|----------|
| Dashboard | `src/vuewebui/src/pages/dashboards/` |
| Auth Pages | `src/vuewebui/src/pages/signin-oidc.vue`, `signout.vue` |
| Router | `src/vuewebui/src/plugins/1.router/` |
| API Client | `src/vuewebui/src/utils/api.js` |
| i18n | `src/vuewebui/src/plugins/i18n/` |
| Theming | `src/vuewebui/src/plugins/vuetify/` |
| Navigation | `src/vuewebui/src/navigation/` |
| Layouts | `src/vuewebui/src/layouts/` |
