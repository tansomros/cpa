using Ardalis.GuardClauses;

namespace BigLion.CPA.Domain.Entities;

public class Pharmacy : BaseEntity
{
    public string Code { get; private set; }
    public string? LicenseNo { get; private set; }
    public string? NhsoCode { get; private set; }
    public string Name { get; private set; }
    public string? Name2 { get; private set; }
    public int? PharmacyGroupId { get; private set; }
    public virtual PharmacyGroup? PharmacyGroup { get; private set; }
    public int? PharmacyTypeId { get; private set; }
    public virtual PharmacyType? PharmacyType { get; private set; }

    public string? AddressNo { get; private set; }
    public string? DistrictId { get; private set; }
    public string? SubDistrictId { get; private set; }
    public string? ProvinceId { get; private set; }
    public virtual Province? Province { get; private set; }
    public virtual District? District { get; private set; }
    public virtual SubDistrict? SubDistrict { get; private set; }
    public string? ZipCode { get; private set; }
    public string? Fda_Province { get; private set; }
    public string? Office_Tel { get; private set; }
    public string? Office_Fax { get; private set; }
    public string? Office_Mail { get; private set; }
    public string? LineID { get; private set; }
    public string? Co_Name { get; private set; }
    public string? Co_Mail { get; private set; }
    public string? Co_Tel { get; private set; }

    // Bank account fields stay out of this model until that feature is in scope:
    // AccNo, AccName, BankID, BankBrunch, BankType

    public string? PharmacyTypeOther { get; private set; }
    public string? RegisYear { get; private set; }
    public string? Lat { get; private set; }
    public string? Lng { get; private set; }

    public Pharmacy(string code, string name)
    {
        Code = Guard.Against.NullOrWhiteSpace(code);
        Name = Guard.Against.NullOrWhiteSpace(name);
    }

    public void Update(
        string code,
        string name,
        string? licenseNo,
        string? nhsoCode,
        string? name2,
        int? pharmacyGroupId,
        int? pharmacyTypeId,
        string? pharmacyTypeOther,
        string? addressNo,
        string? provinceId,
        string? districtId,
        string? subDistrictId,
        string? zipCode,
        string? fdaProvince,
        string? officeTel,
        string? officeFax,
        string? officeMail,
        string? lineId,
        string? coName,
        string? coMail,
        string? coTel,
        string? regisYear,
        string? lat,
        string? lng,
        bool isActive)
    {
        Code = Guard.Against.NullOrWhiteSpace(code);
        Name = Guard.Against.NullOrWhiteSpace(name);
        AssignDetails(
            licenseNo,
            nhsoCode,
            name2,
            pharmacyGroupId,
            pharmacyTypeId,
            pharmacyTypeOther,
            addressNo,
            provinceId,
            districtId,
            subDistrictId,
            zipCode,
            fdaProvince,
            officeTel,
            officeFax,
            officeMail,
            lineId,
            coName,
            coMail,
            coTel,
            regisYear,
            lat,
            lng);
        IsActive = isActive;
    }

    public void AssignDetails(
        string? licenseNo,
        string? nhsoCode,
        string? name2,
        int? pharmacyGroupId,
        int? pharmacyTypeId,
        string? pharmacyTypeOther,
        string? addressNo,
        string? provinceId,
        string? districtId,
        string? subDistrictId,
        string? zipCode,
        string? fdaProvince,
        string? officeTel,
        string? officeFax,
        string? officeMail,
        string? lineId,
        string? coName,
        string? coMail,
        string? coTel,
        string? regisYear,
        string? lat,
        string? lng)
    {
        LicenseNo = licenseNo;
        NhsoCode = nhsoCode;
        Name2 = name2;
        PharmacyGroupId = pharmacyGroupId;
        PharmacyTypeId = pharmacyTypeId;
        PharmacyTypeOther = pharmacyTypeOther;
        AddressNo = addressNo;
        ProvinceId = provinceId;
        DistrictId = districtId;
        SubDistrictId = subDistrictId;
        ZipCode = zipCode;
        Fda_Province = fdaProvince;
        Office_Tel = officeTel;
        Office_Fax = officeFax;
        Office_Mail = officeMail;
        LineID = lineId;
        Co_Name = coName;
        Co_Mail = coMail;
        Co_Tel = coTel;
        RegisYear = regisYear;
        Lat = lat;
        Lng = lng;
    }

    public void Deactivate()
    {
        IsActive = false;
        DeleteFlag = true;
    }
}
