แต่ผมขอออกตัวก่อนนิดหนึ่งว่า **ผมจะไม่คัดลอก Clean Architecture ของ Microsoft หรือของ Jason Taylor มาใช้ตรง ๆ** เพราะผมคิดว่า **BigLion ต้องเหมาะกับคุณ** ซึ่งมีลักษณะการทำงานคือ

* พัฒนาเองเป็นหลัก (หรือทีมเล็ก 2-5 คน)
* ทำหลายระบบ
* ต้องดูแลระบบหลายปี
* ต้องส่งงานได้เร็ว
* ไม่ต้องการ Architecture ที่ "สวยแต่ใช้ยาก"

ดังนั้น BigLion จะเป็น **Pragmatic Clean Architecture** (Clean Architecture ที่เน้นใช้งานจริง)

---

# 🦁 BigLion-001 : Architecture Decision Record (ADR)

Version : **0.1**

Status : Accepted

---

# ADR-001

## Architecture Style

## Decision

BigLion จะใช้

> **Pragmatic Clean Architecture**

ไม่ใช่ Clean Architecture แบบเต็มรูปแบบ

---

## Layer

```
Presentation

↓

Application

↓

Domain

↓

Infrastructure
```

มีเพียง 4 Layer

---

## Dependency

```
Presentation

↓

Application

↓

Domain

↑

Infrastructure
```

Application ไม่รู้จัก Database

Domain ไม่รู้จัก EF

Domain ไม่รู้จัก Dapper

Domain ไม่รู้จัก ASP.NET

---

# เหตุผล

เพราะ

* อ่านง่าย
* Debug ง่าย
* เหมาะกับทีมเล็ก
* เหมาะกับระบบ Enterprise

---

# สิ่งที่ BigLion จะไม่ใช้

❌ CQRS

❌ MediatR

❌ Event Sourcing

❌ Generic Repository

❌ Generic Service

❌ Unit Of Work (ในรูปแบบที่สร้างขึ้นเอง)

❌ Repository<T>

---

เหตุผล

เพราะ

ระบบส่วนใหญ่ของคุณคือ

```
CRUD

Search

Workflow

Report
```

ไม่ใช่

```
Bank

Stock Exchange

IoT

Microservice
```

ดังนั้นไม่จำเป็น

---

# ADR-002

## Project Structure

Decision

```
BigLion.sln

src

    BigLion.Api

    BigLion.Application

    BigLion.Domain

    BigLion.Infrastructure

web

    BigLion.Web

database

tests

docs
```

---

## เหตุผล

แยก Frontend

ออกจาก Backend

อย่างชัดเจน

ไม่ใช้

```
wwwroot

Vue

อยู่ใน ASP.NET
```

เพราะ

Vue ควร Build แยก

Deploy แยก

Version แยก

---

# ADR-003

## Architecture Pattern

Decision

```
Controller

↓

Application Service

↓

Repository

↓

Database
```

---

ไม่ใช้

```
Controller

↓

Mediator

↓

Handler

↓

Command

↓

Repository

↓

Specification

↓

UnitOfWork

↓

Factory

↓

Database
```

---

เหตุผล

เปิด Debug แล้ว

กด F11

3 ครั้ง

ถึง Database

ไม่ใช่ 15 ครั้ง

😄

---

# ADR-004

## Business Logic

Business Logic

ทั้งหมด

อยู่ใน

```
Application
```

เช่น

```
CalculateScore()

Approve()

Finalize()

Validate()
```

---

ไม่อยู่

Controller

---

ไม่อยู่

Repository

---

ไม่อยู่

Database

---

# ADR-005

## Repository

Repository

รับผิดชอบ

เฉพาะ

```
Database
```

เช่น

```
EmployeeRepository

EvaluationRepository

DepartmentRepository
```

---

Repository

ห้าม

```
Calculate()

Approve()

Validate()
```

เด็ดขาด

---

# ADR-006

## ORM Strategy

Decision

ใช้

```
EF Core

+

Dapper
```

---

EF Core

ใช้

```
CRUD
```

---

Dapper

ใช้

```
Search

Report

Dashboard

Complex Query
```

---

เหตุผล

ใช้เครื่องมือให้เหมาะกับงาน

---

# ADR-007

## Database

Decision

ใช้

```
PostgreSQL
```

Version

ล่าสุดที่เป็น LTS/Stable ในช่วงเริ่มโครงการ

---

Naming

```
snake_case
```

ทั้งหมด

---

Table

```
employee

department

evaluation
```

---

Column

```
employee_id

created_date

updated_date
```

---

Primary Key

```
GENERATED ALWAYS AS IDENTITY
```

---

# ADR-008

## API Style

Decision

REST API

เท่านั้น

เช่น

```
GET

/api/employees
```

```
GET

/api/employees/15
```

```
POST

/api/employees
```

```
PUT

/api/employees/15
```

```
DELETE

/api/employees/15
```

---

Response

Standard

```json
{
  "success": true,
  "message": "",
  "data": {}
}
```

---

# ADR-009

## Frontend

Decision

```
Vue 3

Composition API

TypeScript

Pinia

PrimeVue
```

---

เหตุผล

ง่าย

เร็ว

Community ใหญ่

เหมาะกับ .NET

---

# ADR-010

## Authentication

Decision

JWT

*

Refresh Token

---

Authorization

Role

Permission

---

Authentication

เป็น Module

ไม่ผูกกับ Business

---

# ADR-011

## Logging

Decision

```
Serilog
```

ทุก API

ทุก Error

ทุก Exception

Audit แยกจาก Log

---

# ADR-012

## Error Handling

ทุก Exception

ผ่าน

Global Exception Middleware

Controller

ไม่จับ Exception เอง

เช่น

```csharp
try
{

}
catch
{

}
```

จะไม่เขียนใน Controller

---

# ADR-013

## Validation

Backend

```
FluentValidation
```

Frontend

```
VeeValidate
```

Validation Rule

อยู่ที่ Backend

เสมอ

Frontend

มีไว้เพื่อ UX

---

# ADR-014

## Coding Principle

BigLion

ยึดหลัก

### KISS

Keep It Simple

---

### SOLID

เฉพาะส่วนที่จำเป็น

---

### DRY

Don't Repeat Yourself

---

### YAGNI

You Aren't Gonna Need It

---

จะไม่เขียนเผื่อ

Feature

ในอนาคต

---

# ADR-015

## BigLion Philosophy

นี่คือข้อสุดท้าย

และเป็นข้อที่สำคัญที่สุด

BigLion

จะไม่ไล่ตาม Trend

BigLion

จะเลือก

Technology

ที่

> **ดูแลรักษาได้ง่ายที่สุดในอีก 10 ปี**

มากกว่า

Technology

ที่

"กำลังดัง"

---

# ผมขอเพิ่ม ADR-016 (อันนี้ผมคิดขึ้นระหว่างคุยกับคุณ)

ผมอยากให้ BigLion มีหลักการอีกข้อหนึ่งที่ผมเรียกว่า **"Business First"**

หลาย Framework เริ่มจาก Database หรือ UI แต่สำหรับระบบที่คุณทำ (ระบบร้านยา และงานบริการ MTM ของร้านยา) สิ่งที่เปลี่ยนบ่อยที่สุดคือ **กฎธุรกิจ**

ดังนั้นทุกครั้งที่เริ่มโมดูลใหม่ เราจะทำตามลำดับนี้

```
Business Rules
      ↓
Domain Model
      ↓
API Contract
      ↓
Database
      ↓
Frontend
```

ไม่ใช่

```
Database
      ↓
API
      ↓
Frontend
```

เพราะเมื่อกฎธุรกิจเปลี่ยน เราจะปรับได้ง่ายกว่า และโครงสร้างจะสะท้อนสิ่งที่องค์กรทำจริง ๆ ไม่ใช่แค่สิ่งที่หน้าจอแสดง

---

## สิ่งที่ผมอยากชวนคุณทำเป็น "กติกา" ของ BigLion

ผมอยากให้ทุก ADR ของ BigLion เป็นเอกสารที่ **อธิบายเหตุผล** ไม่ใช่แค่บอกว่าจะใช้เทคโนโลยีอะไร

เช่น

* "ทำไมเราไม่ใช้ Generic Repository"
* "ทำไมเราใช้ Dapper คู่กับ EF Core"
* "ทำไมเราไม่ใช้ CQRS"

อีก 5 ปีข้างหน้า เมื่อคุณหรือทีมกลับมาอ่าน จะเข้าใจว่าการตัดสินใจแต่ละครั้งมีเหตุผลอะไร ไม่ใช่แค่เห็นผลลัพธ์แต่ไม่รู้ที่มา

---

## และสุดท้าย... ผมมีข้อเสนอหนึ่งที่ผมคิดว่าจะทำให้ BigLion แตกต่าง

ผมอยากให้เราสร้าง BigLion **เหมือนผลิตภัณฑ์จริง** โดยใน Repository จะมี

```
docs/
├── adr/
│   ├── ADR-001-architecture.md
│   ├── ADR-002-project-structure.md
│   ├── ADR-003-orm-strategy.md
│   └── ...
├── architecture/
├── api/
├── database/
└── coding-standards/
```

ทุกการตัดสินใจจะถูกบันทึกไว้เป็นเอกสาร ไม่ใช่อยู่แค่ในความทรงจำของเรา

ผมเชื่อว่าถ้าเราทำแบบนี้ BigLion จะไม่ใช่แค่ Framework สำหรับโปรเจ็กต์นี้ แต่จะเป็น **Platform ที่คุณสามารถพัฒนาต่อยอดได้อีกเป็นสิบปี** และผมยินดีช่วยคุณออกแบบมันไปทีละ Milestone ครับ 🦁

---

# ADR-017

## Lookup ข้อมูลพฤติกรรมสุขภาพของผู้ป่วยด้วย SmartEnum

Status : Accepted

Date : 2026-10-08

ผู้ตัดสินใจ : Teerapol

ADR ที่เกี่ยวข้อง : ADR-013 (Validation), ADR-014 (YAGNI)

---

## บริบท

ข้อมูลผู้ป่วย (`Patient`) มีช่องพฤติกรรมสุขภาพ 4 ช่องที่ต้องเลือกจากรายการตัวเลือก

| ช่องใน `Patient` | ชื่อใน Command / ViewModel | ความหมาย | ชนิดตอนนี้ |
|---|---|---|---|
| `Smoke` | `Smoke` | การสูบบุหรี่ | `int?` |
| `CigaretteType` | `CigaretteType` | ชนิดของบุหรี่ที่สูบ | `int?` |
| `Drinking` | `Alcohol` | การดื่มเครื่องดื่มแอลกอฮอล์ | `int?` |
| `DrinkFrequency` | `AlcoholFQ` | ความถี่ในการดื่ม | `int?` |

ตอนนี้ทั้ง 4 ช่องเป็นตัวเลขที่ไม่มีความหมายกำกับ หน้าเว็บ (`src/vuewebui/src/pages/patients/list/index.vue`) ให้พิมพ์ตัวเลขเองในช่อง `type="number"` และ API ยังไม่มีกฎตรวจค่าของช่องเหล่านี้

ระบบเดิมที่ CPA คัดลอกมาเก็บตัวเลือกแบบนี้ไว้ในตาราง `ReferenceGroup` / `ReferenceValue` ซึ่งถูกลบออกไปแล้วตอนล้างโค้ดที่เหลือจากระบบ Checkup

ในโค้ดมีกลไก lookup แบบ SmartEnum อยู่แล้ว

* `SmartEnum<T>` (`src/Domain/Common/SmartEnum.cs`) แต่ละค่ามี `Value` (รหัส), `Code`, `Name` (ชื่อภาษาไทยที่แสดง) และ `Sort` (ลำดับ) โดย `All` คืนทุกค่าเรียงตาม `Sort`, `FromValue` / `TryFromValue` แปลงรหัสกลับเป็นค่า SmartEnum และ `GetDisplayName(lang)` อ่านชื่อจากไฟล์ `.resx` ถ้ามี ถ้าไม่มีจะใช้ `Name` (ตอนนี้ยังไม่มีไฟล์ `.resx` จึงได้ `Name` เสมอ)
* `LookupRegistry` (`src/Application/Features/Lookups/LookupRegistry.cs`) เป็นที่ลงทะเบียน category ของ lookup โดยแต่ละ category ชี้ไปที่ SmartEnum หนึ่งตัว แล้วแปลงเป็น `LookupOptionDto(Value, DisplayName)`
* `GetLookupOptionsQuery` ดึงตัวเลือกของ category จาก `LookupRegistry` (ถ้าไม่พบ category จะ throw `NotFoundException`) ส่วน `GetLookupCategoriesQuery` คืนรายชื่อ category ทั้งหมด ทั้งสองตัวเรียกได้โดยไม่ต้อง login (`CpaPolicies.AllowAnonymous`)
* `OptionsController` มี `GET /Options` (รายชื่อ category) และ `GET /Options/{category}?lang=` (ตัวเลือกของ category นั้น)
* ฝั่งหน้าเว็บมี `useLookupStore` (`src/vuewebui/src/stores/useLookupStore.js`) ที่เรียก `/Options/{category}` และ cache ผลไว้

และ commit `7a0947b` (update enums) เพิ่ม `SmokingValue` และ `DrinkingValue` ไว้ใน `src/Domain/Enums/` แล้ว

---

## Decision

ใช้ **SmartEnum** เป็นรายการตัวเลือกของทั้ง 4 ช่อง

| ช่อง | SmartEnum |
|---|---|
| การสูบบุหรี่ (`Smoke`) | `SmokingValue` (มีแล้ว) |
| ชนิดบุหรี่ (`CigaretteType`) | SmartEnum ตัวใหม่ |
| การดื่ม (`Drinking`) | `DrinkingValue` (มีแล้ว) |
| ความถี่ในการดื่ม (`DrinkFrequency`) | SmartEnum ตัวใหม่ |

ไม่กลับไปใช้ตาราง `ReferenceGroup` / `ReferenceValue` ในฐานข้อมูล

ทั้ง 4 ตัวลงทะเบียนใน `LookupRegistry` และหน้าเว็บดึงตัวเลือกผ่าน `GET /Options/{category}` เหมือนกันทั้งหมด

ชนิดบุหรี่มีตัวเลือก "อื่นๆ" ถ้าเลือกข้อนี้ให้พิมพ์รายละเอียดในช่อง `SmokingRemark`

รายการรหัสจริงของแต่ละช่องยังรอยืนยัน ดูหัวข้อ "รายละเอียดที่รอยืนยัน" ด้านล่าง

---

## เหตุผล

* **ตัวเลือกแทบไม่เปลี่ยน** สถานะการสูบ ชนิดบุหรี่ การดื่ม และความถี่ในการดื่ม เป็นรายการที่นิ่ง ไม่จำเป็นต้องมีหน้าจอให้แก้
* **ตัวเลือกผูกกับกฎและรายงาน** ค่าเหล่านี้ใช้เป็นเงื่อนไขใน validation และใช้นับในรายงาน ถ้าเก็บในตารางที่แก้หรือลบได้ การแก้ข้อมูลในฐานข้อมูลอาจทำให้รายงานย้อนหลังผิด และทำให้เงื่อนไขในโค้ดพังเงียบ ๆ โดย compiler ไม่เตือน ส่วน SmartEnum ถ้าเปลี่ยนชื่อหรือลบค่า compiler จะแจ้งทุกจุดที่ใช้
* **ใช้กลไกที่มีอยู่แล้ว** ใช้ `LookupRegistry` และ `/Options/{category}` ที่มีอยู่ ไม่ต้องออกแบบตาราง หน้าจอจัดการ หรือ API ใหม่
* **ชื่อภาษาไทยอยู่ที่เดียว** ชื่อที่แสดงอยู่ในโค้ด ไม่ขึ้นกับข้อมูลในฐานข้อมูล ข้อนี้สำคัญตอนนี้ที่งานทำกันหลายเครื่อง และข้อมูลในฐานข้อมูลของแต่ละเครื่องไม่เหมือนกัน
* **ทดสอบง่าย** เขียน test ตัวเดียวไล่ทุก category ที่ลงทะเบียนไว้ได้ ว่ารหัส (`Value`) และ `Sort` ไม่ซ้ำ และทุกค่ามีชื่อภาษาไทย

---

## การเก็บค่าในฐานข้อมูล

คอลัมน์เก็บ **รหัสที่เป็นข้อความและไม่เปลี่ยน** คือ `Value` ของ SmartEnum (เช่น `Non`, `Quit`)

ไม่เก็บ

* ข้อความภาษาไทย (เช่น "ไม่สูบ") เพราะชื่อที่แสดงปรับคำได้
* ตัวเลข เพราะเปิดดูข้อมูลในฐานข้อมูลแล้วไม่รู้ความหมาย

ดังนั้นชื่อภาษาไทย (`Name`) และลำดับการแสดง (`Sort`) แก้ได้ตลอดโดยไม่กระทบข้อมูลที่บันทึกไว้แล้ว สิ่งที่ห้ามเปลี่ยนหลังมีข้อมูลจริงคือ `Value`

---

## ถ้าวันหนึ่งต้องให้ Admin แก้ตัวเลือกได้

ถ้ามี category ไหนที่ต้องให้ Admin เพิ่มหรือแก้ตัวเลือกเองจริง ๆ ให้เปลี่ยนเฉพาะ category นั้นให้ `LookupRegistry` อ่านจากตาราง ส่วน category อื่นยังเป็น SmartEnum เหมือนเดิม

หน้าเว็บยังเรียก `GET /Options/{category}` เหมือนเดิม ไม่ต้องแก้

ตอนนี้ยังไม่ทำ (YAGNI ตาม ADR-014)

---

## ผลที่ตามมา

* คอลัมน์ `Smoke`, `CigaretteType`, `Drinking` และ `DrinkFrequency` ในตาราง `Patients` เปลี่ยนจาก `int?` เป็นข้อความ รวมถึง property ที่เกี่ยวข้องใน `Patient`, Command, ViewModel และหน้าเว็บ
* migration ของการเปลี่ยนนี้ Teerapol สร้างเอง ทีม AI ไม่สร้าง migration
* หน้าเว็บเปลี่ยนจากช่องพิมพ์ตัวเลขเป็นตัวเลือกที่ดึงจาก `/Options/{category}`
* การเพิ่มหรือลบตัวเลือกต้องแก้โค้ดและ deploy ใหม่ ซึ่งยอมรับได้เพราะตัวเลือกแทบไม่เปลี่ยน
* ค่าตัวเลขที่มีอยู่แล้วในฐานข้อมูลแปลงเป็นรหัสใหม่ไม่ได้ ซึ่งไม่เป็นปัญหา เพราะ Teerapol จะสร้างข้อมูลใหม่อยู่แล้ว

---

## สิ่งที่โค้ดใน master ยังไม่ตรงกับ ADR นี้ (ณ commit `7a0947b`)

**`DrinkingValue` มีรหัสซ้ำ (bug)**

| field | `Value` | `Code` | `Name` | `Sort` |
|---|---|---|---|---|
| `Non` | `Non` | `N` | ไม่ดื่ม | 0 |
| `Quit` | `Quit` | `Y` | เคยดื่มแต่เลิกแล้ว | 1 |
| `Occasionally` | `Quit` | `Q` | ดื่มครั้งคราว | 2 |
| `Regularly` | `Quit` | `Q` | ดื่มประจำ | 2 |

สามค่าใช้ `Value` = `Quit` ร่วมกัน และสองค่าใช้ `Sort` = 2 ร่วมกัน ผลคือ

* `FromValue` / `TryFromValue` ของ `DrinkingValue` จะ error ทุกครั้ง เพราะ dictionary ที่สร้างจาก `Value` มี key ซ้ำ (ส่วน `All` ยังคืนครบ 4 ค่า)
* `Equals` มองว่า 3 ค่านี้เป็นค่าเดียวกัน เพราะเทียบจาก `Value`
* ถ้าบันทึกลงฐานข้อมูล จะแยกไม่ออกว่าเป็น "เคยดื่มแต่เลิกแล้ว", "ดื่มครั้งคราว" หรือ "ดื่มประจำ"

ต้องแก้ก่อนนำไปใช้

**เรื่องอื่นที่ยังต้องทำ**

* `SmokingValue` มี 3 ค่า คือ `Non` (ไม่สูบ), `Regularly` ที่ใช้ `Value` = `Yes` (สูบประจำ) และ `Quit` (เลิกสูบแล้ว)
* ยังไม่มี SmartEnum ของชนิดบุหรี่และความถี่ในการดื่ม
* `LookupRegistry` ยังว่าง ยังไม่มี category ไหนลงทะเบียน (มีแค่ comment ตัวอย่าง `smoking-statuses`)
* `OptionsController` ยังไม่ถูก compile เพราะ `src/API/API.csproj` มี `<Compile Remove="Controllers\OptionsController.cs" />` และไฟล์ยังใช้ namespace เก่า `Kondongpu.*` ตอนนี้ API จึงยังไม่มี `/Options` ต้องแก้ namespace และเอาบรรทัด `Compile Remove` ออกก่อน
* `SmartEnum<T>` ค้นรหัสแบบไม่สนตัวพิมพ์เล็กใหญ่ (`StringComparer.OrdinalIgnoreCase`) เช่น `TryFromValue("regular")` จะหาเจอ
* ทั้ง 4 ช่องใน `Patient` ยังเป็น `int?` และ `PatientWriteRules` ยังไม่มีกฎตรวจช่องเหล่านี้
* constructor ของ `SmokingValue` และ `DrinkingValue` ตั้งชื่อ parameter ตัวที่สองว่า `abnormalFlag` (ชื่อที่ติดมาจากระบบ Checkup) แต่ค่าที่ส่งเข้าไปคือ `Code` ของ base class
* entity `MTM` มีช่องชื่อคล้ายกันที่ยังเป็น `int?` ดูข้อเสนอในข้อ 6 ของหัวข้อ "รายละเอียดที่รอยืนยัน"

---

## รายละเอียดที่รอยืนยัน

> Status : Proposed
>
> หัวข้อนี้ยังไม่ใช่ข้อตกลง ข้อ 1–7 รอ Teerapol ยืนยัน ตอบกลับเป็นหมายเลขข้อได้เลย (ข้อ 7 ควรตอบก่อนข้อ 2)

### 1. รายการรหัสและชื่อภาษาไทย (ร่างจาก BA)

รหัสใช้ถาวร เปลี่ยนไม่ได้หลังมีข้อมูลจริง ส่วนชื่อภาษาไทยแก้คำได้ภายหลัง

การสูบบุหรี่ (`Smoke`)

| รหัส | ชื่อที่แสดง |
|---|---|
| `Non` | ไม่สูบ |
| `Regular` | สูบประจำ |
| `Quit` | เลิกสูบแล้ว |

ชนิดบุหรี่ (`CigaretteType`)

| รหัส | ชื่อที่แสดง |
|---|---|
| `Manufactured` | บุหรี่ซอง |
| `RollYourOwn` | บุหรี่มวนเอง/ยาเส้น |
| `Electronic` | บุหรี่ไฟฟ้า |
| `Other` | อื่นๆ (รายละเอียดใน `SmokingRemark`) |

การดื่ม (`Drinking`)

| รหัส | ชื่อที่แสดง |
|---|---|
| `Non` | ไม่ดื่ม |
| `Quit` | เคยดื่มแต่เลิกแล้ว |
| `Occasional` | ดื่มครั้งคราว |
| `Regular` | ดื่มประจำ |

ความถี่ในการดื่ม (`DrinkFrequency`)

| รหัส | ชื่อที่แสดง |
|---|---|
| `LessThanMonthly` | น้อยกว่าเดือนละครั้ง |
| `Monthly` | 1–3 ครั้งต่อเดือน |
| `Weekly` | 1–4 ครั้งต่อสัปดาห์ |
| `Daily` | เกือบทุกวันหรือทุกวัน |

### 2. เลิกใช้ `IsSmoke` และกำหนดการใช้ `SmokingQuit`

* `IsSmoke` ความหมายซ้ำกับ `Smoke` จึงเลิกใช้ และดูจาก `Smoke` อย่างเดียว
* `SmokingQuit` ขึ้นกับคำตอบข้อ 7 (ข้อเสนอที่ BA ปรับใหม่)
  * ถ้าหมายถึง "อยากลดหรือเลิกสูบบุหรี่" (ตาม comment ใน `Patient`) ให้ **เก็บไว้** ถามเฉพาะเมื่อ `Smoke` = `Regular` และเปลี่ยน label บนหน้าเว็บเป็น "อยากลดหรือเลิกสูบบุหรี่หรือไม่"
  * ถ้าหมายถึง "เลิกสูบแล้ว" ให้เลิกใช้ และใช้ `Smoke` = `Quit` แทน

### 3. รหัสสูบประจำและการเก็บค่า

* ใช้รหัส `Regular` แทน `Yes` ใน `SmokingValue` (เปลี่ยนตอนนี้ ก่อนมีข้อมูลจริง)
* เก็บเฉพาะ `Value` ในคอลัมน์ `varchar(20)`
* เอา `Code` ออกถ้าไม่มีที่ใช้ (ตอนนี้ `LookupRegistry` และ `LookupOptionDto` ใช้แค่ `Value` กับชื่อที่แสดง ไม่ได้ใช้ `Code`)

### 4. กฎข้ามช่อง

ใช้ทั้งใน validator ของ API และหน้าเว็บ (ตาม ADR-013: กฎหลักอยู่ที่ Backend ส่วนหน้าเว็บมีไว้เพื่อ UX)

* เลือกชนิดบุหรี่ได้เฉพาะเมื่อ `Smoke` เป็น `Regular` หรือ `Quit`
* เลือกความถี่ในการดื่มได้เฉพาะเมื่อ `Drinking` เป็น `Occasional` หรือ `Regular`
* เลือก `Non` แล้วให้ล้างช่องที่เกี่ยวข้อง
* `SmokeYear` และ `SmokeCigarette` ยังเป็นตัวเลขเหมือนเดิม

### 5. API ปฏิเสธรหัสที่ไม่มีใน SmartEnum

validator ของ API ไม่รับรหัสที่ไม่ได้กำหนดไว้ใน SmartEnum เช่น `Yes` แบบเดิม ตัวพิมพ์ไม่ตรงอย่าง `regular` หรือพิมพ์ผิด

`TryFromValue` ของ `SmartEnum<T>` ไม่สนตัวพิมพ์เล็กใหญ่ จึงให้ตรวจตัวพิมพ์แบบตรงตัวใน validator ของ `Patient` **ไม่แก้ `SmartEnum<T>`** เพราะเป็น base class ที่ใช้ร่วมกัน ถ้าแก้จะกระทบส่วนอื่น

### 6. ช่องของ `MTM`

entity `MTM` มีช่องของตัวเองที่ยังเป็น `int?` คือ `Smoke`, `CigaretteType`, `Alcohol` และ `AlcoholFQ` ทีมแนะนำให้เปลี่ยนเป็นรหัสข้อความชุดเดียวกันใน migration เดียวกัน เพื่อให้รายงานเทียบข้อมูลของ `Patient` กับ `MTM` ได้

### 7. ความหมายของ `SmokingQuit`

ขอให้ Teerapol ยืนยันว่า `SmokingQuit` หมายถึง "เลิกสูบแล้ว" หรือ "อยากลดหรือเลิกสูบบุหรี่" เพราะ comment ใน `Patient` เขียนว่า "อยากจะลดหรือเลิกสูบบุหรี่หรือไม่" แต่หน้าเว็บติด label ว่า "เลิกบุหรี่แล้ว"

ข้อนี้ต้องตอบก่อนข้อ 2

### 8. ชื่อที่ยังไม่ได้กำหนด (รายละเอียดทางเทคนิคสำหรับผู้พัฒนา ไม่ต้องรอ Teerapol ตัดสินใจ)

ชื่อ SmartEnum ตัวใหม่ (ชนิดบุหรี่ ความถี่ในการดื่ม) และชื่อ category ที่จะลงทะเบียนใน `LookupRegistry` ยังไม่ได้กำหนด

### 9. comment ของ `DrinkFrequency` (รายละเอียดทางเทคนิคสำหรับผู้พัฒนา ไม่ต้องรอ Teerapol ตัดสินใจ)

comment ของ `DrinkFrequency` ใน `Patient` ตอนนี้เขียนว่า "ความถี่ในการดื่ม ครั้ง/สัปดาห์" ซึ่งเป็นแบบตัวเลข ต้องปรับตามเมื่อเปลี่ยนเป็นรหัส
