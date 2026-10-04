using System.Text.Json.Serialization;

namespace BigLion.CPA.Domain.ValueObjects;
public class LabStructure
{
    public required int CheckupItemId { get; set; }
    public required string CheckupItemCode { get; set; }
    public required string Name { get; set; }
    public string? ResultValue { get; set; }
    public required string ReferenceRange { get; set; }
    public string? IsAbnormal { get; set; }
    public required string CheckupGroupCode { get; set; }
}
