namespace Cpa.Domain.Entities;

public class PaymentMethod
{
    public int PaymentID { get; set; }

    public string? PaymentName { get; set; }

    public double? Amount { get; set; }

    public long? EffectiveTo { get; set; }

    public string? StatusFlag { get; set; }

    public string? ServiceTypeID { get; set; }
}

