# Known Issues & Technical Debt

---

## Resolved Issues (This Session)
- **#1 Authorization disabled** — [Fixed: Corrected boolean logic and early returns]
- **#old-1 Performance logging disabled** — [Fixed: Enabled and mapped to current user]
- **#old-6 Commented-out dead code** — [Fixed: Cleaned 4 files]
- **#old-todo VB.NET Conversion Debt** — [Fixed: Verified C# `break` was correct, removed TODO]
- **#1 Deprecated SqlClient Usage** — [Fixed by User]
- **#2 Auto-Generated API Clients** — [Fixed: Moved to NSwag build-time generation to reduce repository bloat]
- **#3 HOSxP Sync State Fragility** — [Fixed: Added 24-hour max lookback safety mechanism instead of resetting to beginning of time]
- **#4 Empty Integration Test Project** — [Fixed: Bootstrapped PostgreSQL fixture and tested DbContext Interceptors]
- **#5 Limited Application Test Coverage** — [Fixed: Wrote comprehensive unit testing for Authorization, Validation, UnhandledExceptions, Paginations, and Models]
- **#7 Broad Exception Catching** — [Fixed: Refactored `WinFormsUI/FormFactory.cs` to leverage `ExceptionDispatchInfo` preventing `Activator.CreateInstance` from wiping nested exception stack traces within WinForms components]

---
*All known backlog technical debt items have been thoroughly addressed and cleared from the agenda!*
