namespace BigLion.CPA.Domain.Entities;

/// <summary>
/// ข้อมูลผู้ป่วย/ผู้รับบริการ
/// </summary>
public class Patient : BaseEntity
{
    // Personal Information
    public string? ForeName { get; private set; }
    public string? Surname { get; private set; }
    public string? Gender { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public string? CardId { get; private set; }

    // Contact
    public string? Telephone { get; private set; }
    public string? Mobile { get; private set; }
    public string? TimeContact { get; private set; }

    // Address
    public string? AddressType { get; private set; }
    public string? AddressNo { get; private set; }
    public string? Road { get; private set; }
    public string? DistrictId { get; private set; }
    public string? City { get; private set; }
    public string? ProvinceId { get; private set; }
    public string? ProvinceName { get; private set; }
    public string? ZipCode { get; private set; }

    // General Information
    public string? MainClaim { get; private set; }
    public int? Status { get; private set; }
    public string? Education { get; private set; }
    public string? Occupation { get; private set; }

    // Allergy
    public bool? IsAllergy { get; private set; }
    public string? DrugAllergy { get; private set; }

    // Smoking
    public bool? IsSmoke { get; private set; }
    public int? Smoke { get; private set; }
    public int? SmokeYear { get; private set; }
    public int? SmokeCigarette { get; private set; }
    public int? CigaretteType { get; private set; }
    public bool? SmokingQuit { get; private set; }
    public string? SmokingRemark { get; private set; }

    // Alcohol
    public int? Alcohol { get; private set; }
    public int? AlcoholFQ { get; private set; }


    // Navigation properties
    public virtual Province? Province { get; set; }
    public virtual District? District { get; set; }
    public virtual SubDistrict? SubDistrict { get; set; }

    private Patient()
    {
        // Required by EF Core
    }

    public Patient(
        string? foreName,
        string? surname,
        string? cardId,
        string? gender = null,
        DateOnly? birthDate = null)
    {
        ForeName = foreName;
        Surname = surname;
        CardId = cardId;
        Gender = gender;
        BirthDate = birthDate;
    }

    public void UpdatePersonalInformation(
        string? foreName,
        string? surname,
        string? gender,
        DateOnly? birthDate,
        int? ages,
        string? cardId)
    {
        ForeName = foreName;
        Surname = surname;
        Gender = gender;
        BirthDate = birthDate;        
        CardId = cardId;
    }

    public void UpdateContact(
        string? telephone,
        string? mobile,
        string? timeContact)
    {
        Telephone = telephone;
        Mobile = mobile;
        TimeContact = timeContact;
    }

    public void UpdateAddress(
        string? addressType,
        string? addressNo,
        string? road,
        string? districtId,
        string? city,
        string? provinceId,
        string? provinceName,
        string? zipCode)
    {
        AddressType = addressType;
        AddressNo = addressNo;
        Road = road;
        DistrictId = districtId;
        City = city;
        ProvinceId = provinceId;
        ProvinceName = provinceName;
        ZipCode = zipCode;
    }

    public void UpdateGeneralInformation(
        string? mainClaim,
        int? status,
        string? education,
        string? occupation)
    {
        MainClaim = mainClaim;
        Status = status;
        Education = education;
        Occupation = occupation;
    }


    public void UpdateAllergy(
        bool? isAllergy,
        string? drugAllergy)
    {
        IsAllergy = isAllergy;
        DrugAllergy = drugAllergy;
    }

    public void UpdateSmokingHistory(
        bool? isSmoke,
        int? smoke,
        int? smokeYear,
        int? smokeCigarette,
        int? cigaretteType,
        bool? smokingQuit,
        string? smokingRemark)
    {
        IsSmoke = isSmoke;
        Smoke = smoke;
        SmokeYear = smokeYear;
        SmokeCigarette = smokeCigarette;
        CigaretteType = cigaretteType;
        SmokingQuit = smokingQuit;
        SmokingRemark = smokingRemark;
    }

    public void UpdateAlcoholHistory(
        int? alcohol,
        int? alcoholFQ)
    {
        Alcohol = alcohol;
        AlcoholFQ = alcoholFQ;
    }
} 
