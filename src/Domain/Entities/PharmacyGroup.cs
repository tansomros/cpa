namespace Cpa.Domain.Entities;

public class PharmacyGroup : BaseEntity
{
    public string Code { get; set; } 

    public string Name { get; set; }

    public string? Description { get; set; }

    public int Sort { get; set; }
    public PharmacyGroup(string code,string name,int sort)
    {
        Code = code;
        Name = name;
        Sort = sort;        
    }
}

