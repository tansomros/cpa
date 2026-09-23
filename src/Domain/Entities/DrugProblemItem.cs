namespace Cpa.Domain.Entities;

public class DrugProblemItem
{
    public string Code { get; set; } = string.Empty;

    public string? Descriptions { get; set; }

    public string? DrugProblemGroupUID { get; set; }

    public int? Sort { get; set; }

    public string? StatusFlag { get; set; }
}

