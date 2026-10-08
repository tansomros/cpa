using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Domain.Enums;

/// <summary>
/// ชนิดของบุหรี่ที่สูบ (เก็บเป็นรหัสข้อความใน Patient.CigaretteType และ MTM.CigaretteType)
/// ถ้าเลือก Other ให้กรอกรายละเอียดใน SmokingRemark
/// </summary>
public sealed class CigaretteTypeValue : SmartEnum<CigaretteTypeValue>
{
    public static readonly CigaretteTypeValue Manufactured = new("Manufactured", "บุหรี่ซอง", 0);
    public static readonly CigaretteTypeValue RollYourOwn = new("RollYourOwn", "ยาเส้นมวนเอง", 1);
    public static readonly CigaretteTypeValue Electronic = new("Electronic", "บุหรี่ไฟฟ้า", 2);
    public static readonly CigaretteTypeValue Other = new("Other", "อื่นๆ", 3);

    private CigaretteTypeValue(string value, string displayName, int sort) : base(value, displayName, sort) { }
}
