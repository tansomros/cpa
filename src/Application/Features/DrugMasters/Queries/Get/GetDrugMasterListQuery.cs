using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.DrugMasters.ViewModels;

namespace BigLion.CPA.Application.Features.DrugMasters.Queries.Get;

public class GetDrugMasterListQuery : IRequest<PaginatedList<DrugMasterViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetDrugMasterListQueryValidator : AbstractValidator<GetDrugMasterListQuery>
{
    public GetDrugMasterListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetDrugMasterListQueryHandler : IRequestHandler<GetDrugMasterListQuery, PaginatedList<DrugMasterViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDrugMasterListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DrugMasterViewModel>> Handle(GetDrugMasterListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.DrugMasters.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)) || (x.AliasName != null && x.AliasName.Contains(term)));
        }

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<DrugMasterViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
