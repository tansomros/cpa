using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigLion.CPA.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public int Sort {  get; set; }
    public Role(int id,string name,bool isActive,int sort)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        Sort = sort;
    }
}
