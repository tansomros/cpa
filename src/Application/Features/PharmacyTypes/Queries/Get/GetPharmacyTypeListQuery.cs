using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.PharmacyTypes.ViewModels;

namespace BigLion.CPA.Application.Features.PharmacyTypes.Queries.Get;

public class GetPharmacyTypeListQuery : IRequest<PaginatedList<PharmacyTypeViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetPharmacyTypeListQueryValidator : AbstractValidator<GetPharmacyTypeListQuery>
{
    public GetPharmacyTypeListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetPharmacyTypeListQueryHandler : IRequestHandler<GetPharmacyTypeListQuery, PaginatedList<PharmacyTypeViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPharmacyTypeListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PharmacyTypeViewModel>> Handle(GetPharmacyTypeListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.PharmacyTypes.AsNoTracking().Where(x => x.DeleteFlag != true);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)) || (x.Code != null && x.Code.Contains(term)));
        }

        return await query
            .OrderByDescending(x => x.CreatedOn)
            .ProjectTo<PharmacyTypeViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
