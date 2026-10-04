using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.PharmacyTypes.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.PharmacyType;

namespace BigLion.CPA.Application.Features.PharmacyTypes.Queries.Get;

public class GetPharmacyTypeQuery : IRequest<PharmacyTypeViewModel>
{
    public int Id { get; set; }
}

public class GetPharmacyTypeQueryHandler : IRequestHandler<GetPharmacyTypeQuery, PharmacyTypeViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPharmacyTypeQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PharmacyTypeViewModel> Handle(GetPharmacyTypeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.PharmacyTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeleteFlag != true, cancellationToken)
            ?? throw new NotFoundException("PharmacyType", request.Id);

        return _mapper.Map<PharmacyTypeViewModel>(entity);
    }
}
