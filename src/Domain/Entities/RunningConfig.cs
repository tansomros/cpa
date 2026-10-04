using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigLion.CPA.Domain.Entities;

public class RunningConfig
{
    public string Code { get; set; }
    public string Description { get; set; }
    public bool IsCode { get; set; }
    public bool IsRef { get; set; }
    public int DigitCount { get; set; }
    public string? TemplateCode { get; set; }

    public RunningConfig(string code,string description,bool isCode,bool isRef,int digitCount)
    {
        Code = code;
        Description = description;
        IsCode = isCode;
        IsRef = isRef;
        DigitCount = digitCount;
    }
}
