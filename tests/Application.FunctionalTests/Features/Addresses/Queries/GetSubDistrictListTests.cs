using BigLion.Application.Features.Addresses.Queries.Get;
using BigLion.Application.FunctionalTests.Features._Shared;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Addresses.Queries;

public class GetSubDistrictListTests : BaseTestFixture
{
    /// ทดสอบ: ดึงรายชื่อตำบลตาม DistrictId ที่มีข้อมูล ควรคืนรายการตำบลที่ถูกต้อง
    [Test]
    public async Task GetList_WithValidDistrictId_ShouldReturnSubDistricts()
    {
        RunAsDefaultUser();
        await TestDataFactory.CreateTestProvinceAsync("30", "นครราชสีมา");
        await TestDataFactory.CreateTestDistrictAsync("30", "3001", "เมืองนครราชสีมา");
        await TestDataFactory.CreateTestSubDistrictAsync("30", "3001", "300101", "ในเมือง");
        await TestDataFactory.CreateTestSubDistrictAsync("30", "3001", "300102", "โพธิ์กลาง");

        var result = await SendAsync(new GetSubDistrictListQuery { DistrictId = "3001" });

        result.Should().NotBeNull();
        result.SubDistricts.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    /// ทดสอบ: ดึงรายชื่อตำบลตาม DistrictId ที่ไม่มีข้อมูล ควรคืนรายการว่างเปล่า
    [Test]
    public async Task GetList_WithInvalidDistrictId_ShouldReturnEmptyList()
    {
        RunAsDefaultUser();

        var result = await SendAsync(new GetSubDistrictListQuery { DistrictId = "99999" });

        result.Should().NotBeNull();
        result.SubDistricts.Should().BeEmpty();
    }
}
