using Cpa.Application.Common.Interfaces;
using Cpa.Application.Identity.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using IIdentityService = Cpa.Application.Identity.Interfaces.IIdentityService;
using Cpa.Infrastructure.Identity;
using Cpa.Infrastructure.Persistence.Interceptors;
using Cpa.Infrastructure.Services;
using Cpa.Infrastructure.Persistence;

namespace Cpa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBigLionInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptors>();

        // enable json store
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("CPADatabase"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        services.AddDbContext<CpaDatabaseContext>(options => options.UseNpgsql(dataSource));
        //services.AddScoped(provider => (Application.Common.Interfaces.ICpaDatabaseContext)provider.GetRequiredService<Persistence.CpaDatabaseContext>());
        services.AddScoped<ICpaDatabaseContext>(provider =>
    provider.GetRequiredService<CpaDatabaseContext>());
        services.AddScoped<CpaDatabaseContextInitializer>();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();    
        services.AddScoped<IJwtTokenService, JwtTokenService>();    
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddTransient<IDateTime, DateTimeService>();
        return services;
    }
}
