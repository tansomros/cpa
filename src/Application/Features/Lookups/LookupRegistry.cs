using Cpa.Domain.Common;
using Cpa.Domain.Enums;

namespace Cpa.Application.Features.Lookups;

public static class LookupRegistry
{
    private static readonly Dictionary<string, Func<string?, List<LookupOptionDto>>> _lookups =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["exam-results"] = lang => Map(ExamResult.All, lang),
            ["lab-results"] = lang => Map(LabResult.All, lang),
            ["xray-results"] = lang => Map(XrayResult.All, lang),
            ["bmd-results"] = lang => Map(BmdResult.All, lang),
            ["abi-results"] = lang => Map(AbiResult.All, lang),
            ["checkup-statuses"] = lang => Map(CheckupStatus.All, lang),
            ["eye-results"] = lang => Map(EyeResult.All, lang),
            ["hearing-loss-levels"] = lang => Map(HearingLossLevel.All, lang),
            ["vision-acuity-results"] = lang => Map(VisionAcuityResult.All, lang),
        };

    public static List<string> Categories => _lookups.Keys.ToList();

    public static List<LookupOptionDto>? Get(string category, string? language = null)
        => _lookups.TryGetValue(category, out var factory) ? factory(language) : null;

    private static List<LookupOptionDto> Map<T>(IReadOnlyList<T> items, string? language) where T : SmartEnum<T>
        => items.Select(e => new LookupOptionDto(e.Value, e.GetDisplayName(language))).ToList();
}
