using BigLion.CPA.Application.Features.Pharmacy.Commands.Create;
using BigLion.CPA.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;
using ValidationException = BigLion.CPA.Application.Exceptions.ValidationException;

namespace BigLion.Application.FunctionalTests.Features.Pharmacies.Commands;

public class CreatePharmacyTests : BaseTestFixture
{
    [Test]
    public async Task Create_WithValidData_ShouldPersistPharmacy()
    {
        RunAsAdmin();
        await PharmacyTestData.SeedAddressesAsync();

        var id = await SendAsync(new CreatePharmacyCommand
        {
            Code = "PHA-001",
            Name = "ร้านยาชุมชน",
            LicenseNo = "LIC-001",
            ProvinceId = PharmacyTestData.ProvinceId,
            DistrictId = PharmacyTestData.DistrictId,
            SubDistrictId = PharmacyTestData.SubDistrictId,
        });

        var pharmacy = await FindAsync<Pharmacy>(id);
        pharmacy.Should().NotBeNull();
        pharmacy!.Code.Should().Be("PHA-001");
        pharmacy.Name.Should().Be("ร้านยาชุมชน");
        pharmacy.LicenseNo.Should().Be("LIC-001");
        pharmacy.SubDistrictId.Should().Be(PharmacyTestData.SubDistrictId);
        pharmacy.IsActive.Should().BeTrue();
    }

    [Test]
    public async Task Create_WithEmptyCode_ShouldThrowValidationException()
    {
        RunAsAdmin();

        await FluentActions.Invoking(() => SendAsync(new CreatePharmacyCommand
        {
            Code = "",
            Name = "ร้านยาชุมชน",
        })).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_WithEmptyName_ShouldThrowValidationException()
    {
        RunAsAdmin();

        await FluentActions.Invoking(() => SendAsync(new CreatePharmacyCommand
        {
            Code = "PHA-001",
            Name = "  ",
        })).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_WithDistrictOutsideProvince_ShouldThrowValidationException()
    {
        RunAsAdmin();
        await PharmacyTestData.SeedAddressesAsync();

        await FluentActions.Invoking(() => SendAsync(new CreatePharmacyCommand
        {
            Code = "PHA-001",
            Name = "ร้านยาชุมชน",
            ProvinceId = PharmacyTestData.ProvinceId,
            DistrictId = PharmacyTestData.OtherDistrictId,
        })).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_WithUnknownPharmacyGroup_ShouldThrowValidationException()
    {
        RunAsAdmin();

        await FluentActions.Invoking(() => SendAsync(new CreatePharmacyCommand
        {
            Code = "PHA-001",
            Name = "ร้านยาชุมชน",
            PharmacyGroupId = 99999,
        })).Should().ThrowAsync<ValidationException>();
    }
}
