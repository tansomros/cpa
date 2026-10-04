using BigLion.CPA.Application.Common.Exceptions;
using BigLion.CPA.Application.Features.Pharmacy.Commands.Update;
using BigLion.CPA.Application.Features.Pharmacy.Queries.Get;
using BigLion.CPA.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;
using NotFoundException = BigLion.CPA.Application.Exceptions.NotFoundException;
using ValidationException = BigLion.CPA.Application.Exceptions.ValidationException;

namespace BigLion.Application.FunctionalTests.Features.Pharmacies.Commands;

public class UpdatePharmacyTests : BaseTestFixture
{
    [Test]
    public async Task Update_ExistingPharmacy_ShouldUpdateFields()
    {
        RunAsAdmin();
        var id = await PharmacyTestData.CreatePharmacyAsync();
        var current = await SendAsync(new GetPharmacyQuery { Id = id });

        await SendAsync(new UpdatePharmacyCommand
        {
            Id = id,
            RowVersion = current.RowVersion,
            Code = "PHA-002",
            Name = "ร้านยาใหม่",
            Office_Tel = "021234567",
            IsActive = true,
        });

        var updated = await FindAsync<Pharmacy>(id);
        updated!.Code.Should().Be("PHA-002");
        updated.Name.Should().Be("ร้านยาใหม่");
        updated.Office_Tel.Should().Be("021234567");
    }

    [Test]
    public async Task Update_WithStaleRowVersion_ShouldThrowConflictException()
    {
        RunAsAdmin();
        var id = await PharmacyTestData.CreatePharmacyAsync();
        var original = await SendAsync(new GetPharmacyQuery { Id = id });

        await SendAsync(new UpdatePharmacyCommand
        {
            Id = id,
            RowVersion = original.RowVersion,
            Code = "PHA-001",
            Name = "แก้ไขครั้งแรก",
            IsActive = true,
        });

        await FluentActions.Invoking(() => SendAsync(new UpdatePharmacyCommand
        {
            Id = id,
            RowVersion = original.RowVersion,
            Code = "PHA-001",
            Name = "แก้ไขครั้งที่สอง",
            IsActive = true,
        })).Should().ThrowAsync<ConflictException>();
    }

    [Test]
    public async Task Update_NonExistingPharmacy_ShouldThrowNotFoundException()
    {
        RunAsAdmin();

        await FluentActions.Invoking(() => SendAsync(new UpdatePharmacyCommand
        {
            Id = 99999,
            RowVersion = 1,
            Code = "PHA-001",
            Name = "ร้านยาชุมชน",
            IsActive = true,
        })).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task Update_WithEmptyName_ShouldThrowValidationException()
    {
        RunAsAdmin();
        var id = await PharmacyTestData.CreatePharmacyAsync();

        await FluentActions.Invoking(() => SendAsync(new UpdatePharmacyCommand
        {
            Id = id,
            RowVersion = 1,
            Code = "PHA-001",
            Name = "",
            IsActive = true,
        })).Should().ThrowAsync<ValidationException>();
    }
}
