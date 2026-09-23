namespace Cpa.Domain.Entities;

public class LabItem
{
    public int UID { get; set; }

    public string? Name { get; set; }

    public string? AliasName { get; set; }

    public int? UOMUID { get; set; }

    public string? NormalRange { get; set; }

    public int? Sort { get; set; }

    public string? StatusFlag { get; set; }
}

