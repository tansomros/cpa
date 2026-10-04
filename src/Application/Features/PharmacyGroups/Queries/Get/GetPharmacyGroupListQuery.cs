using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.PharmacyGroups.ViewModels;

namespace BigLion.CPA.Application.Features.PharmacyGroups.Queries.Get;

public class GetPharmacyGroupListQuery : IRequest<PaginatedList<PharmacyGroupViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetPharmacyGroupListQueryValidator : AbstractValidator<GetPharmacyGroupListQuery>
{
    public GetPharmacyGroupListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetPharmacyGroupListQueryHandler : IRequestHandler<GetPharmacyGroupListQuery, PaginatedList<PharmacyGroupViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPharmacyGroupListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PharmacyGroupViewModel>> Handle(GetPharmacyGroupListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.PharmacyGroups.AsNoTracking().Where(x => x.DeleteFlag != true);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)) || (x.Code != null && x.Code.Contains(term)));
        }

        return await query
            .OrderByDescending(x => x.CreatedOn)
            .ProjectTo<PharmacyGroupViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
