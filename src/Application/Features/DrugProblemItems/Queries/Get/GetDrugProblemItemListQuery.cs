using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.DrugProblemItems.ViewModels;

namespace BigLion.CPA.Application.Features.DrugProblemItems.Queries.Get;

public class GetDrugProblemItemListQuery : IRequest<PaginatedList<DrugProblemItemViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetDrugProblemItemListQueryValidator : AbstractValidator<GetDrugProblemItemListQuery>
{
    public GetDrugProblemItemListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetDrugProblemItemListQueryHandler : IRequestHandler<GetDrugProblemItemListQuery, PaginatedList<DrugProblemItemViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDrugProblemItemListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DrugProblemItemViewModel>> Handle(GetDrugProblemItemListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.DrugProblemItems.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Code != null && x.Code.Contains(term)) || (x.Descriptions != null && x.Descriptions.Contains(term)));
        }

        return await query
            .OrderBy(x => x.Code)
            .ProjectTo<DrugProblemItemViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
