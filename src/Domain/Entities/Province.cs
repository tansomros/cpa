using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cpa.Domain.Entities;

public class Province
{
    public string Id { get; set; } 
    public string Name { get; set; }
    public string NameEnglish { get; set; }
    public string Region { get; set; }
    public int? ProvinceGroupId { get; set; }
    public virtual ProvinceGroup? ProvinceGroup { get; set; }
    public ICollection<District> Districts { get; set; }

    public Province(string region, string id,string name,string nameEnglish)
    {
        Id = id;    
        Name = name;
        NameEnglish = nameEnglish;
        Region = region;
        Districts = [];
       
    }
}
