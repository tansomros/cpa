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
}
