using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMDrugRemains.ViewModels;

namespace BigLion.CPA.Application.Features.MTMDrugRemains.Queries.Get;

public class GetMTMDrugRemainListQuery : IRequest<PaginatedList<MTMDrugRemainViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class GetMTMDrugRemainListQueryValidator : AbstractValidator<GetMTMDrugRemainListQuery>
{
    public GetMTMDrugRemainListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetMTMDrugRemainListQueryHandler : IRequestHandler<GetMTMDrugRemainListQuery, PaginatedList<MTMDrugRemainViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMDrugRemainListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MTMDrugRemainViewModel>> Handle(GetMTMDrugRemainListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MTMDrugRemains.AsNoTracking();

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<MTMDrugRemainViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
