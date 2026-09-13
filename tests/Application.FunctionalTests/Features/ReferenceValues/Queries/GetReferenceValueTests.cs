#pragma warning disable CS0618
using BigLion.Application.Features.ReferenceValues.Queries.Get;
using BigLion.Application.FunctionalTests.Features._Shared;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.ReferenceValues.Queries;

public class GetReferenceValueTests : BaseTestFixture
{
    /// ทดสอบ: ค้นหาค่าอ้างอิงด้วย Id ที่มีอยู่ ควรคืนข้อมูลที่ถูกต้อง
    [Test]
    public async Task Get_ExistingId_ShouldReturnViewModel()
    {
        RunAsDefaultUser();
        var groupId = await TestDataFactory.CreateTestReferenceGroupAsync();
        var id = await TestDataFactory.CreateTestReferenceValueAsync(groupId, "TEST", "ทดสอบ");

        var result = await SendAsync(new GetReferenceValueQuery { Id = id });

        result.Should().NotBeNull();
        result.ValueCode.Should().Be("TEST");
    }

    /// ทดสอบ: ค้นหาค่าอ้างอิงด้วย Id ที่ไม่มี ควร throw NotFoundException
    [Test]
    public async Task Get_NonExisting_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new GetReferenceValueQuery { Id = 99999 }))
            .Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }
}
