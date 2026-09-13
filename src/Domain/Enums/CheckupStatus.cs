using Cpa.Domain.Common;

namespace Cpa.Domain.Enums;

/// <summary>
/// สถานะการตรวจสุขภาพ — เทียบเท่า ReferenceGroup "CKST"
/// </summary>
public sealed class CheckupStatus : SmartEnum<CheckupStatus>
{
    public static readonly CheckupStatus Pending = new("1", "W", "รอตรวจ", 1);
    public static readonly CheckupStatus InProgress = new("2","O", "กำลังตรวจ", 2);
    public static readonly CheckupStatus Reported = new("3", "R", "รายงานผล", 3);

    private CheckupStatus(string value, string abnormalFlag, string displayName, int sort) : base(value,   abnormalFlag, displayName, sort) { }
}
