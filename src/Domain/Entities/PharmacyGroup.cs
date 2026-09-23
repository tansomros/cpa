namespace Cpa.Domain.Entities;

public class PharmacyGroup
{
    public string Code { get; set; } = string.Empty;

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public int? IsPublic { get; set; }
}

