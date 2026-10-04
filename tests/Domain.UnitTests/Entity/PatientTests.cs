using BigLion.CPA.Domain.Entities;
using BigLion.CPA.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace BigLion.CPA.Domain.UnitTests.Entity;

public class PatientTests
{
    [Test]
    public void CreatePatientObjectShouldBeOk()
    {
        var birthDate = DateOnly.FromDateTime(Convert.ToDateTime("1982-01-01"));
        var item = new Patient("นาย", "รักชาติ", "", "สุรนารี", Gender.Male, birthDate)
        {
            NationId = null,
            BloodGroup = null,
            AddressNo = null,
            SubDistrictId = null,
            DistrictId = null,
            ProvinceId = null,
            ZipCode = null,
            TelephoneNumber = null,
            DrugAllergy = null,
        };

        item.Should().NotBeNull();
        item.Prefix.Should().Be("นาย");
        item.FirstName.Should().Be("รักชาติ");
        item.LastName.Should().Be("สุรนารี");
        item.Gender.Should().Be(Gender.Male);
        item.BirthDate.Should().Be(birthDate);
        item.NationId.Should().BeNull();
        item.BloodGroup.Should().BeNull();
        item.AddressNo.Should().BeNull();
        item.SubDistrictId.Should().BeNull();
        item.ProvinceId.Should().BeNull();
        item.ZipCode.Should().BeNull();
        item.TelephoneNumber.Should().BeNull();
        item.DrugAllergy.Should().BeNull();
        item.IsActive.Should().BeFalse();
    }
}
