using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BigLion.Application.Common.Security;
using BigLion.Infrastructure.Persistence;

namespace BigLion.Application.FunctionalTests;

[SetUpFixture]
public partial class Testing
{
    private static ITestDatabase _database = null!;
    private static CustomWebApplicationFactory _factory = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static readonly FakeCurrentUserService _fakeCurrentUserService = new();
    private static string? _userId;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        _database = await TestDatabaseFactory.CreateAsync();

        _factory = new CustomWebApplicationFactory(
            _database.GetConnection(),
            _database.GetConnectionString(),
            _fakeCurrentUserService);

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task SendAsync(IBaseRequest request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public static string? GetUserId()
    {
        return _userId;
    }

    public static void RunAsDefaultUser()
    {
        _fakeCurrentUserService.Reset();
        _fakeCurrentUserService.Id = "test-user-id";
        _fakeCurrentUserService.Name = "Test User";
        _fakeCurrentUserService.LoginName = "test@local";
        _fakeCurrentUserService.AddPolicies(HealthCheckupPolicies.RequireAuthenticatedUser);
        _userId = _fakeCurrentUserService.Id;
    }

    public static void RunAsDoctor(string doctorCode = "DOC001")
    {
        _fakeCurrentUserService.Reset();
        _fakeCurrentUserService.Id = "test-doctor-id";
        _fakeCurrentUserService.Name = "Dr. Test";
        _fakeCurrentUserService.LoginName = "doctor@local";
        _fakeCurrentUserService.DoctorCode = doctorCode;
        _fakeCurrentUserService.HasDoctorRole = true;
        _fakeCurrentUserService.AddRoles(HealthCheckupRoles.Doctor);
        _fakeCurrentUserService.AddPolicies(
            HealthCheckupPolicies.RequireAuthenticatedUser,
            HealthCheckupPolicies.RequireDoctor);
        _userId = _fakeCurrentUserService.Id;
    }

    public static void RunAsNurse()
    {
        _fakeCurrentUserService.Reset();
        _fakeCurrentUserService.Id = "test-nurse-id";
        _fakeCurrentUserService.Name = "Nurse Test";
        _fakeCurrentUserService.LoginName = "nurse@local";
        _fakeCurrentUserService.AddRoles(HealthCheckupRoles.Nurse);
        _fakeCurrentUserService.AddPolicies(
            HealthCheckupPolicies.RequireAuthenticatedUser,
            HealthCheckupPolicies.RequireNurse);
        _userId = _fakeCurrentUserService.Id;
    }

    public static void RunAsAdmin()
    {
        _fakeCurrentUserService.Reset();
        _fakeCurrentUserService.Id = "test-admin-id";
        _fakeCurrentUserService.Name = "Admin Test";
        _fakeCurrentUserService.LoginName = "admin@local";
        _fakeCurrentUserService.HasAdminRole = true;
        _fakeCurrentUserService.AddRoles(HealthCheckupRoles.Admin);
        _fakeCurrentUserService.AddPolicies(
            HealthCheckupPolicies.RequireAuthenticatedUser,
            HealthCheckupPolicies.RequireAdmin);
        _userId = _fakeCurrentUserService.Id;
    }

    public static void RunAsEmployee(string employeeId = "EMP001")
    {
        _fakeCurrentUserService.Reset();
        _fakeCurrentUserService.Id = "test-employee-id";
        _fakeCurrentUserService.Name = "Employee Test";
        _fakeCurrentUserService.LoginName = "employee@local";
        _fakeCurrentUserService.EmployeeId = employeeId;
        _fakeCurrentUserService.AddRoles(HealthCheckupRoles.Employee);
        _fakeCurrentUserService.AddPolicies(
            HealthCheckupPolicies.RequireAuthenticatedUser,
            HealthCheckupPolicies.RequireEmployee);
        _userId = _fakeCurrentUserService.Id;
    }

    public static void RunAsAnonymous()
    {
        _fakeCurrentUserService.Reset();
        _userId = null;
    }

    public static async Task ResetState()
    {
        try
        {
            await _database.ResetAsync();
        }
        catch (Exception)
        {
        }

        _fakeCurrentUserService.Reset();
        _userId = null;
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<CheckupDatabaseContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<CheckupDatabaseContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task UpdateAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<CheckupDatabaseContext>();

        context.Update(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<CheckupDatabaseContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    public static async Task<List<TEntity>> QueryAsync<TEntity>(
        System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<CheckupDatabaseContext>();

        return await context.Set<TEntity>().Where(predicate).AsNoTracking().ToListAsync();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _database.DisposeAsync();
        await _factory.DisposeAsync();
    }
}
