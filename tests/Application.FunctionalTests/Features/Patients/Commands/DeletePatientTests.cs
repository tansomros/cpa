using BigLion.Application.Features.Patients.Commands.Create;
using BigLion.Application.Features.Patients.Commands.Delete;
using BigLion.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Patients.Commands;

public class DeletePatientTests : BaseTestFixture
{
    /// ทดสอบ: ลบผู้ป่วยที่มีอยู่ ควรลบออกจากฐานข้อมูลสำเร็จ
    [Test]
    public async Task Delete_ExistingPatient_ShouldRemoveFromDatabase()
    {
        RunAsDefaultUser();

        var id = await SendAsync(new CreatePatientCommand
        {
            HospitalNumber = "HNDEL001",
            Prefix = "นาย",
            FirstName = "จะลบ",
            MiddleName = "",
            LastName = "ทดสอบ",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Parse("1990-01-01"))
        });

        await SendAsync(new DeletePatientCommand { Id = id });

        var deleted = await FindAsync<Patient>(id);
        deleted.Should().BeNull();
    }

    /// ทดสอบ: ลบผู้ป่วยที่ไม่มีในระบบ ควร throw NotFoundException
    [Test]
    public async Task Delete_NonExisting_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new DeletePatientCommand { Id = 99999 }))
            .Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }
}
