using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Npgsql;
using Respawn;
using BigLion.Application.Common.Interfaces;
using BigLion.Infrastructure.Persistence;
using BigLion.Infrastructure.Persistence.Interceptors;

namespace BigLion.Infrastructure.IntegrationTests;

[SetUpFixture]
public partial class Testing
{
    private static DbConnection _connection = null!;
    private static string _connectionString = null!;
    private static Respawner _respawner = null!;
    private static ServiceProvider _serviceProvider = null!;
    
    public static Mock<IDateTime> DateTimeMock { get; private set; } = null!;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        _connectionString = configuration.GetConnectionString("SUTH_CheckupTestDb") 
            ?? "Server=127.0.0.1;Port=5432;Database=checkup-test;Username=suth;Password=suth;";
        _connection = new NpgsqlConnection(_connectionString);
        
        var services = new ServiceCollection();
        DateTimeMock = new Mock<IDateTime>();
        
        services.AddSingleton(DateTimeMock.Object);
        services.AddScoped<AuditableEntitySaveChangesInterceptors>();
        
        services.AddDbContext<CheckupDatabaseContext>((sp, options) =>
        {
            options.UseNpgsql(_connectionString);
        });

        _serviceProvider = services.BuildServiceProvider();

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CheckupDatabaseContext>();
        
        context.Database.EnsureDeleted();
        await context.Database.MigrateAsync();

        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }

    public static async Task ResetState()
    {
        await _respawner.ResetAsync(_connection);
    }
    
    public static CheckupDatabaseContext CreateContext()
    {
        var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<CheckupDatabaseContext>();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _connection.DisposeAsync();
        if (_serviceProvider != null) await _serviceProvider.DisposeAsync();
    }
}
