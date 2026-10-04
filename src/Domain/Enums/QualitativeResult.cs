using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Domain.Enums;

public sealed class QualitativeResult : SmartEnum<QualitativeResult>
{
    public static readonly QualitativeResult Negative = new("Negative","", "Negative", 0);
    public static readonly QualitativeResult Positive = new("Positive","", "Positive", 1);
    private QualitativeResult(string value, string abnormalFlag, string displayName, int sort) : base(value, abnormalFlag, displayName, sort) { }
}
