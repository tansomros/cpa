using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cpa.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Role(int id,string name)
    {
        Id = id;
        Name = name;
    }
}
