# คู่มือการพัฒนา (Development Workflow)

คู่มือฉบับนี้เขียนขึ้นสำหรับ**คนในทีม** โดยเฉพาะคนที่เพิ่งเข้าร่วมพัฒนาโปรเจกต์นี้ — สถาปัตยกรรมที่ใช้คืออะไรและทำไมถึงใช้, ติดตั้งเครื่องมืออะไรบ้าง, รันโปรเจกต์ยังไง, เทสยังไง, build ยังไง, migrate database ยังไง, และสร้างโค้ดใหม่ (scaffold) ยังไง เรียงเป็นขั้นตอนตั้งแต่เครื่องเปล่าจนรันโปรเจกต์ได้

> เอกสารสำหรับ AI agent (เช่น Claude Code) ดูที่ [prompt-templates.md](prompt-templates.md) แทน
> ส่วนเรื่อง **deploy ขึ้น server จริง** ยังไม่มีเอกสาร (ยังไม่ได้เขียน) — เอกสารฉบับนี้พูดถึงแค่การพัฒนาในเครื่องตัวเอง (local development)

---

## ส่วนที่ 0 — สถาปัตยกรรมซอฟต์แวร์ที่ใช้ในโปรเจกต์

หัวข้อนี้อธิบาย**แนวคิด/pattern** ที่โปรเจกต์นี้ใช้จริง — คืออะไร ทำงานอย่างไร ทำไมต้องใช้ และได้ประโยชน์อะไร โดยอ้างอิงจากโค้ดจริงในโปรเจกต์ ไม่ใช่ทฤษฎีลอย ๆ (รายละเอียดเชิงเทคนิคแบบเต็มอยู่ที่ [architecture.md](architecture.md) — หัวข้อนี้เป็นฉบับอธิบายสำหรับคนที่ยังไม่เคยเจอ pattern พวกนี้มาก่อน)

### 0.1 Clean Architecture

**คืออะไร:** วิธีแบ่งโค้ดออกเป็น "ชั้น" (layer) ตามความรับผิดชอบ แล้วกำหนดกฎว่าชั้นไหนอ้างอิง (dependency) ชั้นไหนได้บ้าง — หัวใจสำคัญคือ **ชั้นในสุดต้องไม่รู้จักชั้นนอกเลย**

**ทำงานอย่างไรในโปรเจกต์นี้:** แบ่งเป็น 4 โปรเจกต์ (`.csproj`) โดยทิศทางการอ้างอิงเป็นแบบทางเดียว (บังคับจริงตอน compile ไม่ใช่แค่กติกาปากเปล่า):

```text
Domain          → ไม่ reference โปรเจกต์ไหนเลย
Application     → reference Domain
Infrastructure  → reference Application, Domain
API             → reference Application, Infrastructure
```

- **Domain** (`src/Domain`) — Entity, enum, business rule ล้วน ๆ ไม่รู้จัก EF Core ไม่รู้จัก MediatR ไม่รู้จักแม้แต่ว่าเก็บข้อมูลลง PostgreSQL
- **Application** (`src/Application`) — use case ทั้งหมด (CQRS — ดูข้อ 0.2) คุยกับฐานข้อมูลผ่าน **interface** เท่านั้น (`ICpaDatabaseContext`) ไม่รู้จัก EF Core ตัวจริง
- **Infrastructure** (`src/Infrastructure`) — เป็นคน implement `ICpaDatabaseContext` จริง ด้วย EF Core + PostgreSQL (`CpaDatabaseContext`)
- **API** (`src/API`) — controller บาง ๆ รับ request แล้วส่งต่อให้ Application เท่านั้น ไม่มี business logic

**ทำไมต้องใช้ / ประโยชน์:**

- อยากเปลี่ยนฐานข้อมูลจาก PostgreSQL เป็นอย่างอื่น หรือเปลี่ยนวิธี expose API จาก REST เป็นอย่างอื่น ทำได้โดยแทบไม่กระทบ business logic เลย เพราะ Domain/Application ไม่เคยรู้จักสิ่งเหล่านี้ตั้งแต่แรก
- เทส business logic (Domain, Application) ได้เร็วและง่าย โดยไม่ต้องพึ่งฐานข้อมูลจริงหรือรัน web server เลย
- โค้ดใหม่ทุกไฟล์รู้ทันทีว่า "ควรอยู่ชั้นไหน" ลดการถกเถียงเรื่องโครงสร้าง

### 0.2 CQRS (Command Query Responsibility Segregation)

**คืออะไร:** แยกโค้ดที่ "เขียน/เปลี่ยนข้อมูล" (**Command** เช่น สร้าง/แก้ไข/ลบ) ออกจากโค้ดที่ "อ่านข้อมูลอย่างเดียว" (**Query**) ให้เป็นคนละคลาสกันชัดเจน แทนที่จะรวมทุกอย่างไว้ใน Service class ตัวเดียวที่ใหญ่ขึ้นเรื่อย ๆ

**ทำงานอย่างไรในโปรเจกต์นี้:** แต่ละ feature อยู่ที่ `src/Application/Features/{Feature}/` โฟลเดอร์คำสั่งมีแค่ `Commands/Create`, `Commands/Update`, `Commands/Delete` และคำสั่งอ่านอยู่รวมกันที่ `Queries/Get` ไม่ต่อชื่อ entity ท้ายโฟลเดอร์ ชื่อคลาสยังมีชื่อ entity อยู่ เช่น สร้างธนาคารอยู่ที่ `Features/Banks/Commands/Create/` และ namespace คือ `BigLion.CPA.Application.Features.Banks.Commands.Create` ส่วนดึงรายการเดียวกับดึงรายการอยู่ด้วยกันที่ `Features/Banks/Queries/Get/` (`GetBankQuery`, `GetBankListQuery`) ในโฟลเดอร์นั้นมีไฟล์เหล่านี้:

1. `CreateBankCommand.cs` — record ที่เก็บข้อมูล input และ Handler ที่ทำงานจริง
2. `CreateBankCommandValidator.cs` — กติกาตรวจสอบข้อมูล (FluentValidation) แยกไฟล์ของตัวเอง

**ทำไมต้องใช้ / ประโยชน์:**

- เปิดโฟลเดอร์เดียวเห็นครบทุกอย่างของ use case นั้น ไม่ต้องไล่หาใน Service class มหึมาที่มีเป็นร้อยเมธอด
- แก้ไข/เพิ่ม use case หนึ่ง แทบไม่กระทบไฟล์ของ use case อื่นเลย — คนในทีมหลายคนแก้คนละ feature พร้อมกันได้โดย merge conflict น้อยลงมาก
- Query (อ่านอย่างเดียว) ไม่ต้อง track การเปลี่ยนแปลงข้อมูลเหมือน Command ทำให้ปรับแต่งให้อ่านเร็วได้อิสระ ไม่ต้องกังวลจะกระทบฝั่งเขียน

### 0.3 Mediator Pattern (ผ่านไลบรารี MediatR)

**คืออะไร:** ตัวกลางที่รับ "คำสั่ง" (Command/Query) จากผู้เรียก แล้วหา Handler ที่ตรงกันมาทำงานให้เอง — ผู้เรียกไม่จำเป็นต้องรู้จักหรืออ้างอิง Handler นั้นโดยตรงเลย

**ทำงานอย่างไรในโปรเจกต์นี้:** ใน Controller (`src/API/Controllers/`) จะเห็นแค่โค้ดประมาณนี้เสมอ:

```csharp
var result = await Mediator.Send(new CreateBankCommand { ... });
return Ok(result);
```

Controller ไม่ได้ inject `CreateBankCommandHandler` เข้ามาตรง ๆ — MediatR เป็นคนหา Handler ที่รับ `CreateBankCommand` ได้ (ผ่านการสแกนตอน startup) แล้วเรียกให้อัตโนมัติ

**ทำไมต้องใช้ / ประโยชน์:**

- Controller (ชั้น API) ไม่ต้องรู้จัก business logic แม้แต่น้อย — หน้าที่มีแค่ "แปลง HTTP request เป็น Command/Query แล้วส่งเข้า Mediator" (เรียกว่า **thin controller**)
- เพิ่มพฤติกรรมที่ต้องทำ**ทุก** request แบบรวมศูนย์ได้ (ดูข้อ 0.4 — Pipeline Behaviour) โดยไม่ต้องเขียนซ้ำในทุก Handler

### 0.4 Pipeline Behaviours — จุดที่ทำงานร่วมทุก Command/Query

**คืออะไร:** MediatR เปิดให้ "ดัก" ทุกคำสั่งที่ส่งเข้ามา ก่อนจะถึง Handler จริง เพื่อทำงานที่ต้องทำซ้ำ ๆ เหมือนกันทุก use case โดยไม่ต้องเขียนโค้ดซ้ำในทุก Handler

**ทำงานอย่างไรในโปรเจกต์นี้:** อยู่ที่ `src/Application/Common/Behaviours/` เรียงเป็นชั้นตามลำดับนี้ทุกครั้ง:

```text
UnhandledExceptionBehaviour → AuthorizationBehaviour → ValidationBehaviour
  → PerformanceBehaviour → LoggingBehaviour → (Handler จริง)
```

เช่น `ValidationBehaviour` จะรัน FluentValidation validator ของ Command นั้นให้อัตโนมัติ **ก่อน**ถึง Handler เสมอ — ถ้าข้อมูลไม่ผ่าน จะโยน exception ออกไปเลยโดยที่ Handler ไม่ต้องเขียนโค้ดเช็คเอง

**ทำไมต้องใช้ / ประโยชน์:** ไม่ต้องเขียน `if (!ข้อมูลถูกต้อง) throw ...` หรือเช็ค permission ซ้ำทุก Handler — เขียน validator หรือ policy ครั้งเดียว ทุก Command/Query ที่ผ่าน Mediator จะได้พฤติกรรมนี้อัตโนมัติเหมือนกันหมด แก้จุดเดียวมีผลทั้งระบบ

### 0.5 Dependency Injection (DI) และ Dependency Inversion

**คืออะไร:** แทนที่ class จะ "สร้าง" สิ่งที่ต้องใช้ขึ้นมาเอง (เช่น `new CpaDatabaseContext()`) ให้ "รับ" มันเข้ามาทาง constructor แทน แล้วปล่อยให้ตัวกลาง (DI container ของ ASP.NET Core) เป็นคนสร้างของจริงมาป้อนให้เองตอนรันจริง

**ทำงานอย่างไรในโปรเจกต์นี้:** Application layer (ชั้นในที่ห้ามรู้จัก EF Core) นิยาม interface `ICpaDatabaseContext` ไว้เอง (`src/Application/Common/Interfaces/ICpaDatabaseContext.cs`) ส่วน Infrastructure (ชั้นนอก) เป็นคน implement มันจริงด้วย EF Core (`src/Infrastructure/Persistence/CpaDatabaseContext.cs`) แล้วลงทะเบียนไว้ที่ `Infrastructure/DependencyInjection.cs` เวลา Handler ต้องการใช้ฐานข้อมูล ก็แค่ขอ `ICpaDatabaseContext` ผ่าน constructor — DI container จะหยิบของจริง (`CpaDatabaseContext`) มาป้อนให้เองโดย Handler ไม่ต้องรู้เลยว่าเบื้องหลังเป็น EF Core หรือ PostgreSQL

**ทำไมต้องใช้ / ประโยชน์:**

- นี่คือกลไกที่ทำให้ข้อ 0.1 (Clean Architecture) เป็นจริงได้จริง ๆ ไม่ใช่แค่ทฤษฎี — เรียกว่า **Dependency Inversion** (ตัว "D" ใน SOLID, ดูข้อ 0.6): ชั้นในกำหนด "สัญญา" (interface) ไว้ก่อน ชั้นนอกเป็นฝ่ายเดินตามสัญญานั้น ทำให้ทิศทางการพึ่งพา "กลับด้าน" จากที่ควรจะเป็นถ้าเขียนตรงไปตรงมา (ปกติควรจะเป็น business logic ไปเรียก EF Core ตรง ๆ)
- เทส Handler ได้โดยไม่ต้องต่อฐานข้อมูลจริง — ตอนเขียน unit test สามารถสร้าง `ICpaDatabaseContext` ปลอม (mock/fake) แทนของจริงได้
- เปลี่ยน implementation เบื้องหลังได้โดยไม่กระทบโค้ดที่เรียกใช้เลย

### 0.6 SOLID Principles

README ของโปรเจกต์ระบุไว้ว่าใช้หลักการนี้ — สรุปแต่ละตัวสั้น ๆ พร้อมตัวอย่างจริงในโปรเจกต์นี้:

- **S — Single Responsibility:** หนึ่ง Handler รับผิดชอบแค่หนึ่ง use case (ข้อ 0.2) ไม่ทำหลายอย่างปนกัน
- **O — Open/Closed:** เพิ่ม Pipeline Behaviour ใหม่ได้ (ข้อ 0.4) โดยไม่ต้องแก้โค้ด MediatR หรือ Handler เดิมที่มีอยู่แล้วเลย
- **L — Liskov Substitution:** ทุก Behaviour/Handler ทำงานผ่าน interface กลางของ MediatR แทนกันได้ตามสัญญาเดียวกัน
- **I — Interface Segregation:** `ICpaDatabaseContext` เปิดเผยเฉพาะสิ่งที่ Application ต้องใช้จริง ไม่ใช่ทุกความสามารถของ EF Core `DbContext` ทั้งหมด
- **D — Dependency Inversion:** ตามข้อ 0.5 ด้านบน — Application กำหนด interface, Infrastructure เดินตาม

### 0.7 pattern อื่น ๆ ที่ใช้ในโปรเจกต์ (โดยสรุป)

- **FluentValidation** — เขียนกติกาตรวจสอบข้อมูลของแต่ละ Command/Query แบบอ่านง่าย (`RuleFor(x => x.Name).NotEmpty()`) แทนการเขียน `if` เช็คทีละเงื่อนไข ทำงานร่วมกับ `ValidationBehaviour` อัตโนมัติ (ข้อ 0.4)
- **AutoMapper** — แปลง Entity เป็น ViewModel/DTO แบบอิงตามชื่อ property (convention-based) แทนการเขียนโค้ด map ทีละ field เอง
- **Interceptor pattern (audit trail + concurrency)** — `AuditableEntitySaveChangesInterceptors` ดักทุกครั้งที่มีการบันทึกข้อมูลลงฐานข้อมูล เพื่อเซ็ต `CreatedBy`/`LastModified` และบันทึกประวัติการเปลี่ยนแปลงลงตาราง `AuditLog` โดยอัตโนมัติ — Handler ไม่ต้องเขียนโค้ดส่วนนี้เองเลยสักที่เดียว
- **Optimistic Concurrency** — ทุก entity มี concurrency token (ใช้ค่า `xmin` ของ PostgreSQL) ถ้าสองคนแก้ข้อมูลแถวเดียวกันพร้อมกัน คนที่บันทึกทีหลังจะได้ error แทนที่จะเขียนทับข้อมูลของอีกคนแบบเงียบ ๆ

รายละเอียดเชิงเทคนิคแบบเต็มของทุกหัวข้อด้านบน (data model conventions, bounded-context folder map ฯลฯ) อยู่ที่ [architecture.md](architecture.md)

### 0.8 ก่อนเริ่มแก้โค้ดจริง ควรอ่านอะไรก่อน

ไม่ต้องอ่านทั้งหมดรวดเดียวตอนเริ่มงานวันแรก แต่ก่อนเริ่ม**แก้โค้ดจริง**ในแต่ละงาน ให้เช็คเอกสารพวกนี้ก่อนเสมอ (ลำดับเดียวกับที่ AI agent ในโปรเจกต์นี้ถูกสั่งให้อ่านก่อนแก้โค้ดทุกครั้ง — ดู [prompt-templates.md](prompt-templates.md)):

1. **[known-issues.md](known-issues.md)** — สิ่งที่กำลังจะแก้ เคยมีปัญหาที่รู้อยู่แล้วหรือเปล่า? เช็คก่อนเสีย เวลาแก้ปัญหาเดิมซ้ำ หรือแก้แบบขัดกับของเดิมที่เคยแก้ไปแล้ว
2. **[domain.md](domain.md)** — ศัพท์ภาษาไทยที่ใช้ในงาน ถ้างานที่ทำเกี่ยวกับคำพวกนี้ ให้ใช้ศัพท์ตามที่นิยามไว้ในนี้ อย่าตั้งชื่อเอาเองใหม่
3. **[coding-rules.md](coding-rules.md)** — กฎการตั้งชื่อและกฎเทคนิคที่บังคับ (concurrency, ledger, audit trail, permission) ไม่ใช่ทางเลือก และมีกฎที่ห้ามฝ่าฝืนของทั้งโปรเจกต์ (เช่น ห้ามแก้ยอดเงินแบบเขียนทับตรง ๆ ต้องผ่าน ledger)
4. **[ai-agent-guide.md](ai-agent-guide.md)** — โค้ดแต่ละส่วนควรไปอยู่ตรงไหน, กฎ MUST/MUST NOT

ถ้าทำงานร่วมกับ AI (เช่น Claude Code) ในงานนี้ ให้ดู [prompt-templates.md](prompt-templates.md) ด้วย — มี prompt สำเร็จรูปสำหรับแต่ละสเตจของงาน (Entity ใหม่, CQRS feature, unit test, functional test, frontend, e2e) ให้ copy ไปวางแทนที่จะพิมพ์บริบทเองใหม่ทุกครั้ง และดู [`requirements/README.md`](../requirements/README.md) สำหรับวิธีเขียน requirement ของ feature ก่อนเริ่มพัฒนา

---

## ส่วนที่ 1 — เครื่องมือที่ต้องติดตั้งก่อนเริ่มงาน

ติดตั้งตามลำดับด้านล่างนี้ แต่ละหัวข้อจะอธิบายว่า **คืออะไร** และ **ทำไมโปรเจกต์นี้ถึงต้องใช้** เพื่อให้เข้าใจภาพรวมไปด้วย ไม่ใช่แค่ทำตามแล้วลืม

### 1.1 Git + SourceTree

**คืออะไร:** Git คือระบบควบคุมเวอร์ชันของซอร์สโค้ด (version control) ส่วน [SourceTree](https://www.sourcetreeapp.com/) คือโปรแกรม GUI สำหรับใช้งาน Git โดยไม่ต้องพิมพ์คำสั่งเองทั้งหมด

**ทำไมต้องใช้:** ทีมนี้ทำงานกับ repo ใน local folder `D:\PROJECT\CPA` (ทุกเครื่องใช้ path เดียวกัน) ทุกคนต้องสร้าง branch/commit/merge ผ่าน Git โดยไม่ push ขึ้น GitHub — SourceTree ช่วยให้เห็น branch, diff, และ conflict เป็นภาพ ทำให้ทำงานร่วมกันผิดพลาดน้อยลง (ดูขั้นตอน Git แบบเต็มที่ [README.md](../README.md) หัวข้อ "ขั้นตอนเข้าร่วมพัฒนา")

**ติดตั้ง:**

- Windows/macOS: ดาวน์โหลด Git จาก [git-scm.com](https://git-scm.com/) และ SourceTree จาก [sourcetreeapp.com](https://www.sourcetreeapp.com/)
- ตรวจสอบว่าติดตั้งสำเร็จ: `git --version`

### 1.2 Windows Terminal (สำหรับผู้ใช้ Windows)

**คืออะไร:** โปรแกรม terminal ตัวใหม่ของ Microsoft ที่รองรับหลายแท็บ, หลาย shell (PowerShell, Command Prompt, WSL, Git Bash) ในหน้าต่างเดียว

**ทำไมต้องใช้:** งานพัฒนาโปรเจกต์นี้ต้องเปิด terminal พร้อมกันหลายตัว (backend, frontend, database) Windows Terminal ทำให้สลับไปมาระหว่างแท็บได้สะดวกกว่า Command Prompt แบบเดิมมาก และเป็นสิ่งที่ทีมนี้ใช้เป็นมาตรฐาน

**ติดตั้ง:** จาก Microsoft Store ค้นหา "Windows Terminal" หรือดาวน์โหลดจาก [GitHub releases ของ Microsoft/terminal](https://github.com/microsoft/terminal/releases)

### 1.3 .NET SDK

**คืออะไร:** ชุดเครื่องมือสำหรับ compile และรันโค้ด C#/.NET รวมถึงคำสั่ง `dotnet` ที่ใช้ทั้ง build, run, test, และ migrate database

**ทำไมต้องใช้:** โปรเจกต์นี้ pin เวอร์ชันไว้ที่ `global.json` (ปัจจุบันคือ `10.0.302` ขึ้นไป ในตระกูล .NET 10) เพื่อให้ทุกคนในทีมและเครื่อง build server ใช้ SDK สายเดียวกัน ไม่เจอปัญหา "โค้ดฉันรันได้แต่ของเธอรันไม่ได้"

**ติดตั้ง:**

- ดาวน์โหลดจาก [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download) — เลือก **.NET 10 SDK** (ไม่ใช่แค่ Runtime)
- ตรวจสอบว่าติดตั้งสำเร็จ: `dotnet --version` (ควรได้ `10.0.302` ขึ้นไป — ดูเวอร์ชันที่ pin ไว้ล่าสุดใน `global.json`)

### 1.4 dotnet-ef (Entity Framework Core CLI)

**คืออะไร:** เครื่องมือ CLI แยกต่างหากจาก .NET SDK ใช้สำหรับสร้าง/ลบ/รัน database migration (คำสั่ง `dotnet ef ...`)

**ทำไมต้องใช้:** โปรเจกต์นี้ใช้ Entity Framework Core แบบ Code First — ออกแบบ Entity Class ในโค้ดก่อน แล้วให้ `dotnet ef` สร้างตาราง/แก้ไข schema ในฐานข้อมูลให้อัตโนมัติ ไม่ต้องเขียน SQL DDL เอง

**ติดตั้ง:**

```bash
dotnet tool install --global dotnet-ef
```

ถ้าเคยติดตั้งไว้แล้วแต่เวอร์ชันเก่า อัปเดตด้วย:

```bash
dotnet tool update --global dotnet-ef
```

ตรวจสอบ: `dotnet ef --version`

### 1.5 nvm (Node Version Manager) + Node.js

**คืออะไร:** nvm คือตัวจัดการเวอร์ชันของ Node.js ทำให้ติดตั้ง Node หลายเวอร์ชันในเครื่องเดียวและสลับไปมาได้ตามแต่ละโปรเจกต์ต้องการ

**ทำไมต้องใช้ nvm แทนการติดตั้ง Node ตรง ๆ:** โปรเจกต์นี้ pin เวอร์ชัน Node ไว้ที่ `src/vuewebui/.nvmrc` (ปัจจุบันคือ `lts/*` แปลว่า "Node LTS เวอร์ชันล่าสุด ณ ตอนนั้น") ถ้าคุณทำงานหลายโปรเจกต์ที่ต้องการ Node คนละเวอร์ชัน การติดตั้ง Node ตรง ๆ จะสลับเวอร์ชันไม่ได้ ต้องถอนแล้วลงใหม่ทุกครั้ง — nvm แก้ปัญหานี้

**ติดตั้ง (macOS / Linux)** — ใช้ [nvm-sh/nvm](https://github.com/nvm-sh/nvm):

```bash
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.1/install.sh | bash
# ปิดแล้วเปิด terminal ใหม่ หรือ source ~/.zshrc / ~/.bashrc ก่อน แล้วค่อยรันคำสั่งด้านล่าง
```

**ติดตั้ง (Windows)** — nvm ตัวจริงไม่รองรับ Windows โดยตรง ให้ใช้ [nvm-windows (coreybutler/nvm-windows)](https://github.com/coreybutler/nvm-windows/releases) แทน (ดาวน์โหลด `nvm-setup.exe`)

**วิธีสลับ/ติดตั้งเวอร์ชัน Node ตาม `.nvmrc` ของโปรเจกต์:**

macOS/Linux (nvm ตัวจริงอ่าน `.nvmrc` ให้อัตโนมัติ):

```bash
cd src/vuewebui
nvm install    # อ่านเวอร์ชันจาก .nvmrc แล้วติดตั้งให้เลย
nvm use        # สลับไปใช้เวอร์ชันนั้น
```

Windows (nvm-windows **ไม่** อ่าน `.nvmrc` อัตโนมัติ ต้องเปิดไฟล์ดูเวอร์ชันเองแล้วพิมพ์คำสั่ง):

```powershell
nvm install lts
nvm use <เวอร์ชันที่ติดตั้ง เช่น 22.11.0>
```

ตรวจสอบว่าใช้เวอร์ชันถูกต้อง: `node --version`

> เวลาเปิด terminal ใหม่ทุกครั้งต้องเช็คว่า `nvm use` เวอร์ชันที่ถูกต้องหรือยัง โดยเฉพาะถ้าสลับไปทำโปรเจกต์อื่นที่ใช้ Node คนละเวอร์ชันมาก่อน

### 1.6 pnpm

**คืออะไร:** ตัวจัดการแพ็กเกจ (package manager) ของ JavaScript คล้าย `npm`/`yarn` แต่เร็วกว่าและประหยัดพื้นที่ดิสก์กว่า เพราะแชร์ไฟล์แพ็กเกจเดียวกันระหว่างโปรเจกต์แทนที่จะดาวน์โหลดซ้ำ

**ทำไมต้องใช้:** ฝั่ง frontend (`src/vuewebui`) ตั้งค่าไว้เป็น pnpm workspace แล้ว (มีไฟล์ `pnpm-lock.yaml`, `pnpm-workspace.yaml`) ถ้าใช้ `npm install` หรือ `yarn install` แทน จะได้ dependency tree ไม่ตรงกับที่ทีมใช้จริง อาจเจอบั๊กที่คนอื่นไม่เจอ

**ติดตั้ง (แนะนำ ใช้ corepack ที่มากับ Node อยู่แล้ว — ใช้ได้ทั้ง Windows/macOS/Linux):**

```bash
corepack enable
corepack prepare pnpm@latest --activate
```

ถ้า corepack มีปัญหา ติดตั้งตรง ๆ ผ่าน npm แทนได้:

```bash
npm install -g pnpm
```

ตรวจสอบ: `pnpm --version`

### 1.7 PostgreSQL

**คืออะไร:** ฐานข้อมูลเชิงสัมพันธ์ (relational database) ที่โปรเจกต์นี้ใช้เก็บข้อมูลจริง

**ทำไมต้องติดตั้งแบบลงเครื่องจริง ไม่ใช่ Docker:** โปรเจกต์นี้ตั้งใจ**ไม่ใช้ Docker/Testcontainers สำหรับพัฒนาและเทสประจำวัน** ให้ติดตั้ง PostgreSQL server ลงเครื่องโดยตรงแล้วรันทิ้งไว้ตลอด เพื่อความเร็วและความเสถียรตอนเทส

**ติดตั้ง:**

- Windows/macOS: ดาวน์โหลด installer จาก [postgresql.org/download](https://www.postgresql.org/download/) (แนะนำเวอร์ชัน 16.x)
- macOS ทางเลือก: `brew install postgresql@16 && brew services start postgresql@16`
- ระหว่างติดตั้งจะถูกถามให้ตั้ง password ของ superuser (`postgres`) — จำไว้ให้ดี

**เครื่องมือดูข้อมูล (แนะนำ ไม่บังคับ):** [pgAdmin 4](https://www.pgadmin.org/) (มากับ installer อยู่แล้วส่วนใหญ่) หรือ [DBeaver](https://dbeaver.io/) สำหรับเปิดดู/แก้ข้อมูลในตารางแบบ GUI

ตรวจสอบว่าติดตั้งสำเร็จและเชื่อมต่อได้: `psql -U postgres -h localhost`

### 1.8 Docker Desktop (ไม่บังคับ)

**คืออะไร:** โปรแกรมรัน container บนเครื่อง — แพ็กแอปพร้อม dependency ทั้งหมดไว้ใน container เดียว รันที่ไหนก็ได้ผลเหมือนกัน

**สถานะในโปรเจกต์นี้:** ยังไม่มี Docker image, `Dockerfile` หรือไฟล์ docker-compose ของโปรเจกต์ งานพัฒนา/เทสประจำวันไม่ต้องใช้ Docker (ดูข้อ 1.7) ส่วนสคริปต์และเอกสาร deploy ก็ยังไม่มีในโปรเจกต์ — ยังไม่ได้เขียน จะติดตั้ง Docker ไว้ใช้งานอื่นเองก็ได้ ไม่บังคับ

**ติดตั้ง:** ดาวน์โหลดจาก [docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop/)

ตรวจสอบ: `docker --version` และ `docker compose version`

### 1.9 Editor: Visual Studio Code หรือ Visual Studio 2026

เลือกใช้ตัวใดตัวหนึ่งก็ได้ตามความถนัด ทีมนี้ใช้ทั้งสองแบบ

**Visual Studio Code** (แนะนำถ้าต้องการ editor เบา ๆ ตัวเดียวคุมทั้ง backend และ frontend) — ดาวน์โหลดจาก [code.visualstudio.com](https://code.visualstudio.com/) แล้วติดตั้ง extension เหล่านี้เพิ่ม:

| Extension | ทำไมต้องมี |
| --- | --- |
| **C# Dev Kit** (`ms-dotnettools.csdevkit`) | IntelliSense, debug, และรัน unit test ของฝั่ง .NET โดยตรงใน VS Code |
| **Vue - Official** (`Vue.volar`) | รองรับไฟล์ `.vue` (syntax highlight, IntelliSense) ของฝั่ง frontend |
| **ESLint** (`dbaeumer.vscode-eslint`) | โปรเจกต์บังคับ lint ผ่าน ESLint (`.eslintrc.cjs`) — extension นี้ขึ้นเตือน error/warning ให้เห็นทันทีในไฟล์ ไม่ต้องรอรัน `pnpm lint` |
| **EditorConfig for VS Code** (`EditorConfig.EditorConfig`) | อ่านค่า indent/charset จากไฟล์ `.editorconfig` ของโปรเจกต์ ให้ format ตรงกันทุกคนอัตโนมัติ |

**Visual Studio 2026** — ถ้าถนัดฝั่ง backend มากกว่า เปิด `cpa.sln` ได้เลย เหมาะกับคนที่ทำงานฝั่ง .NET เป็นหลักและอยากได้ debugger ที่ครบเครื่องกว่า (ฝั่ง frontend ยังต้องใช้ terminal/VS Code แยกอยู่ดี เพราะ Visual Studio ไม่รองรับ Vue โดยตรง)

### 1.10 (ไม่บังคับ) เครื่องมือทดสอบ API

ปกติใช้ **Swagger UI** ที่มากับโปรเจกต์อยู่แล้วพอ (เปิดเบราว์เซอร์ไปที่ URL ของ API ต่อท้ายด้วย `/swagger` ก็ทดสอบยิง endpoint ได้เลย ไม่ต้องติดตั้งอะไรเพิ่ม) แต่ถ้าต้องการเครื่องมือแยกสำหรับเทสเคสที่ซับซ้อนกว่า (เก็บ request เป็นชุด, ตั้ง environment variable) แนะนำ [Postman](https://www.postman.com/) หรือส่วนขยาย **REST Client** ใน VS Code

---

## ส่วนที่ 2 — ตั้งค่าโปรเจกต์ครั้งแรก (ทำครั้งเดียว)

### 2.1 Clone โปรเจกต์

```bash
git clone https://github.com/tansomros/cpa.git
cd cpa
```

(หรือ clone ผ่าน SourceTree ก็ได้ตามที่อธิบายไว้ในข้อ 1.1)

### 2.2 ติดตั้ง dependency ทั้งสองฝั่ง

```bash
# ฝั่ง backend (.NET) — รันที่ root ของ repo
dotnet restore

# ฝั่ง frontend (Vue) — เข้าไปในโฟลเดอร์ vuewebui ก่อน
cd src/vuewebui
nvm use          # ให้แน่ใจว่าใช้ Node เวอร์ชันตาม .nvmrc ก่อน (ดูข้อ 1.5)
pnpm install
cd ../..
```

### 2.3 สร้างฐานข้อมูล (ทำครั้งเดียวต่อเครื่อง)

เปิด `psql` หรือ pgAdmin แล้วสร้าง 2 ฐานข้อมูล — ตัวหนึ่งไว้ใช้พัฒนาจริง อีกตัวไว้ให้ automated test ใช้แยกกัน จะได้ไม่กระทบข้อมูลที่กำลังพัฒนาอยู่:

```sql
CREATE DATABASE cpathai OWNER cpat;
CREATE DATABASE "cpathai-test" OWNER cpat;
```

> ถ้ายังไม่มี role/user ชื่อ `cpa` ต้องสร้างก่อน เช่น `CREATE USER cpat WITH PASSWORD 'cpat' SUPERUSER;` แล้วปรับ connection string ให้ตรงกับ password ที่ตั้งจริง

connection string อยู่ที่:

- `src/API/appsettings.Development.json` → key `ConnectionStrings:Database` (สำหรับรัน API ตอนพัฒนา)
- `tests/Application.FunctionalTests/appsettings.json` → key `ConnectionStrings:CpaDb` (สำหรับ automated test)

### 2.4 สร้าง/เชื่อถือใบรับรอง HTTPS สำหรับเครื่อง dev (ทำครั้งเดียว)

ทั้ง API (`https://localhost:7114`) และหน้าเว็บตอนพัฒนา (`https://localhost:5173`) รันผ่าน HTTPS บนเครื่อง ถ้าไม่เคย trust ใบรับรองของ .NET dev cert มาก่อน เบราว์เซอร์จะขึ้นเตือน "ไม่ปลอดภัย" ทุกครั้ง แก้ครั้งเดียวจบด้วย:

```bash
dotnet dev-certs https --trust
```

(ฝั่ง frontend ตอนรัน `pnpm dev` ครั้งแรก จะเรียก `dotnet dev-certs` เพื่อ export ใบรับรองเดียวกันไปใช้เองอัตโนมัติ ไม่ต้องตั้งค่าเพิ่ม)

---

## ส่วนที่ 3 — รันโปรเจกต์ในเครื่อง (ทำทุกวันที่พัฒนา)

เปิด 2 terminal (ใน Windows Terminal เปิดเป็น 2 แท็บได้เลย):

```bash
# Terminal 1: Backend
dotnet run --project src/API

# Terminal 2: Frontend
cd src/vuewebui && pnpm dev
```

- **Swagger UI (ทดสอบ API):** `https://localhost:7114/swagger`
- **หน้าเว็บ (frontend dev server):** `https://localhost:5173`

> โปรเจกต์นี้ตั้งค่า **fake auth** ไว้ให้แล้วทั้งสองฝั่งตอนรันแบบ Development (`Identity:UseFakeAuth` ใน `appsettings.Development.json` และ `VITE_USE_FAKE_AUTH` ใน `.env.development`) แปลว่า **ไม่ต้องมี account ระบบ identity จริงก็ล็อกอินเข้าใช้งานได้ทันที** เหมาะสำหรับตอนเริ่มพัฒนา/เทรน — เห็น warning "FAKE AUTH ACTIVE" ตอนรันก็ไม่ต้องตกใจ เป็นเรื่องปกติของโหมด dev

### 3.1 การ Debug (ตั้ง breakpoint)

**Backend (VS Code):** โปรเจกต์มีไฟล์ `.vscode/launch.json` ให้พร้อมใช้แล้ว — เปิด repo ใน VS Code, ไปที่แท็บ **Run and Debug** (`Ctrl+Shift+D` / `Cmd+Shift+D`) เลือก **▶ Debug API** แล้วกด `F5` ได้เลย จะ build ให้อัตโนมัติ (ผ่าน task `build-api`), รันด้วย breakpoint ใช้งานได้จริง, และเปิดเบราว์เซอร์ไปที่ Swagger ให้เองเมื่อ API พร้อม — คลิกซ้ายที่ขอบซ้ายของเลขบรรทัดในโค้ดเพื่อตั้ง breakpoint ตามปกติ

**Backend (Visual Studio 2026):** เปิด `cpa.sln` แล้วกด `F5` ตามปกติ (ตั้งค่า `API` เป็น Startup Project ถ้ายังไม่ได้ตั้ง — คลิกขวาที่ project `API` → **Set as Startup Project**)

**Frontend (Vue):** ปกติไม่ค่อยตั้ง breakpoint ในไฟล์ `.vue` ผ่าน editor กันตรง ๆ แต่ debug ผ่านเบราว์เซอร์แทน — เปิด DevTools ของเบราว์เซอร์ (`F12`) ตั้ง breakpoint ในแท็บ Sources ได้เหมือน JavaScript ทั่วไป (มี source map ให้แล้ว) และแนะนำติดตั้งส่วนขยาย [Vue DevTools](https://devtools.vuejs.org/) เพื่อดู component tree, props, state, Pinia store แบบเรียลไทม์

### 3.2 การทำงานร่วมกับ AI ในแต่ละงาน (workflow แบบ hybrid)

โปรเจกต์นี้ตั้งใจให้ใช้ AI (เช่น Claude Code) เป็นตัวช่วยหลักในการเขียนโค้ด/เทส ไม่ใช่แค่เครื่องมือเสริม — ขั้นตอนทำงานต่อ 1 งาน (feature/บั๊ก) เป็นแบบนี้:

1. **หา/เขียน requirement ก่อน** — เช็คว่ามีไฟล์ requirement ของงานนี้อยู่แล้วใน [`requirements/`](../requirements/) หรือยัง ถ้ายังไม่มี ให้คัดลอก [`requirements/_template.md`](../requirements/_template.md) มากรอกก่อนเริ่ม (ดูวิธีที่ [`requirements/README.md`](../requirements/README.md)) — ขั้นตอนนี้ห้ามข้าม เพราะเป็นบริบทหลักที่ทั้งคนในทีมและ AI ใช้ร่วมกัน
2. **เปิด session กับ AI** — วาง **shared preamble** + prompt template ของสเตจที่จะทำ (Domain entity, CQRS feature, unit test, functional test, frontend, e2e) จาก [prompt-templates.md](prompt-templates.md) แล้วชี้ไปที่ไฟล์ requirement จากข้อ 1
3. **ให้ AI วางแผนก่อน อย่าเพิ่งให้ลงมือแก้โค้ดทันที** — ต่อท้าย prompt ด้วยประมาณ "ก่อนเริ่มแก้โค้ด ช่วยสรุปแผนการทำงานให้ดูก่อน ยังไม่ต้องแก้ไฟล์จริง" (ถ้าใช้ Claude Code สามารถสั่งเข้า **Plan Mode** ได้โดยตรง) แผนควรบอกว่าจะแตะไฟล์ไหนบ้าง แก้อะไร และจะจัดการ business rule/ledger/permission ตาม requirement ยังไง — **ให้ทีมอ่านแผนนี้และปรับแก้ก่อน** ถ้าแผนเข้าใจ requirement ผิด หรือพลาดกฎที่ห้ามฝ่าฝืนใน coding-rules.md (ข้อ 0.8) ให้แก้ตรงนี้ก่อน อย่ารอไปเจอตอนโค้ดเสร็จแล้ว — ตกลงแผนกันแล้วค่อยบอก AI ให้เริ่มลงมือทำจริง
4. **อ่าน diff ที่ AI แก้ก่อนเชื่อ** — ต่อให้แผนผ่านการรีวิวแล้ว AI ก็ยังพลาดตอน implement จริงได้ ต้องอ่านโค้ดที่ได้จริงอีกรอบ ไม่ใช่แค่เห็นว่า build ผ่านแล้วเชื่อทันที โดยเฉพาะจุดที่แตะยอดเงิน หรือ concurrency
5. **รัน build/test เองเสมอ** ตามข้อ 4–5 ของคู่มือนี้ ก่อนจะถือว่างานเสร็จ — AI อาจรายงานว่า "ทดสอบผ่านแล้ว" แต่ต้องรันยืนยันเองอีกครั้งเสมอ
6. **อัปเดตสถานะ requirement** ในไฟล์ `requirements/{Context}/{feature}.md` (ร่าง → พร้อมพัฒนา → กำลังพัฒนา → เสร็จสิ้น) แล้ว commit ตามขั้นตอน Git ปกติ (ส่วนที่ 9)

สรุปสั้น ๆ: **AI เขียนโค้ดได้ แต่ความรับผิดชอบต่อความถูกต้องยังเป็นของคนเสมอ** — ระบบนี้เกี่ยวข้องกับข้อมูลจริงของร้านยาและผู้ป่วย (รวมถึงข้อมูลการให้บริการ MTM และผลแล็บ) การรีวิวก่อน merge จึงสำคัญไม่ว่าใครหรืออะไรเป็นคนเขียนโค้ดนั้นมา

**ทางเลือกแบบอัตโนมัติ**: ขั้นตอนที่ 2–6 ด้านบน (เปิด session, วาง template, ให้ AI วางแผน, รีวิว diff, อัปเดตสถานะ) มีเวอร์ชันที่ AI ขับเคลื่อนเองต่อกันได้ทั้งหมดโดยไม่ต้องเปิด session ใหม่ทีละสเตจ — ดู [automate-workflow.md](automate-workflow.md) (เอกสารสำหรับ AI agent เท่านั้น) ยังคงต้องมี**แผนที่ทีมอนุมัติก่อนทุกสเตจ**เหมือนเดิม เพียงแต่ AI เป็นคนไล่ไปทีละสเตจเองแทนที่ทีมจะต้องเปิด session ใหม่ทุกครั้ง เอกสารนั้นยังครอบคลุมขั้นตอนที่ 1 (เขียน requirement) แบบอัตโนมัติด้วย — AI สรุปจากบทสนทนาของทีมแล้วเขียนไฟล์ให้เอง โดยสถานะ "พร้อมพัฒนา" ยังคงต้องให้คนเป็นผู้กดยืนยันเองเสมอ

### 3.3 เตรียมเนื้อหาการประชุมก่อนส่งให้ AI เขียน requirement (แนะนำ: บันทึกประชุม + NotebookLM)

Claude Code ไม่มีความสามารถ "ฟัง" การประชุมสดด้วยตัวเอง (ไม่มีไมโครโฟน ไม่มี bot เข้าร่วมประชุม) — ขั้นตอนที่ 1 ใน [automate-workflow.md](automate-workflow.md) ต้องการ**ข้อความ** ของบทสนทนา ไม่ใช่เสียงสด ทีมจึงต้องเตรียมเนื้อหาก่อนส่งให้ AI ด้วยขั้นตอนนี้:

1. **บันทึกการประชุม** ด้วยเครื่องมือที่ใช้อยู่แล้ว (Google Meet/Zoom/Teams) — เปิดฟีเจอร์บันทึก (recording) หรือ live caption/transcript ที่แพลตฟอร์มมีให้อยู่แล้ว ไม่ต้องติดตั้งอะไรเพิ่ม
2. **อัปโหลดเข้า [NotebookLM](https://notebooklm.google.com/)** เป็น source ใหม่ — อัปโหลดไฟล์บันทึก (เสียง/วิดีโอ) หรือ transcript ที่แพลตฟอร์มประชุมสร้างให้ก็ได้ NotebookLM จะ clean up เป็น transcript ที่อ่านง่ายให้ภายใน 1–2 นาที (ข้อควรรู้: NotebookLM **ไม่ได้ฟังสด** เป็นการอัปโหลดเสียง/ทรานสคริปต์หลังประชุมเสร็จเข้าไปให้มันประมวลผล)
3. **สั่งให้ NotebookLM สรุปให้ตรงกับโครงสร้าง requirement ของโปรเจกต์นี้เลย** แทนที่จะให้สรุปแบบทั่วไป จะได้ไม่ต้องมาเรียบเรียงใหม่ทีหลัง ตัวอย่าง prompt ที่ใช้ถามใน NotebookLM:

   > "สรุปการประชุมนี้สำหรับเขียนเอกสาร requirement ของระบบซอฟต์แวร์ แยกหัวข้อดังนี้: (1) เป้าหมาย/ปัญหาที่แก้ และใครเป็นผู้ใช้งาน (2) ขอบเขตที่ทำรอบนี้ กับขอบเขตที่ยังไม่ทำ (3) ฟิลด์ข้อมูลที่พูดถึง และฟิลด์ไหนบังคับกรอก (4) กติกาทางธุรกิจ/validation ที่พูดถึง (5) feature นี้แตะยอดเงินหรือไม่ (6) ใครควรมีสิทธิ์เข้าถึง (7) มีอะไรที่คุยกันแล้วยังไม่สรุปหรือยังไม่ตกลงกันบ้าง"

   คำถามนี้เทียบเคียงกับ 14 หัวข้อใน [`requirements/_template.md`](../requirements/_template.md) โดยตรง ทำให้เอาไปวางต่อให้ AI ได้เกือบทันที
4. **คัดลอกผลสรุปจาก NotebookLM มาวางในแชทกับ AI** (เช่น Claude Code) แล้วบอกประมาณ "ช่วยเขียนเป็น requirement ของ feature {ชื่อ}" — AI จะทำตามขั้นตอนที่ 1 ของ [automate-workflow.md](automate-workflow.md) ต่อเอง (เขียนไฟร์ที่ `requirements/{Context}/{feature}.md`, สถานะ "ร่าง" เสมอ, และจะถามกลับถ้าเนื้อหาไม่ชัดเจนพอ **โดยเฉพาะข้อ 7 เรื่องผลกระทบต่อยอดเงินคงเหลือที่ AI จะไม่เดาเองเด็ดขาด**)

ข้อควรระวัง: เนื้อหาการประชุมอาจมีข้อมูลภายใน/ข้อมูลผู้ป่วยปะปนอยู่ได้ — ตรวจสอบก่อนอัปโหลดเข้าเครื่องมือภายนอกใด ๆ (รวมถึง NotebookLM) ว่าไม่มีข้อมูลที่ไม่ควรออกนอกองค์กรอยู่ในนั้น

### 3.4 วิธีพูดคุยในที่ประชุมให้ AI เก็บ requirement ได้ง่ายและแม่นยำ

ต่อให้เสียงชัดและทรานสคริปต์ดี ถ้าคุยกันแบบพูดคลุมเครือ/พูดแทรกกัน AI (หรือแม้แต่คนที่มาอ่านทีหลัง) ก็ยังตีความผิดได้ — หัวข้อนี้เป็น pattern การพูดที่ช่วยให้เนื้อหาแม่นยำขึ้นตั้งแต่ต้นทาง ไม่ต้องท่องจำให้ครบทุกข้อ แต่ยิ่งทำได้มาก transcript ยิ่งแปลงเป็น requirement ได้ตรงและเร็วขึ้น:

#### ก่อนเริ่มคุยเนื้อหา

- พูดชื่อ feature และ bounded context (ถ้ารู้) ให้ชัดตั้งแต่ประโยคแรก เช่น "วันนี้คุยเรื่อง feature ผลแล็บของผู้ป่วย อยู่ใน LabResults context" — กันไม่ให้ AI ต้องเดาว่ากำลังคุยเรื่องอะไร
- ตกลงกันว่าใครจะเป็นคนพูดสรุปปิดท้ายประชุม (ดูข้อสุดท้ายด้านล่าง)

#### ระหว่างคุย

- **แยกให้ชัดระหว่าง "ตกลงแล้ว" กับ "ยังไม่ตกลง"** — ใช้คำเปิดประโยคที่ตายตัว เช่น "สรุปว่า...", "ตกลงกันว่า..." สำหรับเรื่องที่จบแล้ว กับ "อันนี้ยังไม่ชัด ต้องคุยต่อ", "ยังไม่แน่ใจ" สำหรับเรื่องที่ค้างอยู่ — AI ถูกสั่งให้เขียนสถานะ "ไม่แน่ใจ — ต้องคุยกับทีมก่อน" เมื่อเจอเรื่องแบบหลัง แทนที่จะเดา ดังนั้นการพูดแยกให้ชัดจะช่วยให้แยกได้ถูกจริง ๆ
- **พูดขอบเขตแบบขีดเส้นชัด** — "รอบนี้ทำแค่...", "ยังไม่รวม..." แทนการปล่อยให้ตีความเอาเอง
- **พูดฟิลด์ข้อมูลให้ครบสามอย่างในประโยคเดียว**: ชื่อฟิลด์ + บังคับหรือไม่ + กติกา เช่น "รหัสร้านยา เป็นข้อความ บังคับกรอก ห้ามซ้ำ" แทนที่จะพูดแค่ "ต้องมีรหัสร้านยาด้วยนะ" แล้วปล่อยให้ต้องเดาเรื่อง validation
- **พูดเรื่องผลกระทบต่อยอดเงินให้ชัดเสมอ แม้จะดูชัดอยู่แล้วว่าไม่เกี่ยว** — เช่น "feature นี้ไม่แตะยอดเงินเลย" — ข้อนี้ AI จะไม่เดาเองเด็ดขาดไม่ว่ากรณีใด (ดู [automate-workflow.md](automate-workflow.md)) ถ้าไม่มีใครพูดถึงเลย AI จะถามกลับ เสียเวลากว่าพูดไว้ตั้งแต่ต้น
- **พูดสิทธิ์การเข้าถึงให้ชัดถ้ารู้** — ใครทำอะไรได้บ้าง (เช่น "อันนี้ให้แอดมินทำได้อย่างเดียว")
- **ใช้ศัพท์ให้ตรงกับที่มีอยู่แล้วใน [domain.md](domain.md)** ถ้าจะใช้คำใหม่ ให้พูดออกมาตรง ๆ ว่า "คำนี้ยังไม่มีในระบบ นิยามว่า..." จะได้ติดอยู่ใน transcript ชัดเจน ไม่ใช่แค่พูดผ่าน ๆ
- **หลีกเลี่ยงพูดแทรก/พูดพร้อมกันหลายคน** — นอกจากฟังยากแล้ว ทรานสคริปต์ที่พูดทับกันมักตัดสินไม่ได้ว่าใครพูดอะไรจบตรงไหน ถ้าไม่เห็นด้วย ให้รอพูดต่อจากที่อีกฝ่ายพูดจบประโยค
- **เกณฑ์การยอมรับ พูดเป็นรูปแบบ "เมื่อ [เงื่อนไข] แล้วระบบควรจะ [ผลลัพธ์]" ตรง ๆ เลย** — ตรงกับโครงสร้างเทมเพลตพอดี ไม่ต้องให้ AI มาแปลงรูปประโยคเอง

#### ปิดท้ายประชุม

- **ให้คนที่ตกลงกันไว้พูดสรุปทวนทุกหัวข้อหลักก่อนแยกย้าย**: เป้าหมาย → ขอบเขต (ทำ/ไม่ทำ) → ฟิลด์สำคัญ → กติกาทางธุรกิจ → ผลกระทบต่อยอดเงิน → สิทธิ์การเข้าถึง ให้ทุกคนในห้องยืนยันว่าตรงกับที่เข้าใจ ก่อนที่จะแยกย้ายกันไป — ข้อนี้มีประโยชน์สองต่อ: (1) เป็น sanity check สดในห้องประชุมเลยว่าทุกคนเข้าใจตรงกันจริง ไม่ต้องรอไปเจอความเข้าใจผิดตอนอ่าน requirement ที่เขียนเสร็จแล้ว (2) ทำให้ท้ายทรานสคริปต์มีสรุปที่สะอาด เอาไปให้ AI ใช้เป็นข้อมูลหลักได้ทันทีโดยแทบไม่ต้องรื้อทวนบทสนทนาทั้งหมด

---

## ส่วนที่ 4 — รันเทส

```bash
# Backend — รันทุกโปรเจกต์
dotnet test cpa.sln

# รันเฉพาะโปรเจกต์เดียว
dotnet test tests/Domain.UnitTests/
dotnet test tests/Application.UnitTests/
dotnet test tests/Application.FunctionalTests/   # ต้องมีฐานข้อมูล cpathai-test ตามข้อ 2.3 ไม่ใช้ Docker

# รันเทสเดียวโดยระบุชื่อ
dotnet test --filter "FullyQualifiedName~PatientTests"

# Frontend — lint + build (ต้องผ่านทั้งคู่ก่อน merge)
cd src/vuewebui && pnpm run lint && pnpm run build

# Frontend e2e (Playwright)
cd src/vuewebui && pnpm test:e2e
# ดูแบบ headed (เห็นเบราว์เซอร์จริง) พร้อมหน่วงเวลาให้ดูทัน
PW_SLOW_MO=500 pnpm test:e2e:headed
```

---

## ส่วนที่ 5 — Build

```bash
dotnet build cpa.sln --nologo
```

---

## ส่วนที่ 6 — Database Migration

**ก่อนระบบขึ้นใช้งานจริง** (ยังไม่มีข้อมูลจริงใน production) ลบ migration `InitialCreate` แล้วสร้างใหม่ได้ เพราะข้อมูลตั้งต้นทั้งหมด seed ใหม่ได้ แต่**หลังระบบขึ้นใช้งานจริงแล้ว ต้องเพิ่ม migration ทีละส่วนเท่านั้น** ห้ามลบแล้วสร้างใหม่อีก

**ใครเป็นคนทำ:** คุณ Teerapol เป็นคนลบและสร้าง migration เอง — ทีม AI ห้ามสร้าง ลบ หรือ commit ไฟล์ migration และห้ามรัน migration (`dotnet ef database update` / `Update-Database`) กับฐานข้อมูล `cpathai` โดยไม่ได้รับอนุมัติ

ขั้นตอนลบแล้วสร้าง `InitialCreate` ใหม่ (ใช้ได้เฉพาะช่วงก่อนขึ้นใช้งานจริง):

```bash
# ลบฐานข้อมูลเดิมแล้วสร้างใหม่ (ผ่าน psql/pgAdmin) จากนั้น:
dotnet ef migrations remove --project src/Infrastructure --startup-project src/API   # ถ้ามี migration เดิมอยู่แล้ว
dotnet ef migrations add InitialCreate --project src/Infrastructure --startup-project src/API -o Persistence/Migrations
dotnet ef database update --project src/Infrastructure --startup-project src/API
```

หลังระบบขึ้นใช้งานจริงแล้ว ให้เพิ่ม migration ใหม่ทีละส่วนแทน (`dotnet ef migrations add {ชื่อที่บอกว่าเปลี่ยนอะไร}`) — ห้าม drop-and-regenerate อีกต่อไป (ต้องอัปเดตหัวข้อนี้ตอนนั้นด้วย)

> ถ้าใช้ Visual Studio แทน terminal สามารถใช้ Package Manager Console (`Add-Migration`, `Update-Database`, `Remove-Migration`, `Drop-Database`) แทนคำสั่ง `dotnet ef` ได้เหมือนกัน ดูตัวอย่างคำสั่งเต็มที่ [README.md](../README.md)

---

## ส่วนที่ 7 — สร้างโค้ดใหม่ด้วย Scaffolding

ติดตั้ง template ของโปรเจกต์ครั้งเดียวต่อเครื่อง แล้วรันจากโฟลเดอร์ `Commands` หรือ `Queries` ของ feature:

```bash
dotnet new install ./templates/biglion-templates

cd src/Application/Features/Banks/Commands
dotnet new biglion-command -n CreateBank --featureName Banks --boundedContext Banks --returnType int

cd ../Queries
dotnet new biglion-query -n GetBank --featureName Banks --boundedContext Banks --returnType "BankViewModel"
```

template ยังสร้างโฟลเดอร์ตามชื่อ `-n` (`CreateBank`, `GetBank`) และใส่ segment `BoundedContext` ใน namespace หลังสร้างแล้วให้ย้ายไฟล์เข้า `Commands/Create`, `Commands/Update`, `Commands/Delete` หรือ `Queries/Get` แล้วตั้ง namespace เป็น `BigLion.CPA.Application.Features.{Feature}.Commands.Create` (หรือ `.Update`, `.Delete`, `.Queries.Get`) คำสั่งดึงรายการเดียวกับดึงรายการอยู่โฟลเดอร์ `Get` เดียวกัน รายละเอียดเพิ่มอยู่ที่ [ai-agent-guide.md](ai-agent-guide.md)

---

## ส่วนที่ 8 — มาตรฐานการเขียนโค้ด

กติกาแบบเต็ม (การตั้งชื่อ, การแบ่ง layer, concurrency, audit trail) อยู่ที่ [coding-rules.md](coding-rules.md) — **เป็นข้อบังคับ ไม่ใช่ทางเลือก** สรุปสั้น ๆ ที่ต้องจำให้ขึ้นใจ:

1. Interface ขึ้นต้นด้วย `I` ตัวใหญ่ เช่น `ICpaDatabaseContext`
2. ตั้งชื่อคลาส/ตัวแปร/เมธอด ให้สื่อความหมายในตัวเอง ไม่แน่ใจให้ถามทีม
3. `PascalCase` สำหรับชื่อคลาสและเมธอด เช่น `CpaDatabaseContext`, `SaveChangesAsync()`
4. `PascalCase` สำหรับค่าคงที่ (constant) ทั้ง local และ field
5. `camelCase` สำหรับ method argument, local variable, และ private field
6. private field ขึ้นต้นด้วย `_` เช่น `_context`
7. พารามิเตอร์ในฟังก์ชัน ถ้าไม่เกิน 3 ตัวเขียนบรรทัดเดียวได้ ถ้าเกิน 3 ตัวให้ขึ้นบรรทัดใหม่ทุกตัว
8. เส้นทาง API (route) ใช้ lower-kebab-case ทั้งฝั่ง backend และตอนเรียกจาก frontend
9. ห้ามมี build warning เลย (`TreatWarningsAsErrors` เปิดอยู่)
10. ฝั่ง frontend ต้องผ่าน ESLint (`.eslintrc.cjs` — เป็น config รุ่นเก่าโดยตั้งใจ ห้ามเปลี่ยนไปใช้ `eslint.config.js` แบบใหม่ ดูเหตุผลใน [known-issues.md](known-issues.md))

---

## ส่วนที่ 9 — ขั้นตอน Git และการรวมเข้า master (สรุปย่อ)

รายละเอียดเต็มและกติกาการ commit/merge ดูที่ [README.md](../README.md) หัวข้อ "ขั้นตอนเข้าร่วมพัฒนา" และ "การรวม source code เข้า master" — สรุปสั้น ๆ ตรงนี้:

1. ทำงานใน local repo `D:\PROJECT\CPA` เท่านั้น (ทุกเครื่องใช้ path เดียวกัน) — **ห้าม push ขึ้น GitHub และไม่ใช้ cloud**
2. สร้าง local branch ใหม่จาก `master` ตั้งชื่อตาม [docs/git.md](../docs/git.md) เช่น `feat/patient-birthdate`
3. working tree ใช้ร่วมกันหลายคน — ใครจะสลับ branch ต้องแจ้งทีมก่อนเสมอ
4. stage ไฟล์ทีละ path (`git add <path>`) ห้ามใช้ `git add -A`, `git add .` หรือ `git commit -a` (กันไฟล์ migration ที่คุณ Teerapol ยังไม่ commit ติดไปด้วย)
5. commit เป็นชุดเล็ก ๆ ที่เข้าใจง่าย ข้อความ commit ชัดเจน
6. ก่อนรวมเข้า `master`: build ผ่าน และเทสผ่านครบทั้ง 4 โปรเจกต์ (ส่วนที่ 4–5) รวมถึงต้องมี unit test ของ Entity และ integration test ของ command/query ที่สำคัญ (ดูเงื่อนไขใน README.md)
7. Tech Lead เป็นคน review แล้วจึง merge เข้า `master` ได้หลังคุณ Teerapol อนุมัติเท่านั้น

---

## ส่วนที่ 10 — โครงสร้างโปรเจกต์

```
src/
├── Domain/              # Entity, enum, value object — ไม่มี EF, ไม่มี MediatR request type
├── Application/          # CQRS Features/{Feature}/Commands/{Create|Update|Delete} และ Queries/Get
├── Infrastructure/        # EF Core, DbContext, EF configuration, migrations
├── API/                  # Controller แบบบาง (thin), Program.cs
└── vuewebui/             # Vue 3 frontend (pages/, components/, composables/, e2e/)
tests/
├── Domain.UnitTests/
├── Application.UnitTests/
├── Application.FunctionalTests/    # ใช้ Postgres local ไม่ใช้ Docker
└── Infrastructure.IntegrationTests/
```

กติกาการแบ่ง layer แบบเต็มและแผนผังโฟลเดอร์เป้าหมายดูที่ [architecture.md](architecture.md)

---

## ส่วนที่ 11 — CI/CD และ Deploy

ยังไม่มี CI/CD อัตโนมัติ และยังไม่มีสคริปต์หรือเอกสารการ deploy ขึ้น server จริงในโปรเจกต์ (ยังไม่ได้เขียน)

---

## ส่วนที่ 12 — ปัญหาที่พบบ่อย (Troubleshooting)

| อาการ | สาเหตุที่เป็นไปได้ / วิธีแก้ |
| --- | --- |
| `dotnet run` ฟ้อง connection refused ตอนต่อฐานข้อมูล | PostgreSQL service ไม่ได้รันอยู่ — เช็คด้วย `psql -U postgres -h localhost` ถ้าต่อไม่ได้ ให้เปิด service ก่อน (Windows: Services app หา "postgresql-x64-16"; macOS ผ่าน brew: `brew services start postgresql@16`) |
| ต่อฐานข้อมูลได้ แต่ auth ล้มเหลว (password authentication failed) | password ใน `ConnectionStrings:Database` (`appsettings.Development.json`) ไม่ตรงกับ password จริงของ role `cpat` ที่สร้างไว้ — ดูข้อ 2.3 |
| เบราว์เซอร์ขึ้น "ไม่ปลอดภัย" / `NET::ERR_CERT_AUTHORITY_INVALID` ตอนเปิด `https://localhost:...` | ยังไม่ได้ trust dev cert — รัน `dotnet dev-certs https --trust` (ข้อ 2.4) แล้วรีสตาร์ทเบราว์เซอร์ |
| `pnpm dev` หรือ `pnpm install` ค้าง/error เกี่ยวกับ `dotnet dev-certs` | `pnpm dev` เรียก `dotnet dev-certs` เองเพื่อ export ใบรับรองให้ Vite ใช้ — ถ้า .NET SDK ยังไม่ได้ติดตั้ง หรือยังไม่เคย `dotnet dev-certs https --trust` มาก่อนจะมีปัญหาตรงนี้ ติดตั้ง/trust ให้เรียบร้อยก่อน (ข้อ 1.3, 2.4) |
| `pnpm install` error เกี่ยวกับ native dependency หรือ version mismatch | เช็คว่า `node --version` ตรงกับที่ `.nvmrc` ต้องการหรือยัง (`nvm use` ในโฟลเดอร์ `src/vuewebui` ก่อนเสมอ — ข้อ 1.5); ถ้ายังไม่หายลองลบ `node_modules` แล้ว `pnpm install` ใหม่ |
| Address already in use / port ชนกัน (7114, 5046, 5173) | มีโปรเซสเดิมค้างอยู่ (เช่นรัน `dotnet run` ซ้อนสองรอบ, terminal เก่ายังไม่ได้ปิด) — ปิด process เดิมก่อน หรือหา process ที่จับ port นั้นอยู่แล้ว kill ทิ้ง |
| `dotnet ef` ฟ้องว่าไม่รู้จักคำสั่ง | ยังไม่ได้ติดตั้ง dotnet-ef global tool — ดูข้อ 1.4 |
| build/run ผ่านปกติตอนใช้ `dotnet run` แต่พอรัน DLL ที่ build ไว้ตรง ๆ (`dotnet path/to/API.dll`) กลับ error "Please config identity authority" | `dotnet run` ใช้ profile จาก `launchSettings.json` ซึ่งตั้ง `ASPNETCORE_ENVIRONMENT=Development` ให้อัตโนมัติ แต่รัน DLL ตรง ๆ จะ default เป็น `Production` แทน ซึ่งต้องการค่า `Identity:Authority` ที่ไม่ได้ตั้งไว้ในเครื่อง dev — ให้ตั้ง `ASPNETCORE_ENVIRONMENT=Development` เองก่อนรัน หรือใช้ `dotnet run`/VS Code debug config (ข้อ 3.1) แทน |
| ไฟล์ที่แก้บน Windows แล้ว diff ใน Git ขึ้นทั้งไฟล์ทั้งที่แก้แค่บรรทัดเดียว | ปัญหา line ending (CRLF บน Windows vs LF ที่ repo ใช้) — ใน git เก็บเป็น LF ไฟล์ใหม่ให้บันทึกเป็น UTF-8 ไม่มี BOM แต่ไฟล์เก่าบางส่วนยังมี BOM อยู่ ไม่ต้องแก้ย้อนหลัง ที่ root ของ repo ยังไม่มีไฟล์ `.gitattributes` (มีแค่ `src/vuewebui/.gitattributes` ที่มากับ template หน้าเว็บ ซึ่งมีผลเฉพาะไฟล์ในโฟลเดอร์นั้น) จึงต้องเช็คเองว่า `git config core.autocrlf` ตั้งเป็น `true` (Windows) หรือ `input` (macOS/Linux) ไว้แล้วหรือยัง (`.editorconfig` ที่ root กำหนด `end_of_line = lf` ไว้เฉพาะไฟล์ `.cs`) |
| ไม่แน่ใจว่าเปิด `cpa.sln` หรือ `.slnx` ใน Visual Studio | ทั้งสองไฟล์เปิดโปรเจกต์เดียวกัน — `.slnx` เป็นฟอร์แมตใหม่ (XML แทน text format เดิม) ของ Visual Studio ใช้ตัวไหนก็ได้ แต่คำสั่ง `dotnet build`/`dotnet test` ในคู่มือนี้อ้างอิง `.sln` เป็นหลัก |

หาไม่เจอในตารางนี้ ให้เช็ค [known-issues.md](known-issues.md) ก่อนว่าเคยมีคนเจอปัญหาเดียวกันมาก่อนหรือเปล่า แล้วค่อยถามทีม
