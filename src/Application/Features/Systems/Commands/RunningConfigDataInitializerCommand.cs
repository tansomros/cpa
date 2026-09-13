using Cpa.Domain.Entities;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Security;

namespace Cpa.Application.Features.Systems.Commands;
[Authorize(Policy = CpaPolicies.AllowAnonymous)]
public class RunningConfigDataInitializerCommand : IRequest<Unit> { }
public class RunningConfigDataInitializerCommandHandler : IRequestHandler<RunningConfigDataInitializerCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public RunningConfigDataInitializerCommandHandler(ICpaDatabaseContext BigLionDatabaseContext)
    {
        _context = BigLionDatabaseContext;
    }

    public async Task<Unit> Handle(RunningConfigDataInitializerCommand request, CancellationToken cancellationToken)
    {
        await SeedRunningConfigs(cancellationToken); 
        return Unit.Value;
    }

    private async Task SeedRunningConfigs(CancellationToken cancellationToken)
    {
        if (await _context.RunningConfigs.AnyAsync(cancellationToken))
        {
            return;
        }

        var RunningConfig = new[]
        {
            new RunningConfig("C","รหัสลูกค้า",true,false,4),
            new RunningConfig("F","โรงงาน",true,false,2),
            new RunningConfig("K","เลขที่ใบเสร็จรับเงิน",true,true,5),
        };

        await _context.RunningConfigs.AddRangeAsync(RunningConfig, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }     
} 
