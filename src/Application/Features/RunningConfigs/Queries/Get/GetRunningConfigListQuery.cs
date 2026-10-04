using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.RunningConfigs.ViewModels;

namespace BigLion.CPA.Application.Features.RunningConfigs.Queries.Get;

public class GetRunningConfigListQuery : IRequest<PaginatedList<RunningConfigViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetRunningConfigListQueryValidator : AbstractValidator<GetRunningConfigListQuery>
{
    public GetRunningConfigListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetRunningConfigListQueryHandler : IRequestHandler<GetRunningConfigListQuery, PaginatedList<RunningConfigViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetRunningConfigListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RunningConfigViewModel>> Handle(GetRunningConfigListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.RunningConfigs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Code != null && x.Code.Contains(term)));
        }

        return await query
            .OrderBy(x => x.Code)
            .ProjectTo<RunningConfigViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
