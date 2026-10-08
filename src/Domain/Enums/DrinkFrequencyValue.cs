using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Domain.Enums;

/// <summary>
/// ความถี่ในการดื่มเครื่องดื่มแอลกอฮอล์ (เก็บเป็นรหัสข้อความใน Patient.DrinkFrequency และ MTM.AlcoholFQ)
/// </summary>
public sealed class DrinkFrequencyValue : SmartEnum<DrinkFrequencyValue>
{
    public static readonly DrinkFrequencyValue LessThanMonthly = new("LessThanMonthly", "น้อยกว่าเดือนละครั้ง", 0);
    public static readonly DrinkFrequencyValue Monthly = new("Monthly", "เดือนละครั้งขึ้นไป", 1);
    public static readonly DrinkFrequencyValue Weekly = new("Weekly", "สัปดาห์ละครั้งขึ้นไป", 2);
    public static readonly DrinkFrequencyValue Daily = new("Daily", "ทุกวัน", 3);

    private DrinkFrequencyValue(string value, string displayName, int sort) : base(value, displayName, sort) { }
}
