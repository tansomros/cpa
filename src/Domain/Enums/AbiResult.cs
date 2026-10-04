using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Domain.Enums;

/// <summary>
/// ผลการตรวจหลอดเลือด (Ankle-Brachial Index) — เทียบเท่า ReferenceGroup "ABI"
/// </summary>
public sealed class AbiResult : SmartEnum<AbiResult>
{
    public static readonly AbiResult Normal = new("Normal","N", "อยู่ในเกณฑ์ปกติ", 0);
    public static readonly AbiResult Arteriosclerosis = new("C1", "Y", "พบภาวะหลอดเลือดแข็ง", 1);
    public static readonly AbiResult Occlusion = new("C2", "Y", "พบภาวะหลอดเลือดอุดตัน", 2);

    private AbiResult(string value,string abnormalFlag, string displayName, int sort) : base(value,abnormalFlag, displayName, sort) { }
}
