using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Domain.Enums;

/// <summary>
/// การดื่มเครื่องดื่มที่มีแอลกอฮอล์ — แทนที่ ReferenceGroup + ReferenceValue ในฐานข้อมูล
///
/// ข้อดีของ SmartEnum เทียบกับ ReferenceValue:
/// ─────────────────────────────────────────────
/// 1. Type-safe: DrinkingValue.Non แทน magic string "ไม่ดื่ม"
/// 2. IntelliSense: IDE แนะนำค่าที่ใช้ได้ทั้งหมด
/// 3. ไม่ต้อง query DB: DrinkingValue.All ดึงค่าจาก memory ได้เลย
/// 4. รองรับภาษา: .resx files สำหรับ th/en โดยไม่ต้องเพิ่มคอลัมน์ใน DB
/// 5. Validation: DrinkingValue.TryFromValue() ใช้ตรวจสอบค่าใน Validator ได้ทันที
/// 6. Testable: ไม่ต้อง mock DB ในการทดสอบ
/// 7. Refactor-safe: เปลี่ยนชื่อ → compiler error ทุกจุดที่ใช้
///
/// แบบเก่า (ReferenceValue):
///   var results = await _context.ReferenceValues
///       .Where(x => x.ReferenceGroupId == 2)
///       .ToListAsync();  // ต้อง query DB ทุกครั้ง, ไม่มี type safety
///
/// แบบใหม่ (SmartEnum):
///   var results = DrinkingValue.All;              // จาก memory, type-safe
///   var normal = DrinkingValue.Non;             // IntelliSense แนะนำ
///   var parsed = DrinkingValue.FromValue("No"); // safe parsing
/// </summary>
public sealed class DrinkingValue : SmartEnum<DrinkingValue>
{
    public static readonly DrinkingValue Non = new("Non", "ไม่ดื่ม", 0);
    public static readonly DrinkingValue Quit = new("Quit",  "เคยดื่มแต่เลิกแล้ว", 1);
    public static readonly DrinkingValue Occasional = new("Occasional",  "ดื่มครั้งคราว", 2);
    public static readonly DrinkingValue Regular = new("Regular", "ดื่มประจำ", 2);

    private DrinkingValue(string value,  string displayName, int sort) : base(value, displayName, sort) { }
}
