using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Security;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Pharmacy.ViewModels;

namespace BigLion.CPA.Application.Features.Pharmacy.Queries.Get;

[RequirePermission(Permissions.Pharmacies.View)]
public record GetPharmacyQuery : IRequest<PharmacyViewModel>
{
    public required int Id { get; init; }
}

public class GetPharmacyQueryHandler : IRequestHandler<GetPharmacyQuery, PharmacyViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPharmacyQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PharmacyViewModel> Handle(GetPharmacyQuery request, CancellationToken cancellationToken)
    {
        var pharmacy = await _context.Pharmacy
            .Include(p => p.PharmacyGroup)
            .Include(p => p.PharmacyType)
            .Include(p => p.Province)
            .Include(p => p.District)
            .Include(p => p.SubDistrict)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Pharmacy), request.Id);

        var model = _mapper.Map<PharmacyViewModel>(pharmacy);
        model.RowVersion = _context.Entry(pharmacy).Property<uint>("xmin").CurrentValue;
        return model;
    }
}
