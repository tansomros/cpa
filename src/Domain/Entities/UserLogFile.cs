namespace Cpa.Domain.Entities;

public class UserLogFile
{
    public long LogID { get; set; }

    public int? UserID { get; set; }

    public DateTime? Work_Date { get; set; }

    public string? Act_Type { get; set; }

    public string? DB_Effective { get; set; }

    public string? Descrp { get; set; }

    public string? Remark { get; set; }
}

