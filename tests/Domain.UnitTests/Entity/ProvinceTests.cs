using FluentAssertions;
using NUnit.Framework;
using BigLion.Domain.Entities;

namespace BigLion.Domain.UnitTests.Entity;
public class ProvinceTests
{
    [Test]
    public void CreateProvinceObjectShouldBeOk()
    {
        var address = new Province("","30", "นครราชสีมา", "Nakhon Ratchasima")
        {

        };

        address.Should().NotBeNull();
        address.ProvinceId.Should().Be("30");
        address.Name.Should().Be("นครราชสีมา");
        address.NameEnglish.Should().Be("Nakhon Ratchasima");
    }
}
