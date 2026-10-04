namespace BigLion.CPA.Domain.Entities;
/// <summary>
/// [OBSOLETE] ค่าอ้างอิงของแต่ละกลุ่ม — ถูกแทนที่ด้วย SmartEnum ใน Domain.Enums แล้ว
///
/// ใช้ SmartEnum แทน:
///   ExamResult.All       → แทน ReferenceValues ของกลุ่ม "GA"
///   LabResult.All        → แทน ReferenceValues ของกลุ่ม "LAB"
///   XrayResult.All       → แทน ReferenceValues ของกลุ่ม "X"
///   ฯลฯ — ดูรายละเอียดที่ LookupRegistry.cs
///
/// แบบเก่า: await _context.ReferenceValues.Where(x => x.ReferenceGroupId == 2).ToListAsync()
/// แบบใหม่: LabResult.All  // ไม่ต้อง query DB, type-safe, รองรับ i18n
/// </summary>
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน เช่น ExamResult.All, LabResult.FromValue() — ดู LookupRegistry.cs")]
public class ReferenceValue : BaseEntity
{
    /// <summary>
    /// Value ที่ใช้เก็บลง Database
    /// </summary>
    public string ValueCode { get; set; }

    /// <summary>
    /// ชื่อ/คำอธิบาย
    /// </summary>
    public string Descriptions { get; set; }   

    /// <summary>
    /// การเรียงลำดับ
    /// </summary>
    public int Sort { get; set; }

    public int ReferenceGroupId { get; set; }
    public virtual ReferenceGroup? ReferenceGroup { get; set; }
    public ReferenceValue(string valueCode, string descriptions, int referenceGroupId, int sort)
    {
        ValueCode = valueCode;
        Descriptions = descriptions;
        ReferenceGroupId = referenceGroupId;
        Sort = sort;
    }
}
