namespace Cpa.Domain.Entities;

/// <summary>
/// ข้อมูลผู้รับบริการ
/// </summary>
public class Patient : BaseEntity
{
    /// <summary>
    /// HN : Hospital Number เลขที่ประจำตัวผู้ป่วย
    /// </summary>
    public string HospitalNumber { get; set; }

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
    /// สัญชาติ
    /// </summary>
    public string? Nationality { get; set; }

    /// <summary>
    /// ศาสนา
    /// </summary>
    public string? Religious { get; set; }

    /// <summary>
    /// หมู่เลือด ABO
    /// </summary>
    public string? BloodGroup { get; set; }

    /// <summary>
    /// รหัสพนักงาน
    /// </summary>
    public string? EmployeeId { get; set; }

    /// <summary>
    /// สังกัดบริษัท/หน่วยงาน <= ไม่ใช้แบบนี้แล้ว ไปเชื่อมกับ PatientCompany
    /// </summary>
    //public int? CompanyId { get; set; }
    //public virtual Company? Company { get; set; }

    /// <summary>
    /// ที่อยู่ : บ้านเลขที่ หมู่ ถนน ซอย อาคาร ให้รวมอยู่ในฟิลด์นี้
    /// </summary>
    public string? Address { get; set; }

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
    /// คำนำหน้าชื่อภาษาอังกฤษ
    /// </summary>
    public string? PrefixEnglish { get; set; }

    /// <summary>
    /// ชื่ออังกฤษ
    /// </summary>
    public string? FirstNameEnglish { get; set; }

    /// <summary>
    /// นามสกุลอังกฤษ
    /// </summary>
    public string? LastNameEnglish { get; set; }

    /// <summary>
    /// ชื่อกลาง
    /// </summary>
    public string? MiddleNameEnglish { get; set; }

    /// <summary>
    /// สัญชาติ
    /// </summary>
    public string? NationalityEnglish { get; set; }

    /// <summary>
    /// ศาสนา
    /// </summary>
    public string? ReligiousEnglish { get; set; }

    /// <summary>
    /// ที่อยู่ : บ้านเลขที่ หมู่ ถนน ซอย อาคาร ให้รวมอยู่ในฟิลด์นี้
    /// </summary>
    public string? AddressEnglish { get; set; }

    //public ICollection<Checkup>? Checkups { get; set; }

    // Navigation properties
    public virtual Province? Province { get; set; }
    public virtual District? District { get; set; }
    public virtual SubDistrict? SubDistrict { get; set; }

    /// <summary>
    /// กำหนดค่าตั้งต้นที่จำเป็นของข้อมูล ผู้รับบริการ
    /// </summary>
    /// <param name="hospitalNumber">เลขที่ผู้รับบริการ 8 หลัก</param>
    /// <param name="prefix">คำนำหน้าชื่อ</param>
    /// <param name="firstName">ชื่อ</param>
    /// <param name="middleName">ชื่อกลาง</param>
    /// <param name="lastName">นามสกุล</param>
    /// <param name="gender">เพศ</param>
    /// <param name="birthDate">วันเดือนปีเกิด (ค.ส.)</param>
    public Patient(
        string hospitalNumber,
        string prefix,
        string firstName,
        string middleName,
        string lastName,
        string gender,
        DateOnly birthDate)
    {
        HospitalNumber = hospitalNumber;
        Prefix = prefix;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Gender = gender;
        BirthDate = birthDate;
    }
}
