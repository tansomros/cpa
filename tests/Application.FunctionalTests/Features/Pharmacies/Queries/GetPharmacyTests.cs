using BigLion.CPA.Application.Features.Pharmacy.Queries.Get;
using static BigLion.Application.FunctionalTests.Testing;
using NotFoundException = BigLion.CPA.Application.Exceptions.NotFoundException;

namespace BigLion.Application.FunctionalTests.Features.Pharmacies.Queries;

public class GetPharmacyTests : BaseTestFixture
{
    [Test]
    public async Task Get_ExistingPharmacy_ShouldReturnViewModelWithRowVersion()
    {
        RunAsAdmin();
        var id = await PharmacyTestData.CreatePharmacyAsync("PHA-001", "ร้านยาชุมชน");

        var result = await SendAsync(new GetPharmacyQuery { Id = id });

        result.Id.Should().Be(id);
        result.Code.Should().Be("PHA-001");
        result.Name.Should().Be("ร้านยาชุมชน");
        result.RowVersion.Should().NotBe(0u);
    }

    [Test]
    public async Task Get_NonExistingPharmacy_ShouldThrowNotFoundException()
    {
        RunAsAdmin();

        await FluentActions.Invoking(() => SendAsync(new GetPharmacyQuery { Id = 99999 }))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task List_WithSearch_ShouldReturnMatchingPage()
    {
        RunAsAdmin();
        await PharmacyTestData.CreatePharmacyAsync("PHA-001", "ร้านยาชุมชน");
        await PharmacyTestData.CreatePharmacyAsync("PHA-002", "ร้านยาหมอเอ");
        await PharmacyTestData.CreatePharmacyAsync("XYZ-003", "ร้านยาหมอบี");

        var result = await SendAsync(new GetPharmaciesQuery { Search = "PHA", Page = 1, Limit = 10 });

        result.TotalCount.Should().Be(2);
        result.Items.Select(p => p.Code).Should().BeEquivalentTo(["PHA-001", "PHA-002"]);
        result.Items.Should().OnlyContain(p => p.RowVersion != 0u);
    }
}
