using BigLion.CPA.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace BigLion.CPA.Domain.UnitTests.Entity;

public class PharmacyTests
{
    [Test]
    public void Constructor_SetsCodeAndName()
    {
        var pharmacy = new Pharmacy("PHA-001", "ร้านยาชุมชน");

        pharmacy.Code.Should().Be("PHA-001");
        pharmacy.Name.Should().Be("ร้านยาชุมชน");
        pharmacy.LicenseNo.Should().BeNull();
        pharmacy.PharmacyGroupId.Should().BeNull();
        pharmacy.PharmacyTypeId.Should().BeNull();
        pharmacy.ProvinceId.Should().BeNull();
        pharmacy.DistrictId.Should().BeNull();
        pharmacy.SubDistrictId.Should().BeNull();
    }

    [Test]
    public void Constructor_Throws_WhenCodeIsNull()
    {
        var act = () => new Pharmacy(null!, "ร้านยาชุมชน");

        act.Should().Throw<ArgumentNullException>().WithParameterName("code");
    }

    [Test]
    public void Constructor_Throws_WhenCodeIsEmpty()
    {
        var act = () => new Pharmacy("", "ร้านยาชุมชน");

        act.Should().Throw<ArgumentException>().WithParameterName("code");
    }

    [Test]
    public void Constructor_Throws_WhenCodeIsWhitespace()
    {
        var act = () => new Pharmacy("   ", "ร้านยาชุมชน");

        act.Should().Throw<ArgumentException>().WithParameterName("code");
    }

    [Test]
    public void Constructor_Throws_WhenNameIsNull()
    {
        var act = () => new Pharmacy("PHA-001", null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("name");
    }

    [Test]
    public void Constructor_Throws_WhenNameIsEmpty()
    {
        var act = () => new Pharmacy("PHA-001", "");

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Test]
    public void Constructor_Throws_WhenNameIsWhitespace()
    {
        var act = () => new Pharmacy("PHA-001", "   ");

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Test]
    public void Update_SetsFieldsAndActiveFlag()
    {
        var pharmacy = new Pharmacy("PHA-001", "ร้านยาชุมชน");

        Update(pharmacy, "PHA-002", "ร้านยาใหม่", isActive: false, licenseNo: "LIC-1", pharmacyGroupId: 3);

        pharmacy.Code.Should().Be("PHA-002");
        pharmacy.Name.Should().Be("ร้านยาใหม่");
        pharmacy.LicenseNo.Should().Be("LIC-1");
        pharmacy.PharmacyGroupId.Should().Be(3);
        pharmacy.IsActive.Should().BeFalse();
    }

    [Test]
    public void Update_Throws_WhenCodeIsNull()
    {
        var pharmacy = new Pharmacy("PHA-001", "ร้านยาชุมชน");
        var act = () => Update(pharmacy, null!, "ร้านยาชุมชน");

        act.Should().Throw<ArgumentNullException>().WithParameterName("code");
    }

    [Test]
    public void Update_Throws_WhenCodeIsWhitespace()
    {
        var pharmacy = new Pharmacy("PHA-001", "ร้านยาชุมชน");
        var act = () => Update(pharmacy, "   ", "ร้านยาชุมชน");

        act.Should().Throw<ArgumentException>().WithParameterName("code");
    }

    [Test]
    public void Update_Throws_WhenNameIsNull()
    {
        var pharmacy = new Pharmacy("PHA-001", "ร้านยาชุมชน");
        var act = () => Update(pharmacy, "PHA-001", null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("name");
    }

    [Test]
    public void Update_Throws_WhenNameIsWhitespace()
    {
        var pharmacy = new Pharmacy("PHA-001", "ร้านยาชุมชน");
        var act = () => Update(pharmacy, "PHA-001", "   ");

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Test]
    public void Deactivate_SetsInactiveAndDeleted()
    {
        var pharmacy = new Pharmacy("PHA-001", "ร้านยาชุมชน");

        pharmacy.Deactivate();

        pharmacy.IsActive.Should().BeFalse();
        pharmacy.DeleteFlag.Should().BeTrue();
    }

    private static void Update(
        Pharmacy pharmacy,
        string code,
        string name,
        bool isActive = true,
        string? licenseNo = null,
        int? pharmacyGroupId = null)
    {
        pharmacy.Update(
            code: code,
            name: name,
            licenseNo: licenseNo,
            nhsoCode: null,
            name2: null,
            pharmacyGroupId: pharmacyGroupId,
            pharmacyTypeId: null,
            pharmacyTypeOther: null,
            addressNo: null,
            provinceId: null,
            districtId: null,
            subDistrictId: null,
            zipCode: null,
            fdaProvince: null,
            officeTel: null,
            officeFax: null,
            officeMail: null,
            lineId: null,
            coName: null,
            coMail: null,
            coTel: null,
            regisYear: null,
            lat: null,
            lng: null,
            isActive: isActive);
    }
}
