namespace BigLion.CPA.Domain.Entities;
/// <summary>
/// [OBSOLETE] กลุ่มค่าอ้างอิง — ถูกแทนที่ด้วย SmartEnum ใน Domain.Enums แล้ว
///
/// ใช้ SmartEnum แทน:
///   ExamResult      → แทน ReferenceGroup "GA"  (Id=1)
///   LabResult       → แทน ReferenceGroup "LAB" (Id=2)
///   XrayResult      → แทน ReferenceGroup "X"   (Id=3)
///   BmdResult       → แทน ReferenceGroup "BMD" (Id=4)
///   AbiResult       → แทน ReferenceGroup "ABI" (Id=5)
///   CheckupStatus   → แทน ReferenceGroup "CKST"(Id=6)
///   VisionAcuityResult → แทน ReferenceGroup "VA" (Id=7)
///   EyeResult       → แทน ReferenceGroup "EYE" (Id=8)
///   HearingLossLevel → แทน ReferenceGroup "AU" (Id=9)
///
/// ดูตัวอย่างการใช้งานที่ LookupRegistry.cs และ SmartEnumBindingHelper.cs
/// </summary>
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน เช่น ExamResult, LabResult, XrayResult — ดู LookupRegistry.cs")]
public class ReferenceGroup : BaseEntity
{
    /// <summary>
    /// รหัส
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// ชื่อ/คำอธิบาย
    /// </summary>
    public string Descriptions { get; set; }   

    /// <summary>
    /// การเรียงลำดับ
    /// </summary>
    public int Sort { get; set; }

    public ReferenceGroup(string code, string descriptions, int sort)
    {
        Code = code;
        Descriptions = descriptions; 
        Sort = sort;
    }
}
