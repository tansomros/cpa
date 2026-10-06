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
        var item = new Patient("รักชาติ", "สุรนารี", null, Gender.Male, birthDate);

        item.Should().NotBeNull();
        item.ForeName.Should().Be("รักชาติ");
        item.Surname.Should().Be("สุรนารี");
        item.Gender.Should().Be(Gender.Male);
        item.BirthDate.Should().Be(birthDate);
        item.CardId.Should().BeNull();
        item.IsActive.Should().BeFalse();
    }

    [Test]
    public void UpdateMethodsShouldAssignGroupedFields()
    {
        var item = new Patient("รักชาติ", "สุรนารี", "1103700000000");

        item.UpdatePersonalInformation("สมชาย", "ใจดี", Gender.Male, new DateOnly(1982, 1, 1), null, "1103700000001");
        item.UpdateContact("021111111", "0891111111", "09:00-17:00");
        item.UpdateAddress("บ้าน", "99/1", "สุขุมวิท", "1001", "คลองเตย", "10", "กรุงเทพมหานคร", "10110");
        item.UpdateGeneralInformation("สิทธิหลัก", 1, "ปริญญาตรี", "พนักงาน");
        item.UpdateAllergy(true, "Penicillin");
        item.UpdateSmokingHistory(true, 1, 10, 5, 2, false, "สูบทุกวัน");
        item.UpdateAlcoholHistory(1, 3);

        item.ForeName.Should().Be("สมชาย");
        item.Surname.Should().Be("ใจดี");
        item.CardId.Should().Be("1103700000001");
        item.Telephone.Should().Be("021111111");
        item.Mobile.Should().Be("0891111111");
        item.TimeContact.Should().Be("09:00-17:00");
        item.AddressNo.Should().Be("99/1");
        item.DistrictId.Should().Be("1001");
        item.ProvinceName.Should().Be("กรุงเทพมหานคร");
        item.MainClaim.Should().Be("สิทธิหลัก");
        item.Education.Should().Be("ปริญญาตรี");
        item.IsAllergy.Should().BeTrue();
        item.DrugAllergy.Should().Be("Penicillin");
        item.IsSmoke.Should().BeTrue();
        item.SmokingRemark.Should().Be("สูบทุกวัน");
        item.Alcohol.Should().Be(1);
        item.AlcoholFQ.Should().Be(3);
    }
}
