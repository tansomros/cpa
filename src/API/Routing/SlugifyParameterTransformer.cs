using System.Text.RegularExpressions;

namespace Cpa.Presentation.API.Routing;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object? value)
    {
        return value == null
            ? string.Empty
            : Regex.Replace(
                value.ToString() ?? string.Empty,
                "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100)).ToLowerInvariant();
    }
}
