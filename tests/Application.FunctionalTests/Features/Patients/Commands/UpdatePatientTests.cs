using BigLion.Application.Features.Patients.Commands.Create;
using BigLion.Application.Features.Patients.Commands.Update;
using BigLion.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Patients.Commands;

public class UpdatePatientTests : BaseTestFixture
{
    /// ทดสอบ: อัปเดตข้อมูลผู้ป่วยที่มีอยู่ ควรบันทึกข้อมูลใหม่สำเร็จ
    [Test]
    public async Task Update_ExistingPatient_ShouldUpdateFields()
    {
        RunAsDefaultUser();

        var id = await SendAsync(new CreatePatientCommand
        {
            HospitalNumber = "HN010001",
            Prefix = "นาย",
            FirstName = "เดิม",
            MiddleName = "",
            LastName = "ชื่อเก่า",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Parse("1990-01-01"))
        });

        await SendAsync(new UpdatePatientCommand
        {
            Id = id,
            HospitalNumber = "HN010001",
            Prefix = "นาย",
            FirstName = "ใหม่",
            MiddleName = "",
            LastName = "ชื่อใหม่",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Parse("1990-01-01")),
            TelephoneNumber = "0899999999"
        });

        var updated = await FindAsync<Patient>(id);
        updated.Should().NotBeNull();
        updated!.FirstName.Should().Be("ใหม่");
        updated.LastName.Should().Be("ชื่อใหม่");
        updated.TelephoneNumber.Should().Be("0899999999");
    }

    /// ทดสอบ: อัปเดตผู้ป่วยที่ไม่มีในระบบ ควร throw NotFoundException
    [Test]
    public async Task Update_NonExisting_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new UpdatePatientCommand
        {
            Id = 99999,
            HospitalNumber = "HN99999",
            Prefix = "นาย",
            FirstName = "X",
            MiddleName = "",
            LastName = "X",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        })).Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }
}
