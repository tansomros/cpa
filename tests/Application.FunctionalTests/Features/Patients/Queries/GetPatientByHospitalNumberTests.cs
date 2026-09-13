using BigLion.Application.Features.Patients.Commands.Create;
using BigLion.Application.Features.Patients.Queries.Get;
using BigLion.Domain.ValueObjects;

using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Patients.Queries;

public class GetPatientByHospitalNumberTests : BaseTestFixture
{
    /// <summary>
    /// ทดสอบ: ค้นหาผู้ป่วยด้วยเลขโรงพยาบาลที่มีอยู่ ควรคืนข้อมูลผู้ป่วยที่ถูกต้อง
    /// </summary>
    [Test]
    public async Task Get_WithExistingHospitalNumber_ShouldReturnPatient()
    {
        RunAsDefaultUser();

        await SendAsync(new CreatePatientCommand
        {
            HospitalNumber = "HNFND001",
            Prefix = "นาย",
            FirstName = "ค้นหาด้วยHN",
            MiddleName = "",
            LastName = "ทดสอบ",
            Gender = Gender.Male,
            BirthDate = DateOnly.FromDateTime(DateTime.Parse("1990-01-01"))
        });

        var result = await SendAsync(new GetPatientByHospitalNumberQuery { HospitalNumber = "HNFND001" });

        result.Should().NotBeNull();
        result.HospitalNumber.Should().Be("HNFND001");
        result.FirstName.Should().Be("ค้นหาด้วยHN");
    }

    /// <summary>
    /// ทดสอบ: ค้นหาผู้ป่วยด้วยเลขโรงพยาบาลที่ไม่มีอยู่ ควร throw NotFoundException
    /// </summary>
    [Test]
    public async Task Get_WithNonExistingHospitalNumber_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() =>
            SendAsync(new GetPatientByHospitalNumberQuery { HospitalNumber = "NONEXIST" }))
            .Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }
}
