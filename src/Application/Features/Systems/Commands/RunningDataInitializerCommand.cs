using Cpa.Domain.Entities;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Security;

namespace Cpa.Application.Features.Systems.Commands;
[Authorize(Policy = CpaPolicies.AllowAnonymous)]
public class RunningDataInitializerCommand : IRequest<Unit> { }
public class RunningDataInitializerCommandHandler : IRequestHandler<RunningDataInitializerCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public RunningDataInitializerCommandHandler(ICpaDatabaseContext BigLionDatabaseContext)
    {
        _context = BigLionDatabaseContext;
    }

    public async Task<Unit> Handle(RunningDataInitializerCommand request, CancellationToken cancellationToken)
    {
        await SeedRunnings(cancellationToken); 
        return Unit.Value;
    }

    private async Task SeedRunnings(CancellationToken cancellationToken)
    {
        if (await _context.Runnings.AnyAsync(cancellationToken))
        {
            return;
        }

        var Running = new[]
        {
            new Running("A",10,0),
        };

        await _context.Runnings.AddRangeAsync(Running, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }     
} 
