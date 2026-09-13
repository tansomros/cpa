namespace BigLion.Infrastructure.IntegrationTests;

public abstract class BaseTestFixture
{
    [SetUp]
    public async Task TestSetUp()
    {
        await Testing.ResetState();
    }
}
