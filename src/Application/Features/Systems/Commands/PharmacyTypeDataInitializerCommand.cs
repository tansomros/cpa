using Cpa.Domain.Entities;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Security;

namespace Cpa.Application.Features.Systems.Commands;
[Authorize(Policy = CpaPolicies.AllowAnonymous)]
public class PharmacyTypeDataInitializerCommand : IRequest<Unit> { }
public class PharmacyTypeDataInitializerCommandHandler : IRequestHandler<PharmacyTypeDataInitializerCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public PharmacyTypeDataInitializerCommandHandler(ICpaDatabaseContext BigLionDatabaseContext)
    {
        _context = BigLionDatabaseContext;
    }

    public async Task<Unit> Handle(PharmacyTypeDataInitializerCommand request, CancellationToken cancellationToken)
    {
        await SeedPharmacyTypes(cancellationToken); 
        return Unit.Value;
    }

    private async Task SeedPharmacyTypes(CancellationToken cancellationToken)
    {
        if (await _context.PharmacyTypes.AnyAsync(cancellationToken))
        {
            return;
        }

        var PharmacyType = new[]
        {
            new PharmacyType("1","ร้านยาเดี่ยว",1),
            new PharmacyType("2","ร้านยา GPP",2),          
            new PharmacyType("3","ร้านยาเภสัชกร",3),
            new PharmacyType("4","โรงพยาบาล",4),
            new PharmacyType("5","รพสต.",5),
            new PharmacyType("6","เภสัชกร",6),
            new PharmacyType("7","อื่นๆ",7),

        };

        await _context.PharmacyTypes.AddRangeAsync(PharmacyType, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }     
} 
