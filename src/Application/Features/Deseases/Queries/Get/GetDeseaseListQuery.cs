using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Deseases.ViewModels;

namespace BigLion.CPA.Application.Features.Deseases.Queries.Get;

public class GetDeseaseListQuery : IRequest<PaginatedList<DeseaseViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetDeseaseListQueryValidator : AbstractValidator<GetDeseaseListQuery>
{
    public GetDeseaseListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetDeseaseListQueryHandler : IRequestHandler<GetDeseaseListQuery, PaginatedList<DeseaseViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDeseaseListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DeseaseViewModel>> Handle(GetDeseaseListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Deseases.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)) || (x.Code != null && x.Code.Contains(term)));
        }

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<DeseaseViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
