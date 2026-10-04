using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.LabUOMs.ViewModels;

namespace BigLion.CPA.Application.Features.LabUOMs.Queries.Get;

public class GetLabUOMListQuery : IRequest<PaginatedList<LabUOMViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetLabUOMListQueryValidator : AbstractValidator<GetLabUOMListQuery>
{
    public GetLabUOMListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetLabUOMListQueryHandler : IRequestHandler<GetLabUOMListQuery, PaginatedList<LabUOMViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetLabUOMListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LabUOMViewModel>> Handle(GetLabUOMListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.LabUOMs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Descriptions != null && x.Descriptions.Contains(term)));
        }

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<LabUOMViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
