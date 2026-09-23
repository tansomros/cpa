namespace Cpa.Domain.Entities;

public class LabResult
{
    public int UID { get; set; }

    public int? RefUID { get; set; }

    public int? ResultDate { get; set; }

    public int? PatientID { get; set; }

    public int? LabUID { get; set; }

    public string? ResultValue { get; set; }

    public string? IsNormal { get; set; }

    public string? StatusFlag { get; set; }

    public int? CUser { get; set; }

    public DateTime? CWhen { get; set; }

    public int? MUser { get; set; }

    public DateTime? MWhen { get; set; }
}

