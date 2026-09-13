using System.Text.Json.Serialization;

namespace Cpa.Domain.ValueObjects;
public class FinalReport
{
    public ICollection<FinalLab> Labs { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    //[SwaggerSchema(Nullable = true)]
    public FinalVision? Vision { get; set; }

    public FinalReport()
    {
        Labs = [];
        Vision = null;
    }
}
