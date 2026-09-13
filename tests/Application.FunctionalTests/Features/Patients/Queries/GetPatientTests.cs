using BigLion.Application.Features.Patients.Queries.Get;
using BigLion.Application.FunctionalTests.Features._Shared;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Patients.Queries;

public class GetPatientTests : BaseTestFixture
{
    /// ทดสอบ: ค้นหาผู้ป่วยด้วย Id ที่มีอยู่ ควรคืนข้อมูลที่ถูกต้อง
    [Test]
    public async Task Get_ExistingId_ShouldReturnViewModel()
    {
        RunAsDefaultUser();
        var id = await TestDataFactory.CreateTestPatientAsync();

        var result = await SendAsync(new GetPatientQuery { Id = id });

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
    }

    /// ทดสอบ: ค้นหาผู้ป่วยด้วย Id ที่ไม่มี ควร throw NotFoundException
    [Test]
    public async Task Get_NonExisting_ShouldThrowNotFoundException()
    {
        RunAsDefaultUser();

        await FluentActions.Invoking(() => SendAsync(new GetPatientQuery { Id = 99999 }))
            .Should().ThrowAsync<BigLion.Application.Exceptions.NotFoundException>();
    }
}
