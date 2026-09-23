namespace Cpa.Domain.Entities;

public class DrugMaster
{
    public int UID { get; set; }

    public string? TMTID { get; set; }

    public string? Name { get; set; }

    public string? AliasName { get; set; }

    public string? Manufacturer { get; set; }

    public string? FSN { get; set; }

    public int? Sort { get; set; }

    public string? StatusFlag { get; set; }

    public int? OUID { get; set; }
}

