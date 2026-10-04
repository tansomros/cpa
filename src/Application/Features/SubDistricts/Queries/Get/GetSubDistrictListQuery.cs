using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.SubDistricts.ViewModels;

namespace BigLion.CPA.Application.Features.SubDistricts.Queries.Get;

public class GetSubDistrictListQuery : IRequest<PaginatedList<SubDistrictViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetSubDistrictListQueryValidator : AbstractValidator<GetSubDistrictListQuery>
{
    public GetSubDistrictListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetSubDistrictListQueryHandler : IRequestHandler<GetSubDistrictListQuery, PaginatedList<SubDistrictViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetSubDistrictListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SubDistrictViewModel>> Handle(GetSubDistrictListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.SubDistricts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)));
        }

        return await query
            .OrderBy(x => x.SubDistrictId)
            .ProjectTo<SubDistrictViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
