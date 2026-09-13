#pragma warning disable CS0618
using BigLion.Application.Features.ReferenceValues.Commands.Delete;
using BigLion.Application.FunctionalTests.Features._Shared;
using BigLion.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.ReferenceValues.Commands;

public class DeleteReferenceValueTests : BaseTestFixture
{
    /// ทดสอบ: ลบค่าอ้างอิงที่มีอยู่ ควรลบออกจากฐานข้อมูลสำเร็จ
    [Test]
    public async Task Delete_Existing_ShouldRemoveFromDatabase()
    {
        RunAsDefaultUser();
        var groupId = await TestDataFactory.CreateTestReferenceGroupAsync();
        var id = await TestDataFactory.CreateTestReferenceValueAsync(groupId);

        await SendAsync(new DeleteReferenceValueCommand { Id = id });

        var deleted = await FindAsync<ReferenceValue>(id);
        deleted.Should().BeNull();
    }

    /// ทดสอบ: ลบค่าอ้างอิงที่ไม่มีในระบบ ควร throw NotFoundException
    [Test]
    public async Task Delete_NonExisting_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new DeleteReferenceValueCommand { Id = 99999 }))
            .Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }
}
