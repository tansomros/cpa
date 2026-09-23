namespace Cpa.Domain.Entities;

public class UserRoles
{
    public int RoleID { get; set; }

    public int UserID { get; set; }

    public int? isActive { get; set; }

    public string? UpdBy { get; set; }

    public DateTime? UpdDate { get; set; }
}

