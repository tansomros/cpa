using BigLion.CPA.Application.Features.Lookups;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Lookups;

public class GetLookupOptionsTests : BaseTestFixture
{
    /// ทดสอบ: ดึงค่า lookup หมวดที่ไม่มี ควร throw NotFoundException
    [Test]
    public async Task GetOptions_InvalidCategory_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() =>
            SendAsync(new GetLookupOptionsQuery { Category = "nonexistent" }))
            .Should().ThrowAsync<BigLion.CPA.Application.Exceptions.NotFoundException>();
    }

    /// ทดสอบ: Category name เป็น case-insensitive
    [Test]
    public async Task GetOptions_CaseInsensitive_ShouldWork()
    {
        RunAsDefaultUser();

        var options = await SendAsync(new GetLookupOptionsQuery { Category = "SMOKING" });

        options.Select(o => o.Value).Should().Equal("Non", "Regular", "Quit");
    }

    /// ทดสอบ: ไม่มีไฟล์ .resx ภาษาใดก็ตาม (en, ja) ควร fallback กลับเป็นชื่อภาษาไทย
    [TestCase("en")]
    [TestCase("ja")]
    public async Task GetOptions_AnyLanguage_ShouldFallbackToThai(string language)
    {
        RunAsDefaultUser();

        var options = await SendAsync(new GetLookupOptionsQuery { Category = "smoking", Language = language });

        options.Should().HaveCount(3);
        options.Should().Contain(o => o.Value == "Non" && o.DisplayName == "ไม่สูบ");
        options.Should().Contain(o => o.Value == "Regular" && o.DisplayName == "สูบประจำ");
        options.Should().Contain(o => o.Value == "Quit" && o.DisplayName == "เลิกสูบแล้ว");
    }
}