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
