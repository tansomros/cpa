using BigLion.CPA.Application.Features.Pharmacy.Commands.Delete;
using BigLion.CPA.Application.Features.Pharmacy.Queries.Get;
using BigLion.CPA.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;
using NotFoundException = BigLion.CPA.Application.Exceptions.NotFoundException;

namespace BigLion.Application.FunctionalTests.Features.Pharmacies.Commands;

public class DeletePharmacyTests : BaseTestFixture
{
    [Test]
    public async Task Delete_ExistingPharmacy_ShouldSoftDelete()
    {
        RunAsAdmin();
        var id = await PharmacyTestData.CreatePharmacyAsync();
        var current = await SendAsync(new GetPharmacyQuery { Id = id });

        await SendAsync(new DeletePharmacyCommand { Id = id, RowVersion = current.RowVersion });

        var pharmacy = await FindAsync<Pharmacy>(id);
        pharmacy.Should().NotBeNull();
        pharmacy!.IsActive.Should().BeFalse();
        pharmacy.DeleteFlag.Should().BeTrue();
    }

    [Test]
    public async Task Delete_NonExistingPharmacy_ShouldThrowNotFoundException()
    {
        RunAsAdmin();

        await FluentActions.Invoking(() => SendAsync(new DeletePharmacyCommand { Id = 99999, RowVersion = 1 }))
            .Should().ThrowAsync<NotFoundException>();
    }
}
