using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Domain.Enums;

/// <summary>
/// แรงบีบมือ
/// </summary>
public sealed class HandGripResult : SmartEnum<HandGripResult>
{
    public static readonly HandGripResult Normal = new("N","N", "ปกติ", 1); 
    public static readonly HandGripResult Low = new("L","Y", "ต่ำกว่าปกติเล็กน่อย", 0);

    private HandGripResult(string value, string abnormalFlag, string displayName, int sort) : base(value,   abnormalFlag, displayName, sort) { }
} 
