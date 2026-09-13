using BigLion.Application.Features.Patients.Queries.Get;
using BigLion.Application.FunctionalTests.Features._Shared;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Patients.Queries;

public class GetPatientListTests : BaseTestFixture
{
    /// ทดสอบ: ดึงรายชื่อผู้ป่วยเมื่อมีข้อมูล ควรคืนรายการที่ไม่ว่างเปล่า
    [Test]
    public async Task GetList_WhenDataExists_ShouldReturnNonEmptyList()
    {
        RunAsDefaultUser();
        await TestDataFactory.CreateTestPatientAsync();

        var result = await SendAsync(new GetPatientListQuery());

        result.Should().NotBeNull();
        result.Items.Should().NotBeEmpty();
    }

    /// ทดสอบ: ดึงรายชื่อผู้ป่วยเมื่อไม่มีข้อมูล ควรคืนรายการว่างเปล่า
    [Test]
    public async Task GetList_WhenNoData_ShouldReturnEmptyList()
    {
        RunAsDefaultUser();

        var result = await SendAsync(new GetPatientListQuery());

        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
    }
}
