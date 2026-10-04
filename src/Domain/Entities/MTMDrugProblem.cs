namespace BigLion.CPA.Domain.Entities;

public class MTMDrugProblem
{
    public int UID { get; set; }

    public int? MTMUID { get; set; }

    public string? ServiceTypeID { get; set; }

    public string? ProblemGroupUID { get; set; }

    public string? ProblemUID { get; set; }

    public string? ProblemOther { get; set; }

    public int? DrugUID { get; set; }

    public string? Interventions { get; set; }

    public string? FinalResult { get; set; }

    public string? FinalResultOther { get; set; }

    public string? CUser { get; set; }

    public DateTime? CWhen { get; set; }

    public string? MUser { get; set; }

    public DateTime? MWhen { get; set; }

    public int? PatientID { get; set; }

    public string? TMTID { get; set; }

    public int? DrugUID_Old { get; set; }
}

