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
