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
