using BigLion.CPA.Domain.Entities;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Security;

namespace BigLion.CPA.Application.Features.Systems.Commands;
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
            await SyncPrefixIdSequence(cancellationToken);
            return;
        }

        var Prefix = new[]
        {
            new Prefix(1,"นาย"),
            new Prefix(2,"นาง"),          
            new Prefix(3,"นางสาว"),
            new Prefix(4,"ภก."),
            new Prefix(5,"ภญ."),
        };

        await _context.Prefixs.AddRangeAsync(Prefix, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await SyncPrefixIdSequence(cancellationToken);
    }

    private async Task SyncPrefixIdSequence(CancellationToken cancellationToken)
    {
        await _context.Database.ExecuteSqlRawAsync(
            """
            DO $$
            BEGIN
                PERFORM setval(pg_get_serial_sequence('"Prefixs"', 'Id'), COALESCE((SELECT MAX("Id") FROM "Prefixs"), 1));
            END $$;
            """,
            cancellationToken);
    }     
} 
