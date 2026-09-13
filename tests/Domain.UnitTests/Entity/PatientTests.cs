using FluentAssertions;
using NUnit.Framework;
using BigLion.Domain.Entities;
using BigLion.Domain.ValueObjects;
namespace BigLion.Domain.UnitTests.Entity;
public class PatientTests
{
    [Test]
    public void CreatePatientObjectShouldBeOk()
    {
        var item = new Patient("11111111", "นาย", "รักชาติ", "", "สุรนารี", Gender.Male, DateOnly.FromDateTime(Convert.ToDateTime("1982-01-01")))
        {
            NationId = null,
            Nationality = "ไทย",
            Religious = null,
            BloodGroup = null,
            EmployeeId = null,           
            Address = null,
            SubDistrictId = null,
            DistrictId = null,
            ProvinceId = null,
            ZipCode = null,
            TelephoneNumber = null,
            DrugAllergy = null,
        };

        item.Should().NotBeNull();
        item.HospitalNumber.Should().Be("11111111");
        item.Prefix.Should().Be("นาย");
        item.FirstName.Should().Be("รักชาติ");
        item.LastName.Should().Be("สุรนารี");
        item.Gender.Should().Be(Gender.Male);
        item.BirthDate.Should().Be(DateOnly.FromDateTime(Convert.ToDateTime("1982-01-01")));
        item.NationId.Should().BeNull();
        item.Nationality.Should().Be("ไทย");
        item.Religious.Should().BeNull();
        item.BloodGroup.Should().BeNull();
        item.EmployeeId.Should().BeNull();
        item.Address.Should().BeNull();
        item.SubDistrictId.Should().BeNull();
        item.ProvinceId.Should().BeNull();
        item.ZipCode.Should().BeNull();
        item.TelephoneNumber.Should().BeNull();
        item.DrugAllergy.Should().BeNull();
        item.IsActive.Should().Be(false);
    }
}
