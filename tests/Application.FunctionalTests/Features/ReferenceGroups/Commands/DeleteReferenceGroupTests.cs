#pragma warning disable CS0618
using BigLion.Application.Features.ReferenceGroups.Commands.Delete;
using BigLion.Application.FunctionalTests.Features._Shared;
using BigLion.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.ReferenceGroups.Commands;

public class DeleteReferenceGroupTests : BaseTestFixture
{
    /// ทดสอบ: ลบกลุ่มอ้างอิงที่มีอยู่ ควรลบออกจากฐานข้อมูลสำเร็จ
    [Test]
    public async Task Delete_ExistingGroup_ShouldRemoveFromDatabase()
    {
        RunAsDefaultUser();
        var id = await TestDataFactory.CreateTestReferenceGroupAsync();

        await SendAsync(new DeleteReferenceGroupCommand { Id = id });

        var deleted = await FindAsync<ReferenceGroup>(id);
        deleted.Should().BeNull();
    }

    /// ทดสอบ: ลบกลุ่มอ้างอิงที่ไม่มีในระบบ ควร throw NotFoundException
    [Test]
    public async Task Delete_NonExisting_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new DeleteReferenceGroupCommand { Id = 99999 }))
            .Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }
}
