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

หลาย Framework เริ่มจาก Database หรือ UI แต่สำหรับระบบที่คุณทำ (HIS, ตรวจสุขภาพ, ประเมินผลงาน) สิ่งที่เปลี่ยนบ่อยที่สุดคือ **กฎธุรกิจ**

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
