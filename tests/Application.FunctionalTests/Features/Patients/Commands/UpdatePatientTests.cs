using BigLion.CPA.Application.Features.Patients.Commands.Create;
using BigLion.CPA.Application.Features.Patients.Commands.Update;
using BigLion.CPA.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Patients.Commands;

public class UpdatePatientTests : BaseTestFixture
{
    [Test]
    public async Task Update_ExistingPatient_ShouldUpdateFields()
    {
        RunAsDefaultUser();

        var id = await SendAsync(new CreatePatientCommand
        {
            ForeName = "เดิม",
            Surname = "ชื่อเก่า",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Parse("1990-01-01"))
        });

        await SendAsync(new UpdatePatientCommand
        {
            Id = id,
            ForeName = "ใหม่",
            Surname = "ชื่อใหม่",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Parse("1990-01-01")),
            Mobile = "0899999999",
            AddressNo = "88/8",
            IsSmoke = true,
            SmokingRemark = "เลิกแล้ว"
        });

        var updated = await FindAsync<Patient>(id);
        updated.Should().NotBeNull();
        updated!.Id.Should().Be(id);
        updated.ForeName.Should().Be("ใหม่");
        updated.Surname.Should().Be("ชื่อใหม่");
        updated.Mobile.Should().Be("0899999999");
        updated.AddressNo.Should().Be("88/8");
        updated.IsSmoke.Should().BeTrue();
        updated.SmokingRemark.Should().Be("เลิกแล้ว");
    }

    [Test]
    public async Task Update_NonExisting_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new UpdatePatientCommand
        {
            Id = 99999,
            ForeName = "X",
            Surname = "X",
            Gender = "M",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        })).Should().ThrowAsync<BigLion.CPA.Application.Exceptions.NotFoundException>();
    }
}
