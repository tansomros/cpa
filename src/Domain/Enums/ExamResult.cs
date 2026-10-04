using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Domain.Enums;

/// <summary>
/// ผลการตรวจทั่วไป (General Examination) — แทนที่ ReferenceGroup "GA" (Id=1) + ReferenceValue ในฐานข้อมูล
///
/// ข้อดีของ SmartEnum เทียบกับ ReferenceValue:
/// ─────────────────────────────────────────────
/// 1. Type-safe: ExamResult.Normal แทน magic string "Normal"
/// 2. IntelliSense: IDE แนะนำค่าที่ใช้ได้ทั้งหมด
/// 3. ไม่ต้อง query DB: ExamResult.All ดึงค่าจาก memory ได้เลย
/// 4. รองรับภาษา: .resx files สำหรับ th/en โดยไม่ต้องเพิ่มคอลัมน์ใน DB
/// 5. Validation: ExamResult.TryFromValue() ใช้ตรวจสอบค่าใน Validator ได้ทันที
/// 6. Testable: ไม่ต้อง mock DB ในการทดสอบ
/// 7. Refactor-safe: เปลี่ยนชื่อ → compiler error ทุกจุดที่ใช้
/// </summary>
public sealed class ExamResult : SmartEnum<ExamResult>
{
    public static readonly ExamResult NotExamined = new("ไม่ได้ตรวจ", "N", "ไม่ได้ตรวจ", 0);
    public static readonly ExamResult Normal = new("Normal","N", "ปกติ (Normal)", 1);
    public static readonly ExamResult Abnormal = new("Abnormal","Y", "ผิดปกติ (Abnormal)", 2);

    private ExamResult(string value, string abnormalFlag, string displayName, int sort) : base(value,   abnormalFlag, displayName, sort) { }
}
