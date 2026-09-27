namespace Cpa.Domain.Entities;

public class Pharmacy : BaseEntity
{
  
    public string Code { get; set; }
    public string? LicenseNo { get; set; } 
    public string? NhsoCode { get; set; }
    public string Name { get; set; }
    public string? Name2 { get; set; }
    public string? PharmacyGroupId { get; set; }
    public virtual PharmacyGroup? PharmacyGroup { get; set; }
    public string? PharmacyTypeId { get; set; }
    public virtual PharmacyType? PharmacyType { get; set; }

    public string? AddressNo { get; set; }
    public string? DistrictId { get; set;  }
    public string? SubDistrictId { get; set; }
    public string? ProvinceId { get; set; }
    public virtual Province? Province { get; set; }
    public virtual District? District { get; set; }
    public virtual SubDistrict? SubDistrict { get; set; }
    public string? ZipCode { get; set; }
    public string? Fda_Province { get; set; }
    public string? Office_Tel { get; set; }

    public string? Office_Fax { get; set; }

    public string? Office_Mail { get; set; }

    public string? LineID { get; set; }

    public string? Co_Name { get; set; }

    public string? Co_Mail { get; set; }

    public string? Co_Tel { get; set; }

    //public string? AccNo { get; set; }

    //public string? AccName { get; set; }

    //public string? BankID { get; set; }

    //public string? BankBrunch { get; set; }

    //public string? BankType { get; set; }

    public string? PharmacyTypeOther { get; set; }

    public string? RegisYear { get; set; }

    public string? Lat { get; set; }

    public string? Lng { get; set; }

    public Pharmacy(string code,string name)
    {
        Code = code;
        Name = name;
    }

}

