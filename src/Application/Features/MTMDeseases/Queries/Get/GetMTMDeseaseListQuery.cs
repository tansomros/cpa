using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMDeseases.ViewModels;

namespace BigLion.CPA.Application.Features.MTMDeseases.Queries.Get;

public class GetMTMDeseaseListQuery : IRequest<PaginatedList<MTMDeseaseViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class GetMTMDeseaseListQueryValidator : AbstractValidator<GetMTMDeseaseListQuery>
{
    public GetMTMDeseaseListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetMTMDeseaseListQueryHandler : IRequestHandler<GetMTMDeseaseListQuery, PaginatedList<MTMDeseaseViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMDeseaseListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MTMDeseaseViewModel>> Handle(GetMTMDeseaseListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MTMDeseases.AsNoTracking();

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<MTMDeseaseViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
