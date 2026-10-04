using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Prefixs.ViewModels;

namespace BigLion.CPA.Application.Features.Prefixs.Queries.Get;

public class GetPrefixListQuery : IRequest<PaginatedList<PrefixViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetPrefixListQueryValidator : AbstractValidator<GetPrefixListQuery>
{
    public GetPrefixListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetPrefixListQueryHandler : IRequestHandler<GetPrefixListQuery, PaginatedList<PrefixViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPrefixListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PrefixViewModel>> Handle(GetPrefixListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Prefixs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)));
        }

        return await query
            .OrderBy(x => x.Id)
            .ProjectTo<PrefixViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
