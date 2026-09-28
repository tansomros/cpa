# Domain Glossary & Workflow

Shared vocabulary for this project — use these terms consistently in code (entity/property names), UI copy, and commit messages, so a Thai procurement term always maps to the same English identifier. Where a detail is marked "not final," it needs stakeholder confirmation before being relied on for implementation — see the Open Questions section of [roadmap.md](roadmap.md).

## Core entities today

- **Contract (สัญญา)** — a formal agreement with a Vendor, either project-based or a period contract that Purchase Orders can be called off against. Tracks TOR/procurement/inspection committee appointments, collateral, budget, bid winner. See `Contract` entity.
- **Vendor (บริษัทคู่ค้า/ผู้ค้า)** — a supplier/company the hospital contracts or purchases with.
- **Item / Catalog (สินค้า/บริการ)** — the thing being contracted, requisitioned, ordered, or stocked: physical goods (medical supplies, drugs, equipment) or services. Modeled as a single `Item` entity with an `ItemCategory` discriminator and a JSONB `Attributes` column for category-specific fields, not one class per category (see [architecture.md](architecture.md)).
- **UnitOfMeasure (หน่วยนับ)** — how an Item's quantity is expressed (piece, box, ml, etc.).
- **Reference data** — the organizational and physical hierarchy: Building/Floor/Location, Department/Division/ServiceGroup, and the Thai address hierarchy Province/District/SubDistrict. `Location` is a physical place in this hierarchy (a building/floor/department) — it has no relationship to any specific Contract; don't reintroduce one (see roadmap.md Phase 5 for why this needed fixing). `ServiceGroup` (กลุ่มงาน, e.g. "กลุ่มงานพยาบาล"/"กลุ่มงานการแพทย์") is the top level above `Division` (ฝ่าย) above `Department` (แผนก) — was named `Sector` until 2026-08-18; renamed since "Sector" didn't read naturally in English for a Thai hospital service-group grouping.
- **Warehouse (คลัง/สถานที่จัดเก็บ)** — a physical stock storage point (e.g. "เภสัชกรรม 1", "คลังกลาง"), distinct from `Location`. A single Item's stock can be split across multiple Warehouses at once (e.g. one contracted pack of 10 boxes received as 5+5 into two different warehouses) — this is exactly why Warehouse isn't just a field on `ContractItem` or `Location`.

## Planned procurement workflow (PPR → PR → PO)

Confirmed with the project owner. Sequence:

```
ProcurementPlan (PPR)  →  PurchaseRequisition (PR)  →  PurchaseOrder (PO)  →  GoodsReceipt
   แผนจัดซื้อจัดจ้าง          ใบขอซื้อ/ขอจ้าง              ใบสั่งซื้อ/สั่งจ้าง          ใบตรวจรับพัสดุ
```

Contract sits alongside/after PO — a PO can either be ad-hoc or a call-off order against an existing period Contract.

- **ProcurementPlan (PPR)** — the annual/period procurement plan, budget-linked, department-scoped, listing planned items/categories and budgeted amounts. States (proposed, not final): `Draft → Approved → Active → Closed`.
- **PurchaseRequisition (PR)** — raised by a requesting department, optionally against a `ProcurementPlan` line, listing the Items and quantities needed. States (proposed, not final): `Requested → Approved (dept head) → Approved (procurement) → Rejected`.
- **PurchaseOrder (PO)** — raised by procurement against approved PR line(s), issued to one Vendor, referencing a Contract when one applies. States (proposed, not final): `Draft → Sent → Acknowledged → PartiallyReceived → Received → Closed/Cancelled`.
- **GoodsReceipt** — records receipt of goods/services against a PO; this is the event that will eventually update Stock (see below).

**Budget tracking**: `ProcurementPlan` budget-remaining is derived from an append-only ledger of budget events (commit on PR approval, consume on PO/GoodsReceipt, release on cancellation) — never a single mutable balance field. `Contract.SumPOAmount` (currently a manually-maintained decimal) should become a derived value once real POs exist and link to a Contract.

**Not yet designed** (needs a dedicated domain session with procurement/finance stakeholders before implementation):
- Exact approval / delegation-of-authority limits per role and PR/PO value.
- Precise budget-checking rules against `ProcurementPlan` (hard stop vs. warn-and-override, who can override).
- Integration contract with the existing legacy system / HOSXP — what data flows in which direction, real-time or batch.

## Inventory/stock workflow

Foundation landed (roadmap.md Phase 5, pulled forward early) — the full workflow (GoodsReceipt-triggered receiving, issue/transfer/adjustment UI, lot/batch/expiry) is still pending a dedicated design session, but the core ledger model is real and working today.

- **Warehouse (คลัง/สถานที่จัดเก็บ)** — see above.
- **StockItem** — Item × Warehouse × on-hand quantity, materialized from movements (never a directly-edited counter) — mutated only via `StockItem.ApplyMovement(StockMovement)`.
- **StockMovement** — Receipt, Issue, Transfer, Adjustment; append-only ledger, on-hand quantity is always derived from summing this ledger (`SUM(Quantity)` per Item × Warehouse). A `Transfer` is two linked rows (out of the source, into the destination — correlated by `TransferGroupId`), not one row with from/to columns, so reconciliation stays a plain sum. Only `Receipt` has an Application-layer command today (`ReceiveStockCommand`); `Issue`/`Transfer`/`Adjustment` exist as domain factory methods but have no command/endpoint yet.
- **Traceability**: a `StockMovement` can optionally reference the `ContractItem` it was received against, so "where did this stock come from" is always answerable — this is what a `GoodsReceipt` entity will eventually formalize (Phase 4).
- **Open question**: whether lot/batch/expiry tracking is required — very likely yes given drugs are in scope, but not decided.

## External systems

- **HOSXP** — an existing Thai Hospital Information System the frontend already has a separate API client configuration for (`$hosxpapi` / `VITE_HOSXP_API_BASE_URL`). Exact integration contract (what data flows which direction) is an open question — see [roadmap.md](roadmap.md).
- **Legacy inventory system** — the system this project is intended to eventually replace. No migration/cutover plan exists yet.

## Notes on terminology

- "PPR" in this project specifically means Procurement Plan (แผนจัดซื้อจัดจ้าง), not to be confused with other Thai government-procurement acronyms (e.g. ปร.4/ปร.5 construction cost estimate forms) that mean something unrelated — don't assume PPR means the same thing in a different Thai government-procurement context you may have seen elsewhere.
- Keep entity/property names in English (matching the rest of the codebase's convention), but keep this glossary and UI-facing labels bilingual so the mapping between the Thai business term and the English identifier stays discoverable.
