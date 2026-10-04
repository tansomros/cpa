using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMs.ViewModels;

namespace BigLion.CPA.Application.Features.MTMs.Queries.Get;

public class GetMTMListQuery : IRequest<PaginatedList<MTMViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetMTMListQueryValidator : AbstractValidator<GetMTMListQuery>
{
    public GetMTMListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetMTMListQueryHandler : IRequestHandler<GetMTMListQuery, PaginatedList<MTMViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MTMViewModel>> Handle(GetMTMListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MTMs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.HospitalName != null && x.HospitalName.Contains(term)));
        }

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<MTMViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
