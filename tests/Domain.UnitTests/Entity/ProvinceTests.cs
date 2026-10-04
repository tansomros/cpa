using FluentAssertions;
using NUnit.Framework;
using BigLion.CPA.Domain.Entities;

namespace BigLion.CPA.Domain.UnitTests.Entity;
public class ProvinceTests
{
    [Test]
    public void CreateProvinceObjectShouldBeOk()
    {
        var address = new Province("","30", "นครราชสีมา", "Nakhon Ratchasima")
        {

        };

        address.Should().NotBeNull();
        address.Id.Should().Be("30");
        address.Name.Should().Be("นครราชสีมา");
        address.NameEnglish.Should().Be("Nakhon Ratchasima");
    }
}
