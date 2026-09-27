namespace Cpa.Domain.Entities;

public class ProvinceGroup
{
    public string Id { get; set; } 

    public string Name { get; set; }

    public ProvinceGroup(string id, string name)
    {
        Id = id;
        Name = name; 
    }

}

