using BigLion.Domain.Enums;
using BigLion.Application.Features.Lookups;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Lookups;

public class GetLookupOptionsTests : BaseTestFixture
{
    /// ทดสอบ: ดึงรายการหมวดหมู่ lookup ทั้งหมด ควรคืนรายการที่ไม่ว่าง
    [Test]
    public async Task GetCategories_ShouldReturnAllCategories()
    {
        RunAsDefaultUser();

        var categories = await SendAsync(new GetLookupCategoriesQuery());

        categories.Should().NotBeEmpty();
        categories.Should().Contain("exam-results");
        categories.Should().Contain("lab-results");
        categories.Should().Contain("hearing-loss-levels");
    }

    /// ทดสอบ: ดึงค่า lookup หมวด exam-results ควรคืน Normal, Abnormal, ไม่ได้ตรวจ
    [Test]
    public async Task GetOptions_ExamResults_ShouldReturnExpectedValues()
    {
        RunAsDefaultUser();

        var options = await SendAsync(new GetLookupOptionsQuery { Category = "exam-results" });

        options.Should().HaveCount(3);
        options.Should().Contain(o => o.Value == "Normal");
        options.Should().Contain(o => o.Value == "Abnormal");
        options.Should().Contain(o => o.Value == "ไม่ได้ตรวจ");
    }

    /// ทดสอบ: ดึงค่า lookup หมวด hearing-loss-levels ควรคืน 5 ระดับ
    [Test]
    public async Task GetOptions_HearingLossLevels_ShouldReturnFiveLevels()
    {
        RunAsDefaultUser();

        var options = await SendAsync(new GetLookupOptionsQuery { Category = "hearing-loss-levels" });

        options.Should().HaveCount(5);
        options.Select(o => o.Value).Should().ContainInOrder(
            "ปกติ", "เสียเล็กน้อย", "เสียปานกลาง", "เสียมาก", "เสียรุนแรง");
    }

    /// ทดสอบ: ดึงค่า lookup หมวดที่ไม่มี ควร throw NotFoundException
    [Test]
    public async Task GetOptions_InvalidCategory_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() =>
            SendAsync(new GetLookupOptionsQuery { Category = "nonexistent" }))
            .Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }

    /// ทดสอบ: SmartEnum.FromValue round-trip ควรคืนค่าที่ถูกต้อง
    [Test]
    public void SmartEnum_FromValue_ShouldRoundTrip()
    {
        var normal = ExamResult.FromValue("Normal");
        normal.Should().BeSameAs(ExamResult.Normal);
        normal.DisplayName.Should().Be("ปกติ (Normal)");
    }

    /// ทดสอบ: SmartEnum.FromValue ด้วยค่าที่ไม่มี ควร throw InvalidOperationException
    [Test]
    public void SmartEnum_FromValue_InvalidValue_ShouldThrow()
    {
        FluentActions.Invoking(() => ExamResult.FromValue("INVALID"))
            .Should().Throw<InvalidOperationException>();
    }

    /// ทดสอบ: Category name เป็น case-insensitive
    [Test]
    public async Task GetOptions_CaseInsensitive_ShouldWork()
    {
        RunAsDefaultUser();

        var options = await SendAsync(new GetLookupOptionsQuery { Category = "EXAM-RESULTS" });

        options.Should().NotBeEmpty();
    }

    /// ทดสอบ: ดึงค่า lookup หมวด exam-results ด้วย lang=en ควรคืน English display names
    [Test]
    public async Task GetOptions_ExamResults_English_ShouldReturnEnglishDisplayNames()
    {
        RunAsDefaultUser();

        var options = await SendAsync(new GetLookupOptionsQuery { Category = "exam-results", Language = "en" });

        options.Should().HaveCount(3);
        options.Should().Contain(o => o.Value == "Normal" && o.DisplayName == "Normal");
        options.Should().Contain(o => o.Value == "Abnormal" && o.DisplayName == "Abnormal");
        options.Should().Contain(o => o.Value == "ไม่ได้ตรวจ" && o.DisplayName == "Not Examined");
    }

    /// ทดสอบ: ดึงค่า lookup หมวด hearing-loss-levels ด้วย lang=en ควรคืน English display names
    [Test]
    public async Task GetOptions_HearingLossLevels_English_ShouldReturnEnglishDisplayNames()
    {
        RunAsDefaultUser();

        var options = await SendAsync(new GetLookupOptionsQuery { Category = "hearing-loss-levels", Language = "en" });

        options.Should().HaveCount(5);
        options.Should().Contain(o => o.Value == "ปกติ" && o.DisplayName == "Normal");
        options.Should().Contain(o => o.Value == "เสียเล็กน้อย" && o.DisplayName == "Mild Hearing Loss");
        options.Should().Contain(o => o.Value == "เสียรุนแรง" && o.DisplayName == "Profound Hearing Loss");
    }

    /// ทดสอบ: ดึงค่า lookup ด้วยภาษาที่ไม่รองรับ ควร fallback กลับเป็นค่า Thai default
    [Test]
    public async Task GetOptions_UnsupportedLanguage_ShouldFallbackToThai()
    {
        RunAsDefaultUser();

        var options = await SendAsync(new GetLookupOptionsQuery { Category = "exam-results", Language = "ja" });

        options.Should().HaveCount(3);
        options.Should().Contain(o => o.Value == "Normal" && o.DisplayName == "ปกติ (Normal)");
        options.Should().Contain(o => o.Value == "Abnormal" && o.DisplayName == "ผิดปกติ (Abnormal)");
    }

    /// ทดสอบ: SmartEnum.GetDisplayName ควรคืนค่า localized display name ตามภาษา
    [Test]
    public void SmartEnum_GetDisplayName_ShouldReturnLocalizedName()
    {
        ExamResult.Normal.GetDisplayName().Should().Be("ปกติ (Normal)");
        ExamResult.Normal.GetDisplayName("en").Should().Be("Normal");
        ExamResult.Abnormal.GetDisplayName("en").Should().Be("Abnormal");
    }
}
