using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.PharmacyGroups.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.PharmacyGroup;

namespace BigLion.CPA.Application.Features.PharmacyGroups.Queries.Get;

public class GetPharmacyGroupQuery : IRequest<PharmacyGroupViewModel>
{
    public int Id { get; set; }
}

public class GetPharmacyGroupQueryHandler : IRequestHandler<GetPharmacyGroupQuery, PharmacyGroupViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPharmacyGroupQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PharmacyGroupViewModel> Handle(GetPharmacyGroupQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.PharmacyGroups
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeleteFlag != true, cancellationToken)
            ?? throw new NotFoundException("PharmacyGroup", request.Id);

        return _mapper.Map<PharmacyGroupViewModel>(entity);
    }
}
