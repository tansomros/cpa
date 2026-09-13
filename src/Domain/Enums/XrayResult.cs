using Cpa.Domain.Common;

namespace Cpa.Domain.Enums;

/// <summary>
/// ผลการตรวจเอกซเรย์ (X-ray) — เทียบเท่า ReferenceGroup "X"
/// </summary>
public sealed class XrayResult : SmartEnum<XrayResult>
{
    public static readonly XrayResult Normal = new("Normal","N", "ไม่พบความผิดปกติ", 0);
    public static readonly XrayResult Abnormal = new("Abnormal","Y", "ตรวจพบความผิดปกติ", 1);
    public static readonly XrayResult WaitForSpecialist = new("W","W", "ส่งแพทย์เฉพาะทางอ่านผล", 9);

    private XrayResult(string value,string flag, string displayName, int sort) : base(value,flag, displayName, sort) { }
}
