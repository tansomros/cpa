#pragma warning disable CS0618
using BigLion.Domain.Entities;
using BigLion.Application.Features.Systems.Commands;

using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Systems.Commands;

public class DataInitializerTests : BaseTestFixture
{
    /// <summary>
    /// ทดสอบ: สั่ง Seed ข้อมูลประเภทการตรวจ เมื่อตารางว่าง ควร Seed ข้อมูลเริ่มต้นเข้าไปในฐานข้อมูล
    /// </summary>
    [Test]
    public async Task CheckupTypeInitializer_ShouldSeedData()
    {
        RunAsDefaultUser();

        await SendAsync(new CheckupTypeDataInitializerCommand());

        var count = await CountAsync<CheckupType>();
        count.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// ทดสอบ: สั่ง Seed ข้อมูลประเภทการตรวจซ้ำ ควรไม่ทำให้ข้อมูลซ้ำซ้อน (Idempotent)
    /// </summary>
    [Test]
    public async Task CheckupTypeInitializer_WhenRunTwice_ShouldNotDuplicate()
    {
        RunAsDefaultUser();

        await SendAsync(new CheckupTypeDataInitializerCommand());
        var firstCount = await CountAsync<CheckupType>();

        await SendAsync(new CheckupTypeDataInitializerCommand());
        var secondCount = await CountAsync<CheckupType>();

        secondCount.Should().Be(firstCount);
    }

    /// <summary>
    /// ทดสอบ: สั่ง Seed ข้อมูลหมวดตรวจ ควร Seed ข้อมูลเริ่มต้นเข้าไปในฐานข้อมูล
    /// </summary>
    [Test]
    public async Task CheckupClassInitializer_ShouldSeedData()
    {
        RunAsDefaultUser();

        await SendAsync(new CheckupClassDataInitializerCommand());

        var count = await CountAsync<CheckupClass>();
        count.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// ทดสอบ: สั่ง Seed ข้อมูลกลุ่มตรวจ (ต้อง Seed หมวดตรวจก่อน) ควร Seed ข้อมูลเริ่มต้นเข้าไปในฐานข้อมูล
    /// </summary>
    [Test]
    public async Task CheckupGroupInitializer_ShouldSeedData()
    {
        RunAsDefaultUser();

        // ต้อง Seed หมวดตรวจก่อนเนื่องจากมี Foreign Key
        await SendAsync(new CheckupClassDataInitializerCommand());
        await SendAsync(new CheckupGroupDataInitializerCommand());

        var count = await CountAsync<CheckupGroup>();
        count.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// ทดสอบ: สั่ง Seed ข้อมูลรายการตรวจ (ต้อง Seed กลุ่มตรวจก่อน) ควร Seed ข้อมูลเริ่มต้นเข้าไปในฐานข้อมูล
    /// </summary>
    [Test]
    public async Task CheckupItemInitializer_ShouldSeedData()
    {
        RunAsDefaultUser();

        await SendAsync(new CheckupClassDataInitializerCommand());
        await SendAsync(new CheckupGroupDataInitializerCommand());
        await SendAsync(new CheckupItemDataInitializerCommand());

        var count = await CountAsync<CheckupItem>();
        count.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// ทดสอบ: สั่ง Seed ข้อมูลความถี่การได้ยิน ควร Seed ข้อมูลเริ่มต้นเข้าไปในฐานข้อมูล
    /// </summary>
    [Test]
    public async Task HearingHertzInitializer_ShouldSeedData()
    {
        RunAsDefaultUser();

        await SendAsync(new HearingHertzDataInitializerCommand());

        var count = await CountAsync<HearingHertz>();
        count.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// [OBSOLETE] ทดสอบ: สั่ง Seed ข้อมูลกลุ่มอ้างอิง — ถูกแทนที่ด้วย SmartEnum แล้ว ดู LookupRegistry.cs
    /// </summary>
    [Test]
    [Obsolete("ReferenceGroup ถูกแทนที่ด้วย SmartEnum ใน Domain.Enums แล้ว")]
    public async Task ReferenceGroupInitializer_ShouldSeedData()
    {
        RunAsDefaultUser();

#pragma warning disable CS0618 // Obsolete
        await SendAsync(new ReferenceGroupDataInitializerCommand());
#pragma warning restore CS0618

        var count = await CountAsync<ReferenceGroup>();
        count.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// [OBSOLETE] ทดสอบ: สั่ง Seed ข้อมูลค่าอ้างอิง — ถูกแทนที่ด้วย SmartEnum แล้ว ดู LookupRegistry.cs
    /// </summary>
    [Test]
    [Obsolete("ReferenceValue ถูกแทนที่ด้วย SmartEnum ใน Domain.Enums แล้ว")]
    public async Task ReferenceValueInitializer_ShouldSeedData()
    {
        RunAsDefaultUser();

#pragma warning disable CS0618 // Obsolete
        await SendAsync(new ReferenceGroupDataInitializerCommand());
        await SendAsync(new ReferenceValueDataInitializerCommand());
#pragma warning restore CS0618

        var count = await CountAsync<ReferenceValue>();
        count.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// ทดสอบ: สั่ง Seed ข้อมูลคำแนะนำ ควร Seed ข้อมูลเริ่มต้นเข้าไปในฐานข้อมูล
    /// </summary>
    [Test]
    public async Task RecommendationInitializer_ShouldSeedData()
    {
        RunAsDefaultUser();

        await SendAsync(new RecommendationDataInitializerCommand());

        var count = await CountAsync<Recommendation>();
        count.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// ทดสอบ: สั่ง Seed ข้อมูลจังหวัดของประเทศไทย ควร Seed ข้อมูลเริ่มต้นเข้าไปในฐานข้อมูล
    /// </summary>
    [Test]
    public async Task ThaiProvinceInitializer_ShouldSeedData()
    {
        RunAsDefaultUser();

        await SendAsync(new ThaiProvinceDataInitializerCommand());

        var count = await CountAsync<Province>();
        count.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// ทดสอบ: สั่ง Seed ข้อมูลเทมเพลตคำแนะนำ ควร Seed ข้อมูลเริ่มต้นเข้าไปในฐานข้อมูล
    /// </summary>
    [Test]
    public async Task RecommendationTemplateInitializer_ShouldSeedData()
    {
        RunAsDefaultUser();

        await SendAsync(new RecommendationTemplateInitializerCommand());

        var count = await CountAsync<RecommendationTemplate>();
        count.Should().BeGreaterThan(0);
    }
}
