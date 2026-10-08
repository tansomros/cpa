using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Lookups;
using FluentAssertions;
using NUnit.Framework;

namespace BigLion.CPA.Application.UnitTests.Features.Lookups;

/// <summary>
/// GetLookupOptionsQuery / GetLookupCategoriesQuery over the lifestyle lookups registered
/// in LookupRegistry. There are no .resx files, so every language returns the Thai names.
/// </summary>
public class GetLookupOptionsQueryTests
{
    public static IEnumerable<TestCaseData> ExpectedOptions()
    {
        yield return new TestCaseData(
                "smoking",
                new[] { "Non", "Regular", "Quit" },
                new[] { "ไม่สูบ", "สูบประจำ", "เลิกสูบแล้ว" })
            .SetArgDisplayNames("smoking");
        yield return new TestCaseData(
                "cigarette-type",
                new[] { "Manufactured", "RollYourOwn", "Electronic", "Other" },
                new[] { "บุหรี่ซอง", "ยาเส้นมวนเอง", "บุหรี่ไฟฟ้า", "อื่นๆ" })
            .SetArgDisplayNames("cigarette-type");
        yield return new TestCaseData(
                "drinking",
                new[] { "Non", "Quit", "Occasional", "Regular" },
                new[] { "ไม่ดื่ม", "เคยดื่มแต่เลิกแล้ว", "ดื่มครั้งคราว", "ดื่มประจำ" })
            .SetArgDisplayNames("drinking");
    }

    [Test]
    public async Task GetCategories_ShouldReturnExactlyTheLifestyleCategories()
    {
        var categories = await new GetLookupCategoriesQueryHandler()
            .Handle(new GetLookupCategoriesQuery(), CancellationToken.None);

        categories.Should().BeEquivalentTo("smoking", "cigarette-type", "drinking");
    }

    [TestCaseSource(nameof(ExpectedOptions))]
    public async Task GetOptions_ShouldReturnAllOptionsInSortOrderWithThaiText(
        string category, string[] expectedCodes, string[] expectedThai)
    {
        var options = await GetOptions(category);

        options.Should().HaveCount(expectedCodes.Length);
        options.Select(o => o.Value).Should().Equal(expectedCodes);
        options.Select(o => o.DisplayName).Should().Equal(expectedThai);
    }

    [Test]
    public async Task GetOptions_EveryRegisteredCategory_ShouldReturnUniqueCodesWithThaiText()
    {
        foreach (var category in LookupRegistry.Categories)
        {
            var options = await GetOptions(category);

            options.Should().NotBeEmpty("category {0} is registered", category);
            options.Select(o => o.Value.ToUpperInvariant()).Should().OnlyHaveUniqueItems();
            options.Should().AllSatisfy(o =>
            {
                o.Value.Should().NotBeNullOrWhiteSpace();
                o.DisplayName.Should().MatchRegex(@"\p{IsThai}", "{0}/{1} needs Thai text", category, o.Value);
            });
        }
    }

    [TestCase("SMOKING", "smoking")]
    [TestCase("Smoking", "smoking")]
    [TestCase("CIGARETTE-TYPE", "cigarette-type")]
    [TestCase("Drinking", "drinking")]
    public async Task GetOptions_CategoryName_ShouldBeCaseInsensitive(string category, string canonical)
    {
        var options = await GetOptions(category);

        options.Should().NotBeEmpty();
        options.Should().Equal(await GetOptions(canonical));
    }

    [TestCase("en")]
    [TestCase("ja")]
    [TestCase("th")]
    [TestCase("")]
    [TestCase(null)]
    public async Task GetOptions_AnyLanguage_ShouldFallBackToThai(string? language)
    {
        var options = await GetOptions("smoking", language);

        options.Select(o => o.DisplayName).Should().Equal("ไม่สูบ", "สูบประจำ", "เลิกสูบแล้ว");
    }

    [TestCase("nonexistent")]
    [TestCase("drink-frequency")]
    public async Task GetOptions_UnknownCategory_ShouldThrowNotFoundException(string category)
    {
        await FluentActions.Invoking(() => GetOptions(category))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*LookupCategory*{category}*");
    }

    [TestCase("")]
    [TestCase(" ")]
    public void Validator_EmptyCategory_ShouldFail(string category)
    {
        var result = new GetLookupOptionsQueryValidator().Validate(new GetLookupOptionsQuery { Category = category });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(GetLookupOptionsQuery.Category));
    }

    [Test]
    public void Validator_KnownCategory_ShouldPass()
    {
        new GetLookupOptionsQueryValidator().Validate(new GetLookupOptionsQuery { Category = "smoking" })
            .IsValid.Should().BeTrue();
    }

    private static Task<List<LookupOptionDto>> GetOptions(string category, string? language = null)
        => new GetLookupOptionsQueryHandler().Handle(
            new GetLookupOptionsQuery { Category = category, Language = language },
            CancellationToken.None);
}