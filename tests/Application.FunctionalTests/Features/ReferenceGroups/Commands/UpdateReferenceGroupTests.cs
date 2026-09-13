#pragma warning disable CS0618
using BigLion.Application.Features.ReferenceGroups.Commands.Update;
using BigLion.Application.FunctionalTests.Features._Shared;
using BigLion.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.ReferenceGroups.Commands;

public class UpdateReferenceGroupTests : BaseTestFixture
{
    /// ทดสอบ: อัปเดตกลุ่มอ้างอิงที่มีอยู่ ควรบันทึกข้อมูลใหม่สำเร็จ
    [Test]
    public async Task Update_ExistingGroup_ShouldUpdateFields()
    {
        RunAsDefaultUser();
        var id = await TestDataFactory.CreateTestReferenceGroupAsync("OLD", "เก่า");

        await SendAsync(new UpdateReferenceGroupCommand
        {
            Id = id,
            Code = "NEW",
            Descriptions = "ใหม่",
            Sort = 5
        });

        var updated = await FindAsync<ReferenceGroup>(id);
        updated.Should().NotBeNull();
        updated!.Code.Should().Be("NEW");
        updated.Descriptions.Should().Be("ใหม่");
    }

    /// ทดสอบ: อัปเดตกลุ่มอ้างอิงที่ไม่มีในระบบ ควร throw ValidationException
    [Test]
    public async Task Update_NonExisting_ShouldThrowValidationException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new UpdateReferenceGroupCommand
        {
            Id = 99999,
            Code = "X",
            Descriptions = "X",
            Sort = 1
        })).Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }
}
