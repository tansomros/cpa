using BigLion.CPA.Domain.Entities;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Security;

namespace BigLion.CPA.Application.Features.Systems.Commands;
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
            new RunningConfig("A","รหัสร้านยาเดี่ยว",true,false,4),
            new RunningConfig("B","รหัสร้านยา Boots",true,false,4), 
        };

        await _context.RunningConfigs.AddRangeAsync(RunningConfig, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }     
} 
