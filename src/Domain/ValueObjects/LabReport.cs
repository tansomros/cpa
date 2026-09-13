using System.Text.Json.Serialization;

namespace Cpa.Domain.ValueObjects;
public class LabReport
{

    public ICollection<LabStructure> Labs { get; set; }

    public LabReport()
    {
        Labs = []; 
    }
}
