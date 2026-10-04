using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMBehaviors.ViewModels;

namespace BigLion.CPA.Application.Features.MTMBehaviors.Queries.Get;

public class GetMTMBehaviorListQuery : IRequest<PaginatedList<MTMBehaviorViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class GetMTMBehaviorListQueryValidator : AbstractValidator<GetMTMBehaviorListQuery>
{
    public GetMTMBehaviorListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetMTMBehaviorListQueryHandler : IRequestHandler<GetMTMBehaviorListQuery, PaginatedList<MTMBehaviorViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMBehaviorListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MTMBehaviorViewModel>> Handle(GetMTMBehaviorListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MTMBehaviors.AsNoTracking();

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<MTMBehaviorViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
