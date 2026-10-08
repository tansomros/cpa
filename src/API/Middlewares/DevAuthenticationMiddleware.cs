using System.Security.Claims;

namespace BigLion.CPA.Presentation.API.Middlewares;

/// <summary>
/// Development bypass middleware — ใช้แทน JWT Bearer authentication ตอน dev บน internet
/// ที่ไม่สามารถเชื่อมต่อ Identity Server (identity.suth.go.th) ได้
///
/// เปิดใช้งานโดยตั้งค่า "Identity:UseDevelopmentBypass": true ใน appsettings.Development.json
/// ข้อมูลผู้ใช้จะอ่านจาก "Identity:DevelopmentUser" — ต้องใส่ค่าจริงจากฐานข้อมูล
/// เช่น LoginName และ EmployeeId ต้องตรงกับผู้ใช้ที่มีอยู่จริงในฐานข้อมูล เพื่อให้ CurrentUserService ทำงานได้ถูกต้อง
///
/// Middleware นี้จะสร้าง ClaimsPrincipal พร้อม claims ทั้งหมดที่ CurrentUserService ต้องการ
/// และ scope ที่ authorization policies ต้องการ แล้ว set ลง HttpContext.User
/// ก่อนที่ UseAuthentication() จะทำงาน
/// </summary>
public class DevAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public DevAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var devUserSection = _configuration.GetSection("Identity:DevelopmentUser");

        var nameIdentifier = devUserSection["NameIdentifier"] ?? "dev-user-001";
        var name = devUserSection["Name"] ?? "Developer (Dev Bypass)";
        var employeeId = devUserSection["EmployeeId"] ?? "DEV001";
        var loginName = devUserSection["LoginName"] ?? "devuser";
        var doctorCode = devUserSection["DoctorCode"] ?? "DEV-DOC";
        var position = devUserSection["Position"] ?? "Developer";
        var hasAdminRole = devUserSection.GetValue("HasAdminRole", true);
        var hasDoctorRole = devUserSection.GetValue("HasDoctorRole", true);
        var hasNurseRole = devUserSection.GetValue("HasNurseRole", false);
        var isBeCheckupGroup = devUserSection.GetValue("IsBeCheckupGroup", true);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, nameIdentifier),
            new(ClaimTypes.Name, name),
            new("EmployeeId", employeeId),
            new("LoginName", loginName),
            new("DoctorCode", doctorCode),
            new("Position", position),
            new("scope", "suth-checkup-api"),
            new("scope", "openid"),
            new("scope", "profile"),
            new("scope", "offline_access"),
            new("scope", "license"),
            new("scope", "employee_id"),
        };

        if (hasAdminRole)
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        if (hasDoctorRole)
            claims.Add(new Claim(ClaimTypes.Role, "User : แพทย์ (Doctor)"));
        if (hasNurseRole)
            claims.Add(new Claim(ClaimTypes.Role, "User : พยาบาล (Nurse)"));
        if (isBeCheckupGroup)
            claims.Add(new Claim(ClaimTypes.Role, "User : ตรวจสุขภาพ(CheckUp)"));

        // authenticationType ต้องไม่เป็น null เพื่อให้ IsAuthenticated = true
        var identity = new ClaimsIdentity(claims, "DevBypass");
        context.User = new ClaimsPrincipal(identity);

        await _next(context);
    }
}

public static class DevAuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseDevAuthentication(this IApplicationBuilder app)
        => app.UseMiddleware<DevAuthenticationMiddleware>();
}
