namespace Cpa.Domain.Entities;

public class Pharmacist
{
    public int Id { get; set; }

    public int? PharmacyId { get; set; }

    public string? Name { get; set; }

    public string? LicenseNo { get; set; }

    public string? WorkTime { get; set; }

    public string? WorkType { get; set; }

    public DateTime? MWhen { get; set; }

    public string? MUser { get; set; }

    public string? PositionName { get; set; }
}

