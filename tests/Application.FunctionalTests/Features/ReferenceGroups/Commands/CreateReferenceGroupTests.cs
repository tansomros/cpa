#pragma warning disable CS0618
using BigLion.Application.Features.ReferenceGroups.Commands.Create;
using BigLion.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;
using ValidationException = BigLion.Application.Exceptions.ValidationException;

namespace BigLion.Application.FunctionalTests.Features.ReferenceGroups.Commands;

public class CreateReferenceGroupTests : BaseTestFixture
{
    /// ทดสอบ: สร้างกลุ่มอ้างอิงใหม่ด้วยข้อมูลที่ถูกต้อง ควรคืนค่า Id ที่มากกว่า 0
    [Test]
    public async Task Create_WithValidData_ShouldReturnPositiveId()
    {
        RunAsDefaultUser();

        var command = new CreateReferenceGroupCommand
        {
            Code = "BLOOD",
            Descriptions = "กลุ่มเลือด",
            Sort = 1
        };

        var result = await SendAsync(command);
        result.Should().BeGreaterThan(0);
    }

    /// ทดสอบ: สร้างกลุ่มอ้างอิงแล้วตรวจสอบข้อมูลที่บันทึก
    [Test]
    public async Task Create_WithValidData_ShouldPersistEntity()
    {
        RunAsDefaultUser();

        var id = await SendAsync(new CreateReferenceGroupCommand
        {
            Code = "GENDER",
            Descriptions = "เพศ",
            Sort = 2
        });

        var entity = await FindAsync<ReferenceGroup>(id);
        entity.Should().NotBeNull();
        entity!.Code.Should().Be("GENDER");
        entity.Descriptions.Should().Be("เพศ");
    }

    /// ทดสอบ: สร้างกลุ่มอ้างอิงโดยไม่ระบุรหัส ควร throw ValidationException
    [Test]
    public async Task Create_WithEmptyCode_ShouldThrowValidationException()
    {
        RunAsDefaultUser();

        var command = new CreateReferenceGroupCommand
        {
            Code = "",
            Descriptions = "ทดสอบ",
            Sort = 1
        };

        var act = () => SendAsync(command);
        (await act.Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().ContainKey("Code")
            .WhoseValue.Should().Contain("Code ต้องไม่เป็นค่าว่าง");
    }
}
