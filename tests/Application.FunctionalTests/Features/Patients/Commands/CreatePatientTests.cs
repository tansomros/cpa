using BigLion.CPA.Application.Features.Patients.Commands.Create;
using BigLion.CPA.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;
using ValidationException = BigLion.CPA.Application.Exceptions.ValidationException;

namespace BigLion.Application.FunctionalTests.Features.Patients.Commands;

public class CreatePatientTests : BaseTestFixture
{
    [Test]
    public async Task Create_WithValidData_ShouldReturnPositiveId()
    {
        RunAsDefaultUser();

        var command = new CreatePatientCommand
        {
            ForeName = "สมชาย",
            Surname = "ใจดี",
            Gender = "M",
            BirthDate = new DateOnly(1985, 6, 15),
            CardId = "1103700990001"
        };

        var result = await SendAsync(command);
        result.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task Create_WithValidData_ShouldPersistEntity()
    {
        RunAsDefaultUser();

        var command = new CreatePatientCommand
        {
            ForeName = "สมหญิง",
            Surname = "ใจดี",
            Gender = "F",
            BirthDate = new DateOnly(1990, 3, 20),
            Telephone = "0891234567",
            DrugAllergy = "Penicillin",
            IsAllergy = true
        };

        var id = await SendAsync(command);

        var entity = await FindAsync<Patient>(id);
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(id);
        entity.ForeName.Should().Be("สมหญิง");
        entity.Gender.Should().Be("F");
        entity.Telephone.Should().Be("0891234567");
        entity.IsAllergy.Should().BeTrue();
        entity.DrugAllergy.Should().Be("Penicillin");
    }

    [Test]
    public async Task Create_WithEmptyForeName_ShouldThrowValidationException()
    {
        RunAsDefaultUser();

        var command = new CreatePatientCommand
        {
            ForeName = "",
            Surname = "ระบบ",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var act = () => SendAsync(command);
        (await act.Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().ContainKey("ForeName")
            .WhoseValue.Should().Contain("ชื่อต้องไม่ว่าง");
    }
}
