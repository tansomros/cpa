using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.DrugProblemGroups.ViewModels;

namespace BigLion.CPA.Application.Features.DrugProblemGroups.Queries.Get;

public class GetDrugProblemGroupListQuery : IRequest<PaginatedList<DrugProblemGroupViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetDrugProblemGroupListQueryValidator : AbstractValidator<GetDrugProblemGroupListQuery>
{
    public GetDrugProblemGroupListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetDrugProblemGroupListQueryHandler : IRequestHandler<GetDrugProblemGroupListQuery, PaginatedList<DrugProblemGroupViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDrugProblemGroupListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DrugProblemGroupViewModel>> Handle(GetDrugProblemGroupListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.DrugProblemGroups.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Code != null && x.Code.Contains(term)) || (x.Descriptions != null && x.Descriptions.Contains(term)));
        }

        return await query
            .OrderBy(x => x.Code)
            .ProjectTo<DrugProblemGroupViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
