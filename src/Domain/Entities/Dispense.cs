namespace BigLion.CPA.Domain.Entities;

public class Dispense
{
    public long UID { get; set; }

    public string? RefID { get; set; }

    public DateOnly? RefillDate { get; set; }

    public int? PatientID { get; set; }

    public string? LocationID { get; set; }

    public string? Remark { get; set; }

    public int? MTMUID { get; set; }

    public int? DrugUID { get; set; }

    public string? TMTID { get; set; }

    public double? QTY { get; set; }

    public string? UOM { get; set; }

    public string? UsedRemark { get; set; }

    public string? CUser { get; set; }

    public DateTime? CWhen { get; set; }

    public string? MUser { get; set; }

    public DateTime? MWhen { get; set; }
}

