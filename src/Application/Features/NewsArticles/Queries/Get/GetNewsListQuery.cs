using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.NewsArticles.ViewModels;

namespace BigLion.CPA.Application.Features.NewsArticles.Queries.Get;

public class GetNewsListQuery : IRequest<PaginatedList<NewsViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetNewsListQueryValidator : AbstractValidator<GetNewsListQuery>
{
    public GetNewsListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetNewsListQueryHandler : IRequestHandler<GetNewsListQuery, PaginatedList<NewsViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetNewsListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<NewsViewModel>> Handle(GetNewsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.News.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Title != null && x.Title.Contains(term)));
        }

        return await query
            .OrderBy(x => x.NewsID)
            .ProjectTo<NewsViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
