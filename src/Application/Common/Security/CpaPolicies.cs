namespace Cpa.Application.Common.Security;
public static class CpaPolicies
{
    public const string AllowAnonymous = "AllowAnonymous"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ไม่ต้องระบุตัวตน
    public const string RequireAuthenticatedUser = "RequiredAuthenticatedUser"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องระบุตัวตน
    public const string RequirePharmacy = "RequirePharmacy"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องเป็น ร้านยา
    public const string RequireReporter = "RequireReporter"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องเป็น Reporter
    public const string RequireManager = "RequireManager"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องเป็น Manager
    public const string RequireHost = "RequireHost"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องเป็น ผู้ดูแลระบบ
    public const string RequireAdmin = "RequireAdmin"; // อนุญาตเข้าใช้งาน feature ได้โดยที่ต้องเป็น admin

    public const string CanCreate = "CanCreate";
    public const string CanEdit = "CanEdit";
    public const string CanDelete = "CanDelete";
    public const string CanView = "CanView";
}
