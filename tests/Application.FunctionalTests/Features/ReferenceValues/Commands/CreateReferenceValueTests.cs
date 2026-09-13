#pragma warning disable CS0618
using BigLion.Application.Features.ReferenceValues.Commands.Create;
using BigLion.Application.FunctionalTests.Features._Shared;
using BigLion.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.ReferenceValues.Commands;

public class CreateReferenceValueTests : BaseTestFixture
{
    /// ทดสอบ: สร้างค่าอ้างอิงใหม่ด้วยข้อมูลที่ถูกต้อง ควรคืนค่า Id ที่มากกว่า 0
    [Test]
    public async Task Create_WithValidData_ShouldReturnPositiveId()
    {
        RunAsDefaultUser();
        var groupId = await TestDataFactory.CreateTestReferenceGroupAsync();

        var command = new CreateReferenceValueCommand
        {
            ValueCode = "MALE",
            Descriptions = "ชาย",
            ReferenceGroupId = groupId,
            Sort = 1
        };

        var result = await SendAsync(command);
        result.Should().BeGreaterThan(0);
    }

    /// ทดสอบ: สร้างค่าอ้างอิงแล้วตรวจสอบข้อมูลที่บันทึก
    [Test]
    public async Task Create_WithValidData_ShouldPersistEntity()
    {
        RunAsDefaultUser();
        var groupId = await TestDataFactory.CreateTestReferenceGroupAsync();

        var id = await SendAsync(new CreateReferenceValueCommand
        {
            ValueCode = "FEMALE",
            Descriptions = "หญิง",
            ReferenceGroupId = groupId,
            Sort = 2
        });

        var entity = await FindAsync<ReferenceValue>(id);
        entity.Should().NotBeNull();
        entity!.ValueCode.Should().Be("FEMALE");
    }
}
