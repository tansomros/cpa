using BigLion.Application.Features.Addresses.Queries.Get;
using BigLion.Application.FunctionalTests.Features._Shared;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Addresses.Queries;

public class GetProvinceListTests : BaseTestFixture
{
    /// ทดสอบ: ดึงรายชื่อจังหวัดเมื่อมีข้อมูลในระบบ ควรคืนรายการที่ไม่ว่างเปล่า
    [Test]
    public async Task GetList_WhenDataExists_ShouldReturnNonEmptyList()
    {
        RunAsDefaultUser();
        await TestDataFactory.CreateTestProvinceAsync("10", "กรุงเทพมหานคร");
        await TestDataFactory.CreateTestProvinceAsync("30", "นครราชสีมา");

        var result = await SendAsync(new GetProvinceListQuery());

        result.Should().NotBeNull();
        result.Provinces.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    /// ทดสอบ: ดึงรายชื่อจังหวัดเมื่อไม่มีข้อมูล ควรคืนรายการว่างเปล่า
    [Test]
    public async Task GetList_WhenNoData_ShouldReturnEmptyList()
    {
        RunAsDefaultUser();

        var result = await SendAsync(new GetProvinceListQuery());

        result.Should().NotBeNull();
        result.Provinces.Should().BeEmpty();
    }
}
