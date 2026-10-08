# Domain Glossary & Workflow

Shared vocabulary for this project — use these terms consistently in code (entity/property names), UI copy, and commit messages, so a Thai business term always maps to the same English identifier. Where a detail is marked "not final," it needs stakeholder confirmation before being relied on for implementation.

## Core entities today

- **Pharmacy (ร้านขายยา)** — เป็นข้อมูลร้านขายยา ซึ่งเป็นผู้ใช้งานระบบหลัก เป็นคนบันทึกข้อมูลและให้บริการต่างๆแก่ผู้รับบริการ ข้อมูลก็จะประกอบไปด้วย รหัส ชื่อร้านยา ที่ตั้ง เป็นต้น. See `Pharmacy` entity.
- **Patient (ผู้ป่วย/ผู้รับบริการ)** — a patient/customer the pharmacy service. เป็นข้อมูลผู้ป่วย/ผู้มารับบริการที่ร้านขายยา. See `Patient` entity.
- **Service (รายการให้บริการ)** — เป็นรายการ transaction การให้บริการแก่ผู้รับบริการ ว่าให้บริการเรื่องอะไร มีรายละเอียดอะไรบ้าง.

