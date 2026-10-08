using System.Text.Json;
using BigLion.CPA.Presentation.API.Converters;

namespace BigLion.Application.FunctionalTests.Converters;

// The API sets InvariantCulture globally in Program.cs, but the converter itself
// must not depend on that: on a th-TH machine (Buddhist calendar) a culture-less
// ToString("yyyy-MM-dd") would emit the year as B.E. (e.g. 2530 instead of 1987).
public class JsonDateOnlyConverterTests
{
    private static JsonSerializerOptions Options()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonDateOnlyConverter());
        return options;
    }

    [Test]
    [SetCulture("th-TH")]
    public void Write_UnderThaiCulture_EmitsGregorianIsoDate()
    {
        var json = JsonSerializer.Serialize(new DateOnly(1987, 3, 15), Options());

        json.Should().Be("\"1987-03-15\"");
    }

    [Test]
    [SetCulture("th-TH")]
    public void Read_UnderThaiCulture_ParsesYearAsGregorian()
    {
        var date = JsonSerializer.Deserialize<DateOnly>("\"1987-03-15\"", Options());

        date.Should().Be(new DateOnly(1987, 3, 15));
    }

    [TestCase(1987, 3, 15)]
    [TestCase(2000, 2, 29)]
    [TestCase(1906, 10, 8)]
    [TestCase(2026, 12, 31)]
    [SetCulture("th-TH")]
    public void RoundTrip_UnderThaiCulture_KeepsSameDate(int year, int month, int day)
    {
        var original = new DateOnly(year, month, day);

        var json = JsonSerializer.Serialize(original, Options());
        var back = JsonSerializer.Deserialize<DateOnly>(json, Options());

        back.Should().Be(original);
    }

    [Test]
    [SetCulture("en-US")]
    public void Write_UnderEnglishCulture_EmitsSameIsoDate()
    {
        var json = JsonSerializer.Serialize(new DateOnly(1987, 3, 15), Options());

        json.Should().Be("\"1987-03-15\"");
    }
}