using Cpa.Domain.Common;

namespace Cpa.Domain.Enums;

/// <summary>
/// ผลการตรวจมวลกระดูก (Bone Mineral Density) — เทียบเท่า ReferenceGroup "BMD"
/// </summary>
public sealed class BmdResult : SmartEnum<BmdResult>
{
    public static readonly BmdResult Normal = new("Normal","N", "มวลกระดูกอยู่ในเกณฑ์ปกติ", 0);
    public static readonly BmdResult OsteopeniaRisk = new("C1","Y", "มวลกระดูกเริ่มบางเมื่อเทียบกับช่วงอายุเดียวกัน", 1);
    public static readonly BmdResult Osteoporosis = new("C2","Y", "กระดูกพรุนเมื่อเทียบกับช่วงอายุเดียวกัน", 2);

    private BmdResult(string value, string abnormalFlag, string displayName, int sort) : base(value,   abnormalFlag, displayName, sort) { }
}
