using Cpa.Domain.Common;

namespace Cpa.Domain.Enums;

/// <summary>
/// ระดับความผิดปกติของการได้ยิน — เทียบเท่า ReferenceGroup "AU"
/// </summary>
public sealed class HearingLossLevel : SmartEnum<HearingLossLevel>
{
    public static readonly HearingLossLevel Normal = new("ปกติ","N", "ปกติ", 0);
    public static readonly HearingLossLevel Mild = new("เสียเล็กน้อย","Y", "เสียเล็กน้อย", 1);
    public static readonly HearingLossLevel Moderate = new("เสียปานกลาง", "Y", "เสียปานกลาง", 2);
    public static readonly HearingLossLevel Severe = new("เสียมาก", "Y", "เสียมาก", 3);
    public static readonly HearingLossLevel Profound = new("เสียรุนแรง", "Y", "เสียรุนแรง", 4);

    private HearingLossLevel(string value,string abnormalFlag , string displayName, int sort) : base(value,abnormalFlag, displayName, sort) { }
}
