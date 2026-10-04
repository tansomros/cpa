using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.BehaviorProblemItems.ViewModels;

namespace BigLion.CPA.Application.Features.BehaviorProblemItems.Queries.Get;

public class GetBehaviorProblemItemListQuery : IRequest<PaginatedList<BehaviorProblemItemViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetBehaviorProblemItemListQueryValidator : AbstractValidator<GetBehaviorProblemItemListQuery>
{
    public GetBehaviorProblemItemListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetBehaviorProblemItemListQueryHandler : IRequestHandler<GetBehaviorProblemItemListQuery, PaginatedList<BehaviorProblemItemViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetBehaviorProblemItemListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BehaviorProblemItemViewModel>> Handle(GetBehaviorProblemItemListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.BehaviorProblemItems.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Descriptions != null && x.Descriptions.Contains(term)));
        }

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<BehaviorProblemItemViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
