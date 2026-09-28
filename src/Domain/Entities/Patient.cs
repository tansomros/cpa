namespace Cpa.Domain.Entities;

/// <summary>
/// ข้อมูลผู้รับบริการ
/// </summary>
public class Patient : BaseEntity
{

    /// <summary>
    /// คำนำหน้าชื่อไทย
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// ชื่อไทย
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// นามสกุลไทย
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// ชื่อกลาง
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// เพศ
    /// </summary>
    public string Gender { get; set; }

    /// <summary>
    /// วันเกิด
    /// </summary>
    public DateOnly BirthDate { get; set; }

    /// <summary>
    /// เลขบัตรประชาชนสำหรับคนไทย เป็นเลข 13 หลัก / passport สำหรับชาวต่างชาติ
    /// </summary>
    public string? NationId { get; set; }


    /// <summary>
    /// หมู่เลือด ABO
    /// </summary>
    public string? BloodGroup { get; set; }


    /// <summary>
    /// ที่อยู่ : บ้านเลขที่ หมู่ ถนน ซอย อาคาร ให้รวมอยู่ในฟิลด์นี้
    /// </summary>
    public string? AddressNo { get; set; }

    /// <summary>
    /// แขวง/ตำบล
    /// </summary>
    public string? SubDistrictId { get; set; }

    /// <summary>
    /// เขต/อำเภอ
    /// </summary>
    public string? DistrictId { get; set; }

    /// <summary>
    /// จังหวัด
    /// </summary>
    public string? ProvinceId { get; set; }

    /// <summary>
    /// รหัสไปรษณีย์
    /// </summary>
    public string? ZipCode { get; set; }

    /// <summary>
    /// เบอร์โทร
    /// </summary>
    public string? TelephoneNumber { get; set; }

    /// <summary>
    /// แพ้ยา
    /// </summary>
    public string? DrugAllergy { get; set; }

    /// <summary>
    /// โรคประจำตัว
    /// </summary>
    public string? ChronicDisease { get; set; }

    /// <summary>
    /// สิทธิการรักษาหลัก
    /// </summary>
    public string? MainClaim { get; set; }

    /// <summary>
    /// การศึกษา
    /// </summary>
    public string? Education { get; set; }

    /// <summary>
    /// อาชีพ
    /// </summary>
    public string? Occupation { get; set; }

    /// <summary>
    /// สูบบุหรี่
    /// </summary>
    public bool? IsSmoke { get; set; }

    /// <summary>
    /// รายละเอียดอื่นๆเกี่ยวกับการสูบบุหรี่
    /// </summary>
    public string? SmokeRemark { get; set; }

    public bool? IsDrink { get; set; }
    public string? DrinkRemark {  get; set; }

    // Navigation properties
    public virtual Province? Province { get; set; }
    public virtual District? District { get; set; }
    public virtual SubDistrict? SubDistrict { get; set; }

   
    public Patient(
        string prefix,
        string firstName,
        string middleName,
        string lastName,
        string gender,
        DateOnly birthDate)
    {
        Prefix = prefix;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Gender = gender;
        BirthDate = birthDate;
    }
}
