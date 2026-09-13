#pragma warning disable CS0618
using BigLion.Application.Features.ReferenceGroups.Queries.GetPaginatedList;
using BigLion.Application.FunctionalTests.Features._Shared;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.ReferenceGroups.Queries;

public class GetReferenceGroupPaginatedListTests : BaseTestFixture
{
    /// ทดสอบ: ดึงรายการกลุ่มอ้างอิงแบบแบ่งหน้าเมื่อมีข้อมูล ควรคืนรายการที่ไม่ว่างเปล่า
    [Test]
    public async Task GetPaginatedList_WhenDataExists_ShouldReturnResults()
    {
        RunAsDefaultUser();
        await TestDataFactory.CreateTestReferenceGroupAsync("RG1", "กลุ่ม 1");
        await TestDataFactory.CreateTestReferenceGroupAsync("RG2", "กลุ่ม 2");

        var result = await SendAsync(new GetReferenceGroupPaginatedListQuery
        {
            Page = 1,
            Length = 10
        });

        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    /// ทดสอบ: ดึงรายการกลุ่มอ้างอิงเมื่อไม่มีข้อมูล ควรคืนรายการว่างเปล่า
    [Test]
    public async Task GetPaginatedList_WhenNoData_ShouldReturnEmptyList()
    {
        RunAsDefaultUser();

        var result = await SendAsync(new GetReferenceGroupPaginatedListQuery
        {
            Page = 1,
            Length = 10
        });

        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
    }
}
