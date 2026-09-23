namespace Cpa.Domain.Entities;

public class Pharmacy
{
    public int UID { get; set; }

    public string LocationID { get; set; } = string.Empty;

    public string? LocationName { get; set; }

    public string? LocationName2 { get; set; }

    public string? LocationName3 { get; set; }

    public string? LocationGroupID { get; set; }

    public string? Address { get; set; }

    public string? ProvinceID { get; set; }

    public string? ProvinceName { get; set; }

    public string? FDA_Province { get; set; }

    public string? ZipCode { get; set; }

    public string? Office_Tel { get; set; }

    public string? Office_Fax { get; set; }

    public string? Office_Mail { get; set; }

    public string? LineID { get; set; }

    public string? Co_Name { get; set; }

    public string? Co_Mail { get; set; }

    public string? Co_Tel { get; set; }

    public string? AccNo { get; set; }

    public string? AccName { get; set; }

    public string? BankID { get; set; }

    public string? BankBrunch { get; set; }

    public string? BankType { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string? UpdBy { get; set; }

    public int? isPublic { get; set; }

    public string? CardID { get; set; }

    public string? LocationType { get; set; }

    public string? TypeOther { get; set; }

    public string? RegisYear { get; set; }

    public string? LocationCode { get; set; }

    public string? LicenseNo { get; set; }

    public string? Lat { get; set; }

    public string? Lng { get; set; }

    public string? CUser { get; set; }

    public DateTime? CWhen { get; set; }

    public string? MUser { get; set; }

    public DateTime? MWhen { get; set; }
}

