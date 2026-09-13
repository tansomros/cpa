namespace Cpa.Domain.ValueObjects;
public class CheckupGroupValue
{
    public static readonly string VitalSigns = "VS";
    /// <summary>
    /// ลักษณะภายนอกทั่วไป
    /// </summary>
    public static readonly string GeneralApprearance = "GA";
    /// <summary>
    /// การตรวจตา
    /// </summary>
    public static readonly string VisionScreening = "VA";
    /// <summary>
    /// การตรวจการได้ยิน
    /// </summary>
    public static readonly string Audiogram = "AU";
    public static readonly string Dental = "DENT";
    public static readonly string Lung = "Lung";
    /// <summary>
    /// ความเข้มข้นของเลือด
    /// </summary> 
    public static readonly string Hematocite = "HCT";
    /// <summary>
    /// จำนวนเม็ดเลือดขาวและแยกชนิด
    /// </summary>
    public static readonly string WhiteBloodCell = "WBC";
    /// <summary>
    /// Complete Blood Count
    /// </summary>
    public static readonly string CBC = "CBC";
    /// <summary>
    /// เกล็ดเลือด (Platelet Count)
    /// </summary>
    public static readonly string PlateletCount = "PLC";
    /// <summary>
    /// น้ำตาลในเลือด
    /// </summary>
    public static readonly string BloodSugar = "FBS";
    /// <summary>
    /// ระดับไขมันในเลือด (Lipid Profile)
    /// </summary>
    public static readonly string Lipid = "LP";
    /// <summary>
    /// ระดับกรดยูริค (Uric Acid)
    /// </summary>
    public static readonly string UricAcid = "UR";
    /// <summary>
    /// การทำงานของไต (Renal Function)
    /// </summary>
    public static readonly string Renal = "RFT";
    /// <summary>
    /// การทำงานของตับ (Liver Function)
    /// </summary>
    public static readonly string Liver = "LFT";
    /// <summary>
    /// Blood Chemistry
    /// </summary>
    public static readonly string BloodChemistry = "BC";

    /// <summary>
    /// การตรวจปัสสาวะ (Urine Analysis)
    /// </summary>
    public static readonly string Urine = "UA";
    /// <summary>
    /// ตรวจอุจจาระ (Stool Examination)
    /// </summary>
    public static readonly string StoolExam = "ST";
    /// <summary>
    /// Stool Culture and Sensitivity
    /// </summary>
    public static readonly string StoolCulture = "LM";
    /// <summary>
    /// Special Test and Other Lab
    /// </summary>
    public static readonly string SpecialTest = "SP";
    /// <summary>
    /// การตรวจสารพิษในเลือด
    /// </summary>
    public static readonly string ToxicMetalBlood = "TX";
    /// <summary>
    /// การตรวจหาเชื้อ-ภูมิคุ้มกันต่อไวรัสตับอักเสบ บี
    /// </summary>
    public static readonly string Hepatitis = "HB";

    /// <summary>
    /// การตรวจหาระดับไทรอยด์ในเลือด
    /// </summary>
    public static readonly string Thyroid = "TSH";
    /// <summary>
    /// การตรวจหามะเร็งและสารบ่งชี้มะเร็ง  (Cancer Marker)
    /// </summary>
    public static readonly string CancerMarker = "CA";
    /// <summary>
    /// X-ray
    /// </summary>
    public static readonly string Xray = "X";
    /// <summary>
    /// การตรวจอื่นๆ
    /// </summary>
    public static readonly string OtherLab = "O"; 
    /// <summary>
    /// การตรวจภายใน/สูติ-นารีเวช
    /// </summary>
    public static readonly string PelvicExam = "W";
    /// <summary>
    /// โรคติดต่อ
    /// </summary>
    public static readonly string Contagious = "CD";

    public static readonly string BodyComposition = "BCA";


}
