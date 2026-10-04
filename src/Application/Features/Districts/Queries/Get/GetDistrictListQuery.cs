using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Districts.ViewModels;

namespace BigLion.CPA.Application.Features.Districts.Queries.Get;

public class GetDistrictListQuery : IRequest<PaginatedList<DistrictViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetDistrictListQueryValidator : AbstractValidator<GetDistrictListQuery>
{
    public GetDistrictListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetDistrictListQueryHandler : IRequestHandler<GetDistrictListQuery, PaginatedList<DistrictViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDistrictListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DistrictViewModel>> Handle(GetDistrictListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Districts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)));
        }

        return await query
            .OrderBy(x => x.Id)
            .ProjectTo<DistrictViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
