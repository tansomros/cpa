using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Security;
using Cpa.Domain.Entities;

#pragma warning disable CS0618
namespace Cpa.Application.Features.Systems.Commands;

/// <summary>
/// [OBSOLETE] à¸à¹à¸­à¸¡à¸¹à¸¥à¹à¸«à¸¥à¹à¸²à¸à¸µà¹à¸à¸¹à¸à¹à¸à¸à¸à¸µà¹à¸à¹à¸§à¸¢ SmartEnum à¹à¸ Domain.Enums à¹à¸¥à¹à¸§
/// à¹à¸¡à¹à¸à¹à¸­à¸ seed à¸¥à¸ DB à¸­à¸µà¸à¸à¹à¸­à¹à¸ â à¹à¸à¹ ExamResult.All, LabResult.All à¸¯à¸¥à¸¯ à¸à¸²à¸ memory à¹à¸à¹à¹à¸¥à¸¢
/// </summary>
[Obsolete("à¸à¹à¸­à¸¡à¸¹à¸¥à¹à¸«à¸¥à¹à¸²à¸à¸µà¹à¸à¸¹à¸à¹à¸à¸à¸à¸µà¹à¸à¹à¸§à¸¢ SmartEnum à¹à¸ Domain.Enums à¹à¸¥à¹à¸§ à¸à¸¹ LookupRegistry.cs")]
[Authorize(Policy = CpaPolicies.AllowAnonymous)]
public class ReferenceGroupDataInitializerCommand : IRequest<Unit> { }
public class ReferenceGroupDataInitializerCommandHandler : IRequestHandler<ReferenceGroupDataInitializerCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public ReferenceGroupDataInitializerCommandHandler(ICpaDatabaseContext checkupDatabaseContext)
    {
        _context = checkupDatabaseContext;
    }

    public async Task<Unit> Handle(ReferenceGroupDataInitializerCommand request, CancellationToken cancellationToken)
    {
        await SeedReferenceGroups(cancellationToken); 
        return Unit.Value;
    }

    private async Task SeedReferenceGroups(CancellationToken cancellationToken)
    {
        if (await _context.ReferenceGroups.AnyAsync(cancellationToken))
        {
            return;
        }

        var ReferenceGroup = new[]
        {
            new ReferenceGroup("GA","General",1) { Id = 1 },
            new ReferenceGroup("LAB","Lab",2) { Id = 2 },
            new ReferenceGroup("X","Xray",3) { Id = 3 },
            new ReferenceGroup("BMD","à¸à¸²à¸£à¸à¸£à¸§à¸à¸¡à¸§à¸¥à¸à¸£à¸°à¸à¸¹à¸",4) { Id = 4 },
            new ReferenceGroup("ABI","à¸à¸²à¸£à¸à¸£à¸§à¸à¸«à¸¥à¸­à¸à¹à¸¥à¸·à¸­à¸",5) { Id = 5 },
            new ReferenceGroup("CKST","à¸ªà¸à¸²à¸à¸°à¸à¸²à¸£à¸à¸£à¸§à¸à¸ªà¸¸à¸à¸ à¸²à¸",6) { Id = 6 },
            new ReferenceGroup("VA","à¸£à¸°à¸à¸±à¸à¸ªà¸²à¸¢à¸à¸²",7) { Id = 7 },
            new ReferenceGroup("EYE","à¸à¸¥à¸à¸²à¸£à¸à¸£à¸§à¸à¸ªà¸²à¸¢à¸à¸²",8) { Id = 8 },
            new ReferenceGroup("AU","à¸à¸§à¸²à¸¡à¸à¸´à¸à¸à¸à¸à¸´à¸«à¸¹",9) { Id = 9 }
        };

        await _context.ReferenceGroups.AddRangeAsync(ReferenceGroup, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
     
}
