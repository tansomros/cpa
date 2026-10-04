using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Pharmacists.ViewModels;

namespace BigLion.CPA.Application.Features.Pharmacists.Queries.Get;

public class GetPharmacistListQuery : IRequest<PaginatedList<PharmacistViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetPharmacistListQueryValidator : AbstractValidator<GetPharmacistListQuery>
{
    public GetPharmacistListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetPharmacistListQueryHandler : IRequestHandler<GetPharmacistListQuery, PaginatedList<PharmacistViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPharmacistListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PharmacistViewModel>> Handle(GetPharmacistListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Pharmacists.AsNoTracking().Where(x => x.DeleteFlag != true);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)));
        }

        return await query
            .OrderByDescending(x => x.CreatedOn)
            .ProjectTo<PharmacistViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
