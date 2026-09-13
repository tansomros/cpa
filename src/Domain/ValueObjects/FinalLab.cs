namespace Cpa.Domain.ValueObjects;
public class FinalLab
{
    public required string CheckupGroupCode { get; set; }
    public required string Name { get; set; }
    public string? ResultValue { get; set; }
    public required string ReferenceRange { get; set; }
    public string? IsAbnormal { get; set; }
}
