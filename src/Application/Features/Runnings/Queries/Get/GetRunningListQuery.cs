using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Runnings.ViewModels;

namespace BigLion.CPA.Application.Features.Runnings.Queries.Get;

public class GetRunningListQuery : IRequest<PaginatedList<RunningViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetRunningListQueryValidator : AbstractValidator<GetRunningListQuery>
{
    public GetRunningListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetRunningListQueryHandler : IRequestHandler<GetRunningListQuery, PaginatedList<RunningViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetRunningListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RunningViewModel>> Handle(GetRunningListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Runnings.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Code != null && x.Code.Contains(term)));
        }

        return await query
            .OrderBy(x => x.Code)
            .ProjectTo<RunningViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
