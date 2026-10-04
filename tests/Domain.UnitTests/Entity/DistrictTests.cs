using FluentAssertions;
using NUnit.Framework;
using BigLion.CPA.Domain.Entities;

namespace BigLion.CPA.Domain.UnitTests.Entity;
public class DistrictTests
{
    [Test]
    public void CreateDistrictObjectShouldBeOk()
    {
        var address = new District("30", "3001", "เมืองนครราชสีมา", "Muang")
        {
        };

        address.Should().NotBeNull();
        address.ProvinceId.Should().Be("30");
        address.Id.Should().Be("3001");
        address.Name.Should().Be("เมืองนครราชสีมา");
        address.NameEnglish.Should().Be("Muang");
    }
}
