using FluentAssertions;
using NUnit.Framework;
using BigLion.Domain.Entities;

namespace BigLion.Domain.UnitTests.Entity;
public class SubDistrictTests
{
    [Test]
    public void CreateSubDistrictObjectShouldBeOk()
    {
        var address = new SubDistrict("", "3001", "30310", "จอหอ", "Choho", "30310")
        {

        };

        address.Should().NotBeNull();
        address.SubDistrictId.Should().Be("30310");
        address.DistrictId.Should().Be("3001");
        address.Name.Should().Be("จอหอ");
        address.NameEnglish.Should().Be("Choho");
        address.ZipCode.Should().Be("30310");
    }
}
