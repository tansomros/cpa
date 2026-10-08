using BigLion.CPA.Domain.Common;

namespace BigLion.CPA.Application.Features.Lookups;

public static class LookupRegistry
{
    private static readonly Dictionary<string, Func<string?, List<LookupOptionDto>>> _lookups =
        new(StringComparer.OrdinalIgnoreCase)
        {
            // Register CPA lookups here, e.g. ["smoking-statuses"] = lang => Map(SmokingStatus.All, lang),
        };

    public static List<string> Categories => _lookups.Keys.ToList();

    public static List<LookupOptionDto>? Get(string category, string? language = null)
        => _lookups.TryGetValue(category, out var factory) ? factory(language) : null;

    private static List<LookupOptionDto> Map<T>(IReadOnlyList<T> items, string? language) where T : SmartEnum<T>
        => items.Select(e => new LookupOptionDto(e.Value, e.GetDisplayName(language))).ToList();
}
