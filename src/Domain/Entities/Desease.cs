namespace Cpa.Domain.Entities;

public class Desease
{
    public int UID { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? ParentUID { get; set; }

    public string? IsChapter { get; set; }

    public string? ICD { get; set; }

    public int? Sort { get; set; }

    public string? StatusFlag { get; set; }

    public string? isICD { get; set; }
}

