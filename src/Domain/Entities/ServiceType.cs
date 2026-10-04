namespace BigLion.CPA.Domain.Entities;

public class ServiceType
{
    public string ServiceTypeID { get; set; } = string.Empty;

    public string? ServiceName { get; set; }

    public string? Descriptions { get; set; }

    public string? Status { get; set; }

    public int? ProjectID { get; set; }
}

