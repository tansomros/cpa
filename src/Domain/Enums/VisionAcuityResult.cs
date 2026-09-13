using Cpa.Domain.Common;

namespace Cpa.Domain.Enums;

/// <summary>
/// ระดับสายตา — แทนที่ ReferenceGroup "VA" (Id=7)
///
/// ข้อดีของ SmartEnum เทียบกับ ReferenceValue:
/// ─────────────────────────────────────────────
/// 1. Type-safe: VisionAcuityResult.Normal แทน magic string "ปกติ"
/// 2. IntelliSense: IDE แนะนำค่าที่ใช้ได้ทั้งหมด
/// 3. ไม่ต้อง query DB: VisionAcuityResult.All ดึงค่าจาก memory ได้เลย
/// 4. รองรับภาษา: .resx files สำหรับ th/en โดยไม่ต้องเพิ่มคอลัมน์ใน DB
/// 5. Validation: VisionAcuityResult.TryFromValue() ใช้ตรวจสอบค่าใน Validator ได้ทันที
/// 6. Testable: ไม่ต้อง mock DB ในการทดสอบ
/// 7. Refactor-safe: เปลี่ยนชื่อ → compiler error ทุกจุดที่ใช้
///
/// แบบเก่า (ReferenceValue):
///   var results = await _context.ReferenceValues
///       .Where(x => x.ReferenceGroupId == 7)
///       .ToListAsync();  // ต้อง query DB ทุกครั้ง, ไม่มี type safety
///
/// แบบใหม่ (SmartEnum):
///   var results = VisionAcuityResult.All;              // จาก memory, type-safe
///   var normal = VisionAcuityResult.Normal;             // IntelliSense แนะนำ
///   var parsed = VisionAcuityResult.FromValue("ปกติ");  // safe parsing
/// </summary>
public sealed class VisionAcuityResult : SmartEnum<VisionAcuityResult>
{
    public static readonly VisionAcuityResult Normal = new("ปกติ","N", "ปกติ", 0);
    public static readonly VisionAcuityResult Farsighted = new("สายตายาว", "Y", "สายตายาว", 1);
    public static readonly VisionAcuityResult Nearsighted = new("สายตาสั้น","Y", "สายตาสั้น", 2);

    private VisionAcuityResult(string value, string abnormalFlag, string displayName, int sort) : base(value,   abnormalFlag, displayName, sort) { }
}
