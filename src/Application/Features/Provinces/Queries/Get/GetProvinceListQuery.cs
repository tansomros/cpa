using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Provinces.ViewModels;

namespace BigLion.CPA.Application.Features.Provinces.Queries.Get;

public class GetProvinceListQuery : IRequest<PaginatedList<ProvinceViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetProvinceListQueryValidator : AbstractValidator<GetProvinceListQuery>
{
    public GetProvinceListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetProvinceListQueryHandler : IRequestHandler<GetProvinceListQuery, PaginatedList<ProvinceViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetProvinceListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProvinceViewModel>> Handle(GetProvinceListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Provinces.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)));
        }

        return await query
            .OrderBy(x => x.Id)
            .ProjectTo<ProvinceViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
