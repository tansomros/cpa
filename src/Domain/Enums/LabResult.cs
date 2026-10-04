using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Domain.Enums;

/// <summary>
/// ผลตรวจแลป (Lab) — แทนที่ ReferenceGroup "LAB" (Id=2) + ReferenceValue ในฐานข้อมูล
///
/// ข้อดีของ SmartEnum เทียบกับ ReferenceValue:
/// ─────────────────────────────────────────────
/// 1. Type-safe: LabResult.Normal แทน magic string "Normal"
/// 2. IntelliSense: IDE แนะนำค่าที่ใช้ได้ทั้งหมด
/// 3. ไม่ต้อง query DB: LabResult.All ดึงค่าจาก memory ได้เลย
/// 4. รองรับภาษา: .resx files สำหรับ th/en โดยไม่ต้องเพิ่มคอลัมน์ใน DB
/// 5. Validation: LabResult.TryFromValue() ใช้ตรวจสอบค่าใน Validator ได้ทันที
/// 6. Testable: ไม่ต้อง mock DB ในการทดสอบ
/// 7. Refactor-safe: เปลี่ยนชื่อ → compiler error ทุกจุดที่ใช้
///
/// แบบเก่า (ReferenceValue):
///   var results = await _context.ReferenceValues
///       .Where(x => x.ReferenceGroupId == 2)
///       .ToListAsync();  // ต้อง query DB ทุกครั้ง, ไม่มี type safety
///
/// แบบใหม่ (SmartEnum):
///   var results = LabResult.All;              // จาก memory, type-safe
///   var normal = LabResult.Normal;             // IntelliSense แนะนำ
///   var parsed = LabResult.FromValue("Normal"); // safe parsing
/// </summary>
public sealed class LabResult : SmartEnum<LabResult>
{
    public static readonly LabResult Normal = new("Normal","N", "อยู่ในเกณฑ์ปกติ (Normal)", 0);
    public static readonly LabResult Abnormal = new("Abnormal","Y", "ค่าผิดปกติ (Abnormal)", 1);

    private LabResult(string value, string abnormalFlag, string displayName, int sort) : base(value,   abnormalFlag, displayName, sort) { }
}
