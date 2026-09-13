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
