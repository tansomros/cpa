using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Domain.Enums;

/// <summary>
/// ผลการตรวจสายตา — เทียบเท่า ReferenceGroup "EYE"
/// </summary>
public sealed class EyeResult : SmartEnum<EyeResult>
{
    public static readonly EyeResult Normal = new("ปกติ", "N", "ปกติ", 0);
    public static readonly EyeResult Farsighted = new("สายตายาว", "Y", "สายตายาว", 1);
    public static readonly EyeResult Nearsighted = new("สายตาสั้น", "Y", "สายตาสั้น", 2);

    private EyeResult(string value,string abnormalFlag, string displayName, int sort) : base(value,abnormalFlag, displayName, sort) { }
}
