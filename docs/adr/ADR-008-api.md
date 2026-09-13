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