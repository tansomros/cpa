namespace BigLion.CPA.Domain.Entities;

public class DrugProblemGroup
{
    public string Code { get; set; } = string.Empty;

    public string? Descriptions { get; set; }

    public int? Sort { get; set; }

    public string? StatusFlag { get; set; }
}

