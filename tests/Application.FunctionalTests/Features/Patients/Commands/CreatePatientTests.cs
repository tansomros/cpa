using BigLion.Application.Features.Patients.Commands.Create;
using BigLion.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;
using ValidationException = BigLion.Application.Exceptions.ValidationException;

namespace BigLion.Application.FunctionalTests.Features.Patients.Commands;

public class CreatePatientTests : BaseTestFixture
{
    /// ทดสอบ: สร้างผู้ป่วยใหม่ด้วยข้อมูลที่ถูกต้อง ควรคืนค่า Id ที่มากกว่า 0
    [Test]
    public async Task Create_WithValidData_ShouldReturnPositiveId()
    {
        RunAsDefaultUser();

        var command = new CreatePatientCommand
        {
            HospitalNumber = "HN990001",
            Prefix = "นาย",
            FirstName = "สมชาย",
            MiddleName = "",
            LastName = "ใจดี",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Parse("1985-06-15")),
            Nationality = "ไทย"
        };

        var result = await SendAsync(command);
        result.Should().BeGreaterThan(0);
    }

    /// ทดสอบ: สร้างผู้ป่วยแล้วตรวจสอบว่าข้อมูลถูกบันทึกอย่างถูกต้อง
    [Test]
    public async Task Create_WithValidData_ShouldPersistEntity()
    {
        RunAsDefaultUser();

        var command = new CreatePatientCommand
        {
            HospitalNumber = "HN990002",
            Prefix = "นาง",
            FirstName = "สมหญิง",
            MiddleName = "",
            LastName = "ใจดี",
            Gender = "F",
            BirthDate = DateOnly.FromDateTime(DateTime.Parse("1990-03-20")),
            TelephoneNumber = "0891234567"
        };

        var id = await SendAsync(command);

        var entity = await FindAsync<Patient>(id);
        entity.Should().NotBeNull();
        entity!.HospitalNumber.Should().Be("HN990002");
        entity.FirstName.Should().Be("สมหญิง");
        entity.Gender.Should().Be("F");
    }

    /// ทดสอบ: สร้างผู้ป่วยโดยไม่ระบุ HospitalNumber ควร throw ValidationException
    [Test]
    public async Task Create_WithEmptyHospitalNumber_ShouldThrowValidationException()
    {
        RunAsDefaultUser();

        var command = new CreatePatientCommand
        {
            HospitalNumber = "",
            Prefix = "นาย",
            FirstName = "ทดสอบ",
            MiddleName = "",
            LastName = "ระบบ",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var act = () => SendAsync(command);
        (await act.Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().ContainKey("HospitalNumber")
            .WhoseValue.Should().Contain("HN ต้องไม่ว่าง");
    }

    /// ทดสอบ: สร้างผู้ป่วยโดยไม่ระบุชื่อ ควร throw ValidationException
    [Test]
    public async Task Create_WithEmptyFirstName_ShouldThrowValidationException()
    {
        RunAsDefaultUser();

        var command = new CreatePatientCommand
        {
            HospitalNumber = "HN990003",
            Prefix = "นาย",
            FirstName = "",
            MiddleName = "",
            LastName = "ระบบ",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var act = () => SendAsync(command);
        (await act.Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().ContainKey("FirstName")
            .WhoseValue.Should().Contain("FirstName ไม่สามารถเป็นค่าว่างได้");
    }
}
