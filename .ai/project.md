# Project Overview

## Project Name
**SUTH Checkup** — Health Checkup Management System

## Purpose
A comprehensive health checkup management system for **Suranaree University of Technology Hospital (SUTH)**. It manages patient health checkup records, laboratory results, imaging results, physical examinations, and health reports across the full checkup lifecycle.

## Target Users
- **Doctors** — Review checkup results, write recommendations, approve final reports
- **Nurses** — Record vital signs, manage checkup workflows, enter physical exam findings
- **Admin Staff** — Manage master data, checkup types, companies, reference values
- **Employees/Patients** — View their health checkup results (via web UI)

## Business Problem Solved
Centralizes the health checkup process for a hospital. Replaces fragmented workflows by:
- Integrating with the existing **HOSxP** hospital information system for patient/lab/xray data
- Providing a unified platform for recording and reviewing checkup results
- Generating comprehensive health reports with cumulative comparisons
- Supporting both web and desktop interfaces for different user roles

## Core Features
1. **Patient Management** — Register and manage patient demographics synced from HOSxP
2. **Checkup Visit Tracking** — Create, update, and manage checkup visits with vital signs
3. **Lab Results** — Record and track laboratory test results with reference ranges
4. **X-ray Results** — Manage imaging results with abnormality flags
5. **Vision Testing** — Eye examination including acuity, color blindness, pressure
6. **Audiogram/Hearing** — Hearing tests with frequency-level data
7. **Lung Function** — Spirometry (FVC, FEV1) with severity classification
8. **Physical Examination** — Systematic body exam findings (HEENT, chest, abdomen, etc.)
9. **Recommendations** — Doctor recommendations with reusable templates
10. **Report Generation** — Final health checkup reports with cumulative comparison
11. **HOSxP Integration** — Automated background sync of patients, labs, x-rays, and doctors
12. **Multi-client Access** — Web UI (Vue.js), Desktop (WinForms), REST API

## Technology Stack

| Layer | Technology |
|-------|-----------|
| **Backend API** | ASP.NET Core 9.0, C# 12 |
| **Architecture** | Clean Architecture, CQRS with MediatR |
| **Database** | PostgreSQL with EF Core 9.0 (Npgsql) |
| **Authentication** | OAuth2/JWT via Identity Server (Duende) |
| **Web Frontend** | Vue 3, Vite, Vuetify 3, Pinia, Vue Router |
| **Desktop Client** | WinForms (.NET 9), DevExpress, WebView2 |
| **Background Service** | .NET Worker Service (HosxpWorkerService) |
| **Logging** | Serilog (Worker), Microsoft.Extensions.Logging (API) |
| **Validation** | FluentValidation |
| **Mapping** | AutoMapper |
| **API Docs** | Swagger/OpenAPI (Swashbuckle + NSwag) |
| **SDK** | .NET 9.0 |
