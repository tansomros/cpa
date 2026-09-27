namespace Cpa.Domain.Entities;

public class Pharmacist : BaseEntity
{
    public string? Name { get; set; }

    public string? LicenseNo { get; set; }

    public string? WorkTime { get; set; }

    public string? WorkType { get; set; }
  
    public string? PositionName { get; set; }
    public int? PharmacyId { get; set; }
    public virtual Pharmacy? Pharmacy { get; set; }

    public Pharmacist()
    {
        
    }
}

