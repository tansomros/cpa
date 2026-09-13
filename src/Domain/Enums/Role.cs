using Kondongpu.Domain.Common;

namespace Kondongpu.Domain.Enums;

/// <summary>
/// สิทธิ์ User
/// </summary>
public sealed class Role : SmartEnum<Role>
{
    public static readonly Role Administrator = new("1","A", "ADMINISTRATOR",1);
    public static readonly Role Officer = new("2","O", "OFFICER", 2);
    public static readonly Role Manager = new("3","M", "MANAGER",3);
    public static readonly Role User = new("4","U", "USER",4);

    private Role(string value,string code,string name, int sort) : base(value,code,name, sort) { }
}
