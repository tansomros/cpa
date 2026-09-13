using BigLion.Application.Features.Addresses.Queries.Get;
using BigLion.Application.FunctionalTests.Features._Shared;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Addresses.Queries;

public class GetDistrictListTests : BaseTestFixture
{
    /// ทดสอบ: ดึงรายชื่ออำเภอตาม ProvinceId ที่มีข้อมูล ควรคืนรายการอำเภอที่ถูกต้อง
    [Test]
    public async Task GetList_WithValidProvinceId_ShouldReturnDistricts()
    {
        RunAsDefaultUser();
        await TestDataFactory.CreateTestProvinceAsync("30", "นครราชสีมา");
        await TestDataFactory.CreateTestDistrictAsync("30", "3001", "เมืองนครราชสีมา");
        await TestDataFactory.CreateTestDistrictAsync("30", "3002", "ปักธงชัย");

        var result = await SendAsync(new GetDistrictListQuery { ProvinceId = "30" });

        result.Should().NotBeNull();
        result.Districts.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    /// ทดสอบ: ดึงรายชื่ออำเภอตาม ProvinceId ที่ไม่มีข้อมูล ควรคืนรายการว่างเปล่า
    [Test]
    public async Task GetList_WithInvalidProvinceId_ShouldReturnEmptyList()
    {
        RunAsDefaultUser();

        var result = await SendAsync(new GetDistrictListQuery { ProvinceId = "99999" });

        result.Should().NotBeNull();
        result.Districts.Should().BeEmpty();
    }
}
