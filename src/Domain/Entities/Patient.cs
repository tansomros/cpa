using BigLion.CPA.Domain.Enums;

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
    /// <summary>
    /// เวลาที่สะดวกให้ติดต่อกลับ
    /// </summary>
    public string? TimeContact { get; private set; }

    // Address
    /// <summary>
    /// ประเภทที่อยู่ : ที่อยู่ปัจจุบัน / ที่อยู่ตามบัตร ปชช.
    /// </summary>
    public string? AddressType { get; private set; }
    public string? AddressNo { get; private set; }
    public string? Road { get; private set; }
    public string? SubDistrictId { get; private set; }
    public string? DistrictId { get; private set; }
    public string? ProvinceId { get; private set; }
    public string? ZipCode { get; private set; }

    // General Information
    /// <summary>
    /// สิทธิ์การรักษา
    /// </summary>
    public string? MainClaim { get; private set; }
    /// <summary>
    /// การศึกษา
    /// </summary>
    public string? Education { get; private set; }
    /// <summary>
    /// อาชีพ
    /// </summary>
    public string? Occupation { get; private set; }

    // Allergy
    public bool? IsAllergy { get; private set; }
    /// <summary>
    /// ชื่อยาที่แพ้
    /// </summary>
    public string? DrugAllergy { get; private set; }

    // Smoking
 
    /// <summary>
    /// การสูบบุหรี่
    /// </summary>
    public string? Smoke { get; private set; }
    /// <summary>
    /// จำนวนปี
    /// </summary>
    public int? SmokeYear { get; private set; }
    /// <summary>
    /// จำนวนมวน/วัน
    /// </summary>
    public int? SmokeCigarette { get; private set; }
    /// <summary>
    /// ชนิดของบุหรี่ที่สูบ
    /// </summary>
    public string? CigaretteType { get; private set; }
    /// <summary>
    /// อยากจะลดหรือเลิกสูบบุหรี่หรือไม่
    /// </summary>
    public bool? SmokingQuit { get; private set; }
    public string? SmokingRemark { get; private set; }

    // Alcohol
    /// <summary>
    /// การดิ่มเครื่องดื่มแอลกอฮอล์
    /// </summary>
    public string? Drinking { get; private set; }
    /// <summary>
    /// ความถี่ในการดื่ม วัน/สัปดาห์
    /// </summary>
    public int? DrinkFrequency { get; private set; }

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
        string? cardId)
    {
        ForeName = foreName;
        Surname = surname;
        Gender = gender;
        BirthDate = birthDate;
        CardId = cardId;
    }

    public void UpdateContact(string? telephone, string? timeContact)
    {
        Telephone = telephone;
        TimeContact = timeContact;
    }

    public void UpdateAddress(
        string? addressType,
        string? addressNo,
        string? road,
        string? districtId,
        string? city,
        string? provinceId,
        string? zipCode)
    {
        AddressType = addressType;
        AddressNo = addressNo;
        Road = road;
        SubDistrictId = districtId;
        DistrictId = city;
        ProvinceId = provinceId;
        ZipCode = zipCode;
    }

    public void UpdateGeneralInformation(
        string? mainClaim,
        bool isActive,
        string? education,
        string? occupation)
    {
        MainClaim = mainClaim;
        IsActive = isActive;
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

    // Cross-field rules live here so every caller gets the same result:
    // - cigarette type, smoking years and cigarettes per day are kept only for Regular or Quit smokers;
    // - SmokingQuit ("wants to cut down or quit") is kept only for Regular smokers;
    // - drink frequency (days per week) is kept only for Occasional or Regular drinkers.
    // Any other status (including empty) clears those fields to null.
    public void UpdateSmokingHistory(
        string? smoke,
        int? smokeYear,
        int? smokeCigarette,
        string? cigaretteType,
        bool? smokingQuit,
        string? smokingRemark)
    {
        var status = ParseCode<SmokingValue>(smoke);
        var hasSmoked = status == SmokingValue.Regular || status == SmokingValue.Quit;

        Smoke = smoke;
        SmokeYear = hasSmoked ? smokeYear : null;
        SmokeCigarette = hasSmoked ? smokeCigarette : null;
        CigaretteType = hasSmoked ? cigaretteType : null;
        SmokingQuit = status == SmokingValue.Regular ? smokingQuit : null;
        SmokingRemark = smokingRemark;
    }

    public void UpdateAlcoholHistory(
        string? alcohol,
        int? alcoholFQ)
    {
        var status = ParseCode<DrinkingValue>(alcohol);
        var drinks = status == DrinkingValue.Occasional || status == DrinkingValue.Regular;

        Drinking = alcohol;
        DrinkFrequency = drinks ? alcoholFQ : null;
    }

    private static T? ParseCode<T>(string? code) where T : SmartEnum<T>
        => code is not null && SmartEnum<T>.TryFromValue(code, out var value) ? value : null;
} 
