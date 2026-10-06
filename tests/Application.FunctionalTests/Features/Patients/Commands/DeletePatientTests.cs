using BigLion.CPA.Application.Features.Patients.Commands.Create;
using BigLion.CPA.Application.Features.Patients.Commands.Delete;
using BigLion.CPA.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Patients.Commands;

public class DeletePatientTests : BaseTestFixture
{
    [Test]
    public async Task Delete_ExistingPatient_ShouldRemoveFromDatabase()
    {
        RunAsDefaultUser();

        var id = await SendAsync(new CreatePatientCommand
        {
            ForeName = "จะลบ",
            Surname = "ทดสอบ",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Parse("1990-01-01"))
        });

        await SendAsync(new DeletePatientCommand { Id = id });

        var deleted = await FindAsync<Patient>(id);
        deleted.Should().BeNull();
    }

    [Test]
    public async Task Delete_NonExisting_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new DeletePatientCommand { Id = 99999 }))
            .Should().ThrowAsync<BigLion.CPA.Application.Exceptions.NotFoundException>();
    }
}
