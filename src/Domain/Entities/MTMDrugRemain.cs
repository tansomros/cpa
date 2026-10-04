namespace BigLion.CPA.Domain.Entities;

public class MTMDrugRemain
{
    public int UID { get; set; }

    public int? MTMUID { get; set; }

    public string? ServiceTypeID { get; set; }

    public int? DrugUID { get; set; }

    public double? QTY { get; set; }

    public string? UOM { get; set; }

    public string? CUser { get; set; }

    public DateTime? CWhen { get; set; }

    public string? MUser { get; set; }

    public DateTime? MWhen { get; set; }

    public int? PatientID { get; set; }

    public string? RemainFrom { get; set; }

    public int? ReasonUID { get; set; }

    public string? ReasonRemark { get; set; }

    public string? TMTID { get; set; }
}

