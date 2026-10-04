namespace BigLion.CPA.Domain.Entities;

public class MTMDesease
{
    public int UID { get; set; }

    public int? MTMUID { get; set; }

    public string? ServiceTypeID { get; set; }

    public int? DeseaseUID { get; set; }

    public string? DeseaseOther { get; set; }

    public int? CUser { get; set; }

    public DateTime? CWhen { get; set; }

    public int? PatientID { get; set; }

    public string? ICDCode { get; set; }

    public string? DeseaseName { get; set; }
}

