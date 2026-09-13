#pragma warning disable CS0618
using BigLion.Application.Features.ReferenceGroups.Queries.Get;
using BigLion.Application.FunctionalTests.Features._Shared;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.ReferenceGroups.Queries;

public class GetReferenceGroupTests : BaseTestFixture
{
    /// ทดสอบ: ค้นหากลุ่มอ้างอิงด้วย Id ที่มีอยู่ ควรคืนข้อมูลที่ถูกต้อง
    [Test]
    public async Task Get_ExistingId_ShouldReturnViewModel()
    {
        RunAsDefaultUser();
        var id = await TestDataFactory.CreateTestReferenceGroupAsync("TEST", "กลุ่มทดสอบ");

        var result = await SendAsync(new GetReferenceGroupQuery { Id = id });

        result.Should().NotBeNull();
        result.Code.Should().Be("TEST");
    }

    /// ทดสอบ: ค้นหากลุ่มอ้างอิงด้วย Id ที่ไม่มี ควร throw NotFoundException
    [Test]
    public async Task Get_NonExisting_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new GetReferenceGroupQuery { Id = 99999 }))
            .Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }
}
