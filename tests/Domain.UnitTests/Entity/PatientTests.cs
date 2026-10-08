using BigLion.CPA.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace BigLion.CPA.Domain.UnitTests.Entity;

public class PatientTests
{
    [Test]
    public void CreatePatientObjectShouldBeOk()
    {
        var birthDate = DateOnly.FromDateTime(Convert.ToDateTime("1982-01-01"));
        var item = new Patient("รักชาติ", "สุรนารี", "1103700000000", "M", birthDate);

        item.Should().NotBeNull();
        item.ForeName.Should().Be("รักชาติ");
        item.Surname.Should().Be("สุรนารี");
        item.Gender.Should().Be("M");
        item.BirthDate.Should().Be(birthDate);
        item.CardId.Should().Be("1103700000000");
        item.IsActive.Should().BeFalse();
    }

    [Test]
    public void UpdateMethodsShouldAssignGroupedFields()
    {
        var item = new Patient("รักชาติ", "สุรนารี", "1103700000000");

        item.UpdatePersonalInformation("สมชาย", "ใจดี", "M", new DateOnly(1982, 1, 1), "1103700000001");
        item.UpdateContact("021111111", "09:00-17:00");
        item.UpdateAddress("บ้าน", "99/1", "สุขุมวิท", "1001", "100101", "10", "10110");
        item.UpdateGeneralInformation("สิทธิหลัก", true, "ปริญญาตรี", "พนักงาน");
        item.UpdateAllergy(true, "Penicillin");
        item.UpdateSmokingHistory("Regular", 10, 5, "Manufactured", false, "สูบทุกวัน");
        item.UpdateAlcoholHistory("Occasional", 3);

        item.ForeName.Should().Be("สมชาย");
        item.Surname.Should().Be("ใจดี");
        item.CardId.Should().Be("1103700000001");
        item.Telephone.Should().Be("021111111");
        item.TimeContact.Should().Be("09:00-17:00");
        item.AddressNo.Should().Be("99/1");
        item.DistrictId.Should().Be("1001");
        item.SubDistrictId.Should().Be("100101");
        item.ProvinceId.Should().Be("10");
        item.MainClaim.Should().Be("สิทธิหลัก");
        item.Education.Should().Be("ปริญญาตรี");
        item.IsAllergy.Should().BeTrue();
        item.DrugAllergy.Should().Be("Penicillin");
        item.Smoke.Should().Be("Regular");
        item.SmokingRemark.Should().Be("สูบทุกวัน");
        item.Drinking.Should().Be("Occasional");
        item.DrinkFrequency.Should().Be(3);
    }
}
