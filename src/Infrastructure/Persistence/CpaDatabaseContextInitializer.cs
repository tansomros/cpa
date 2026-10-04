using BigLion.CPA.Application.Features.Systems.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BigLion.CPA.Infrastructure.Persistence
{
    public class CpaDatabaseContextInitializer
    {
        private readonly ILogger<CpaDatabaseContext> _logger;
        private readonly CpaDatabaseContext _context;

        public CpaDatabaseContextInitializer(ILogger<CpaDatabaseContext> logger, CpaDatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task MigrationAsync()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while initializing the database.");
                throw;
            }
        }

        public async Task SeedDataAsync(IServiceScope scope)
        {
            try
            {
                var services = scope.ServiceProvider;
                var mediator = services.GetRequiredService<IMediator>();
                await mediator.Send(new ThaiProvinceDataInitializerCommand());
                await mediator.Send(new PrefixDataInitializerCommand());
                await mediator.Send(new RoleDataInitializerCommand());
                await mediator.Send(new RunningConfigDataInitializerCommand());
                await mediator.Send(new RunningDataInitializerCommand());
                await mediator.Send(new UserDataInitializerCommand());
                await mediator.Send(new PharmacyGroupDataInitializerCommand());
                await mediator.Send(new PharmacyTypeDataInitializerCommand());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while initializing the database.");
                throw;
            }
        }
    }
}
