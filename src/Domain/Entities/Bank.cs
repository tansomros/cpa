using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cpa.Domain.Entities;

public class Bank
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    public Bank(int id,string code,string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }

}
