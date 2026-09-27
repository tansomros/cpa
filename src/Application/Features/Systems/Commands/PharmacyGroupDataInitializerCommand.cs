using Cpa.Domain.Entities;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Security;

namespace Cpa.Application.Features.Systems.Commands;
[Authorize(Policy = CpaPolicies.AllowAnonymous)]
public class PharmacyGroupDataInitializerCommand : IRequest<Unit> { }
public class PharmacyGroupDataInitializerCommandHandler : IRequestHandler<PharmacyGroupDataInitializerCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public PharmacyGroupDataInitializerCommandHandler(ICpaDatabaseContext BigLionDatabaseContext)
    {
        _context = BigLionDatabaseContext;
    }

    public async Task<Unit> Handle(PharmacyGroupDataInitializerCommand request, CancellationToken cancellationToken)
    {
        await SeedPharmacyGroups(cancellationToken); 
        return Unit.Value;
    }

    private async Task SeedPharmacyGroups(CancellationToken cancellationToken)
    {
        if (await _context.PharmacyGroups.AnyAsync(cancellationToken))
        {
            return;
        }

        var PharmacyGroup = new[]
        {
            new PharmacyGroup("A","ร้ายยาเดี่ยว",0),
            new PharmacyGroup("B","ร้านยา Boots",2),          
            new PharmacyGroup("C","ร้านยาเครือข่ายร้อยแก่นสารสินธุ์",3),
            new PharmacyGroup("H","โรงพยาบาล",4),
            new PharmacyGroup("P","ร้านยา Pure ( Big C )",5),
            new PharmacyGroup("W","ร้านยา Watsons",6),
            new PharmacyGroup("X","ร้านยา Xta ( CP )",7),
        };

        await _context.PharmacyGroups.AddRangeAsync(PharmacyGroup, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }     
} 
