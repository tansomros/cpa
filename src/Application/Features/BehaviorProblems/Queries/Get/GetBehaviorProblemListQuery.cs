using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.BehaviorProblems.ViewModels;

namespace BigLion.CPA.Application.Features.BehaviorProblems.Queries.Get;

public class GetBehaviorProblemListQuery : IRequest<PaginatedList<BehaviorProblemViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class GetBehaviorProblemListQueryValidator : AbstractValidator<GetBehaviorProblemListQuery>
{
    public GetBehaviorProblemListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetBehaviorProblemListQueryHandler : IRequestHandler<GetBehaviorProblemListQuery, PaginatedList<BehaviorProblemViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetBehaviorProblemListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BehaviorProblemViewModel>> Handle(GetBehaviorProblemListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.BehaviorProblems.AsNoTracking();

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<BehaviorProblemViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
