using BigLion.CPA.Domain.Common;
using BigLion.CPA.Domain.Enums;

namespace BigLion.CPA.Application.Features.Lookups;

public static class LookupRegistry
{
    private static readonly Dictionary<string, Func<string?, List<LookupOptionDto>>> _lookups =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["smoking"] = lang => Map(SmokingValue.All, lang),
            ["cigarette-type"] = lang => Map(CigaretteTypeValue.All, lang),
            ["drinking"] = lang => Map(DrinkingValue.All, lang),
            ["drink-frequency"] = lang => Map(DrinkFrequencyValue.All, lang),
        };

    public static List<string> Categories => _lookups.Keys.ToList();

    public static List<LookupOptionDto>? Get(string category, string? language = null)
        => _lookups.TryGetValue(category, out var factory) ? factory(language) : null;

    private static List<LookupOptionDto> Map<T>(IReadOnlyList<T> items, string? language) where T : SmartEnum<T>
        => items.Select(e => new LookupOptionDto(e.Value, e.GetDisplayName(language))).ToList();
}
