using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cpa.Domain.Entities;

public class Prefix
{    
    public int Id { get; set; }
    public string Name { get; set; }
    public Prefix(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
