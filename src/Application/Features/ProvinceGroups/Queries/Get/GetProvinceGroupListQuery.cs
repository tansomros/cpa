using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.ProvinceGroups.ViewModels;

namespace BigLion.CPA.Application.Features.ProvinceGroups.Queries.Get;

public class GetProvinceGroupListQuery : IRequest<PaginatedList<ProvinceGroupViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetProvinceGroupListQueryValidator : AbstractValidator<GetProvinceGroupListQuery>
{
    public GetProvinceGroupListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetProvinceGroupListQueryHandler : IRequestHandler<GetProvinceGroupListQuery, PaginatedList<ProvinceGroupViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetProvinceGroupListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProvinceGroupViewModel>> Handle(GetProvinceGroupListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ProvinceGroups.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)));
        }

        return await query
            .OrderBy(x => x.Id)
            .ProjectTo<ProvinceGroupViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
