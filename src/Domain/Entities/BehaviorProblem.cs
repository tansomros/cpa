namespace BigLion.CPA.Domain.Entities;

public class BehaviorProblem
{
    public int UID { get; set; }

    public int? ServiceUID { get; set; }

    public int? ProblemUID { get; set; }

    public string? ProblemOther { get; set; }

    public string? Interventions { get; set; }

    public string? FinalResult { get; set; }

    public string? FinalResultOther { get; set; }

    public string? FatFollow { get; set; }

    public string? TasteFallow { get; set; }

    public string? ResultBegin { get; set; }

    public string? ResultEnd { get; set; }

    public string? Remark { get; set; }

    public string? isFollow { get; set; }

    public int? CUser { get; set; }

    public DateTime? CWhen { get; set; }

    public int? MUser { get; set; }

    public DateTime? MWhen { get; set; }

    public string? ServiceTypeID { get; set; }

    public int? PatientID { get; set; }
}

