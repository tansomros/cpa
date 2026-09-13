using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cpa.Domain.Entities;

public class Running
{   
    public string Code { get; set; }
    public int YearCode { get; set; }
    public int LastRunning { get; set; }

    public Running(string code, int yearCode,int lastRunning)
    {
        Code = code;
        YearCode = yearCode;
        LastRunning = lastRunning;
    }
}
