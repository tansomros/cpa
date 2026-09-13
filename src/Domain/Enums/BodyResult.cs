using Cpa.Domain.Common;

namespace Cpa.Domain.Enums;

/// <summary>
/// ผลการ Body Composition
/// </summary>
public sealed class BodyResult : SmartEnum<BodyResult>
{
    public static readonly BodyResult Normal = new("N", "N", "ปกติ", 1);
    public static readonly BodyResult High = new("H", "Y", "สูงกว่าปกติ", 2);
    public static readonly BodyResult Low = new("L", "Y", "ต่ำ", 0);

    private BodyResult(string value,string abnormalFlag, string displayName, int sort) : base(value,abnormalFlag, displayName, sort) { }
}
