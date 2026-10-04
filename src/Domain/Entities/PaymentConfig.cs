namespace BigLion.CPA.Domain.Entities;

public class PaymentConfig
{
    public int itemID { get; set; }

    public string? ProvinceID { get; set; }

    public int? PaymentID { get; set; }

    public long? EffectiveTo { get; set; }

    public string? StatusFlag { get; set; }

    public int? ProjectID { get; set; }
}

