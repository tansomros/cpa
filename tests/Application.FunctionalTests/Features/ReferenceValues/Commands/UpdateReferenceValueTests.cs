#pragma warning disable CS0618
using BigLion.Application.Features.ReferenceValues.Commands.Update;
using BigLion.Application.FunctionalTests.Features._Shared;
using BigLion.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.ReferenceValues.Commands;

public class UpdateReferenceValueTests : BaseTestFixture
{
    /// ทดสอบ: อัปเดตค่าอ้างอิงที่มีอยู่ ควรบันทึกข้อมูลใหม่สำเร็จ
    [Test]
    public async Task Update_Existing_ShouldUpdateFields()
    {
        RunAsDefaultUser();
        var groupId = await TestDataFactory.CreateTestReferenceGroupAsync();
        var id = await TestDataFactory.CreateTestReferenceValueAsync(groupId, "OLD", "เก่า");

        await SendAsync(new UpdateReferenceValueCommand
        {
            Id = id,
            ValueCode = "NEW",
            Descriptions = "ใหม่",
            ReferenceGroupId = groupId,
            Sort = 5
        });

        var updated = await FindAsync<ReferenceValue>(id);
        updated.Should().NotBeNull();
        updated!.ValueCode.Should().Be("NEW");
    }

    /// ทดสอบ: อัปเดตค่าอ้างอิงที่ไม่มีในระบบ ควร throw ValidationException
    [Test]
    public async Task Update_NonExisting_ShouldThrowValidationException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new UpdateReferenceValueCommand
        {
            Id = 99999,
            ValueCode = "X",
            Descriptions = "X",
            ReferenceGroupId = 1,
            Sort = 1
        })).Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }
}
