using BigLion.CPA.Application.Features.Patients.Commands.Create;
using BigLion.CPA.Application.Features.Patients.Queries.Get;
using BigLion.CPA.Domain.ValueObjects;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Patients.Queries;

public class GetPatientByCardIdTests : BaseTestFixture
{
    [Test]
    public async Task Get_WithExistingCardId_ShouldReturnPatient()
    {
        RunAsDefaultUser();

        await SendAsync(new CreatePatientCommand
        {
            ForeName = "ค้นหาด้วยบัตร",
            Surname = "ทดสอบ",
            Gender = Gender.Male,
            BirthDate = DateOnly.FromDateTime(DateTime.Parse("1990-01-01")),
            CardId = "1103700880001"
        });

        var result = await SendAsync(new GetPatientByCardIdQuery { CardId = "1103700880001" });

        result.Should().NotBeNull();
        result.CardId.Should().Be("1103700880001");
        result.ForeName.Should().Be("ค้นหาด้วยบัตร");
    }

    [Test]
    public async Task Get_WithNonExistingCardId_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() =>
            SendAsync(new GetPatientByCardIdQuery { CardId = "0000000000000" }))
            .Should().ThrowAsync<BigLion.CPA.Application.Exceptions.NotFoundException>();
    }
}
