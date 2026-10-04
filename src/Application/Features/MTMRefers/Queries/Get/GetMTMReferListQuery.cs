using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMRefers.ViewModels;

namespace BigLion.CPA.Application.Features.MTMRefers.Queries.Get;

public class GetMTMReferListQuery : IRequest<PaginatedList<MTMReferViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetMTMReferListQueryValidator : AbstractValidator<GetMTMReferListQuery>
{
    public GetMTMReferListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetMTMReferListQueryHandler : IRequestHandler<GetMTMReferListQuery, PaginatedList<MTMReferViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMReferListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MTMReferViewModel>> Handle(GetMTMReferListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MTMRefers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.HospitalName != null && x.HospitalName.Contains(term)));
        }

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<MTMReferViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
