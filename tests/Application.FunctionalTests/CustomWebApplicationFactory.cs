using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using BigLion.Application.Common.Interfaces;
using BigLion.Infrastructure.Persistence;
using BigLion.Presentation.API;

namespace BigLion.Application.FunctionalTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection;
    private readonly string _connectionString;
    private readonly FakeCurrentUserService _fakeCurrentUserService;

    public CustomWebApplicationFactory(
        DbConnection connection,
        string connectionString,
        FakeCurrentUserService fakeCurrentUserService)
    {
        _connection = connection;
        _connectionString = connectionString;
        _fakeCurrentUserService = fakeCurrentUserService;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builder, services) =>
        {
            services
                .RemoveAll<ICurrentUserService>()
                .AddSingleton<ICurrentUserService>(_fakeCurrentUserService);

            // Configure Npgsql data source with Dynamic JSON enabled for tests
            var conn = builder.Configuration.GetConnectionString("SUTH_CheckupTestDb")
                ?? builder.Configuration.GetConnectionString("DefaultConnection");

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(conn);
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();

            services
                .RemoveAll<DbContextOptions<CheckupDatabaseContext>>()
                .AddDbContext<CheckupDatabaseContext>((sp, options) =>
                    options.UseNpgsql(dataSource, npgsql => npgsql.MigrationsAssembly(typeof(CheckupDatabaseContext).Assembly.FullName)));
        });
    }
}
