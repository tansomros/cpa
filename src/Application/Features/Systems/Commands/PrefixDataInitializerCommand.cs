using Cpa.Domain.Entities;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Security;

namespace Cpa.Application.Features.Systems.Commands;
[Authorize(Policy = CpaPolicies.AllowAnonymous)]
public class PrefixDataInitializerCommand : IRequest<Unit> { }
public class PrefixDataInitializerCommandHandler : IRequestHandler<PrefixDataInitializerCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public PrefixDataInitializerCommandHandler(ICpaDatabaseContext BigLionDatabaseContext)
    {
        _context = BigLionDatabaseContext;
    }

    public async Task<Unit> Handle(PrefixDataInitializerCommand request, CancellationToken cancellationToken)
    {
        await SeedPrefixs(cancellationToken); 
        return Unit.Value;
    }

    private async Task SeedPrefixs(CancellationToken cancellationToken)
    {
        if (await _context.Prefixs.AnyAsync(cancellationToken))
        {
            return;
        }

        var Prefix = new[]
        {
            new Prefix(1,"นาย"),
            new Prefix(2,"นาง"),          
            new Prefix(3,"นางสาว"), 
        };

        await _context.Prefixs.AddRangeAsync(Prefix, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }     
} 
