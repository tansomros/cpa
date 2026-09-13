namespace BigLion.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
        var database = new PostgreSQLTestDatabase();

        await database.InitialiseAsync();

        return database;
    }
}
