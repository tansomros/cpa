using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Domain.Enums;

/// <summary>
/// การสูบบุหรี่ — แทนที่ ReferenceGroup + ReferenceValue ในฐานข้อมูล
///
/// ข้อดีของ SmartEnum เทียบกับ ReferenceValue:
/// ─────────────────────────────────────────────
/// 1. Type-safe: SmokingValue.Non แทน magic string "ไม่สูบ"
/// 2. IntelliSense: IDE แนะนำค่าที่ใช้ได้ทั้งหมด
/// 3. ไม่ต้อง query DB: SmokingValue.All ดึงค่าจาก memory ได้เลย
/// 4. รองรับภาษา: .resx files สำหรับ th/en โดยไม่ต้องเพิ่มคอลัมน์ใน DB
/// 5. Validation: SmokingValue.TryFromValue() ใช้ตรวจสอบค่าใน Validator ได้ทันที
/// 6. Testable: ไม่ต้อง mock DB ในการทดสอบ
/// 7. Refactor-safe: เปลี่ยนชื่อ → compiler error ทุกจุดที่ใช้
///
/// แบบเก่า (ReferenceValue):
///   var results = await _context.ReferenceValues
///       .Where(x => x.ReferenceGroupId == 2)
///       .ToListAsync();  // ต้อง query DB ทุกครั้ง, ไม่มี type safety
///
/// แบบใหม่ (SmartEnum):
///   var results = SmokingValue.All;              // จาก memory, type-safe
///   var normal = SmokingValue.Non;             // IntelliSense แนะนำ
///   var parsed = SmokingValue.FromValue("No"); // safe parsing
/// </summary>
public sealed class SmokingValue : SmartEnum<SmokingValue>
{
    public static readonly SmokingValue Non = new("Non","N", "ไม่สูบ", 0);
    public static readonly SmokingValue Regularly = new("Yes","Y", "สูบประจำ", 1);
    public static readonly SmokingValue Quit = new("Quit", "Q", "เลิกสูบแล้ว", 2);

    private SmokingValue(string value, string abnormalFlag, string displayName, int sort) : base(value,   abnormalFlag, displayName, sort) { }
}
