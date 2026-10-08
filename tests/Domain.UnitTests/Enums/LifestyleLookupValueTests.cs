using BigLion.CPA.Domain.Common;
using BigLion.CPA.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace BigLion.CPA.Domain.UnitTests.Enums;

/// <summary>
/// Lifestyle lookups (SmokingValue, CigaretteTypeValue, DrinkingValue).
/// LookupRegistry lives in Application and is not reachable from this project, so each
/// type is listed here; Application.UnitTests checks the registry itself.
/// These types have no English names (no .resx), so only the Thai Name is checked.
/// </summary>
public class LifestyleLookupValueTests
{
    public sealed record LookupItem(string Value, string Name, int Sort);

    public static IEnumerable<TestCaseData> Lookups()
    {
        yield return Case<SmokingValue>("Non", "Regular", "Quit");
        yield return Case<CigaretteTypeValue>("Manufactured", "RollYourOwn", "Electronic", "Other");
        yield return Case<DrinkingValue>("Non", "Quit", "Occasional", "Regular");
    }

    private static TestCaseData Case<T>(params string[] expectedCodes) where T : SmartEnum<T>
    {
        var items = SmartEnum<T>.All.Select(e => new LookupItem(e.Value, e.Name, e.Sort)).ToList();
        Func<string, string> fromValue = code => SmartEnum<T>.FromValue(code).Value;
        return new TestCaseData(items, expectedCodes, fromValue).SetArgDisplayNames(typeof(T).Name);
    }

    [TestCaseSource(nameof(Lookups))]
    public void Values_ShouldBeExactlyTheAgreedCodesInSortOrder(
        List<LookupItem> items, string[] expectedCodes, Func<string, string> fromValue)
    {
        items.Select(i => i.Value).Should().Equal(expectedCodes);
        items.Select(i => i.Sort).Should().BeInAscendingOrder();
    }

    [TestCaseSource(nameof(Lookups))]
    public void Values_ShouldBeUniqueIgnoringCase(
        List<LookupItem> items, string[] expectedCodes, Func<string, string> fromValue)
    {
        items.Select(i => i.Value.ToUpperInvariant()).Should().OnlyHaveUniqueItems();
    }

    [TestCaseSource(nameof(Lookups))]
    public void Sorts_ShouldBeUnique(
        List<LookupItem> items, string[] expectedCodes, Func<string, string> fromValue)
    {
        items.Select(i => i.Sort).Should().OnlyHaveUniqueItems();
    }

    [TestCaseSource(nameof(Lookups))]
    public void Names_ShouldBeNonEmptyThai(
        List<LookupItem> items, string[] expectedCodes, Func<string, string> fromValue)
    {
        items.Should().AllSatisfy(i =>
        {
            i.Name.Should().NotBeNullOrWhiteSpace();
            i.Name.Should().MatchRegex(@"\p{IsThai}", "item {0} needs a Thai display name", i.Value);
        });
    }

    [TestCaseSource(nameof(Lookups))]
    public void FromValue_ShouldFindEveryCodeIgnoringCase(
        List<LookupItem> items, string[] expectedCodes, Func<string, string> fromValue)
    {
        foreach (var code in expectedCodes)
        {
            fromValue(code).Should().Be(code);
            fromValue(code.ToUpperInvariant()).Should().Be(code);
            fromValue(code.ToLowerInvariant()).Should().Be(code);
        }
    }

    [TestCaseSource(nameof(Lookups))]
    public void FromValue_UnknownCode_ShouldThrow(
        List<LookupItem> items, string[] expectedCodes, Func<string, string> fromValue)
    {
        FluentActions.Invoking(() => fromValue("XYZ")).Should().Throw<InvalidOperationException>();
    }
}