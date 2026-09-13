namespace Cpa.Application.Common.Security;
public static class CpaPolicies
{
    public const string AllowAnonymous = "AllowAnonymous"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ไม่ต้องระบุตัวตน
    public const string RequireAuthenticatedUser = "RequiredAuthenticatedUser"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องระบุตัวตน
    public const string RequireDoctor = "RequireDoctor"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องเป็น แพทย์
    public const string RequireNurse = "RequireNurse"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องเป็น พยาบาล
    public const string RequireAdmin = "RequireAdmin"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องเป็น ผู้ดูแลระบบ
    public const string RequireEmployee = "RequireEmployee"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องเป็น พนักงาน

    public const string CanCreate = "CanCreate";
    public const string CanEdit = "CanEdit";
    public const string CanDelete = "CanDelete";
    public const string CanView = "CanView";
}
