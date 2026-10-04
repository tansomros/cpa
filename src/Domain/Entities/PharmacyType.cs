namespace BigLion.CPA.Domain.Entities;

public class PharmacyType : BaseEntity
{ 

    public string Code { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public int Sort { get; set; }
    public PharmacyType(string code,string name,int sort)
    {
        Code = code;
        Name = name;
        Sort = sort;
    }
}

