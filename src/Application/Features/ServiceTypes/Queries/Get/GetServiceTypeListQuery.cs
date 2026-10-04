using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.ServiceTypes.ViewModels;

namespace BigLion.CPA.Application.Features.ServiceTypes.Queries.Get;

public class GetServiceTypeListQuery : IRequest<PaginatedList<ServiceTypeViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetServiceTypeListQueryValidator : AbstractValidator<GetServiceTypeListQuery>
{
    public GetServiceTypeListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetServiceTypeListQueryHandler : IRequestHandler<GetServiceTypeListQuery, PaginatedList<ServiceTypeViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetServiceTypeListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ServiceTypeViewModel>> Handle(GetServiceTypeListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ServiceTypes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Descriptions != null && x.Descriptions.Contains(term)) || (x.ServiceName != null && x.ServiceName.Contains(term)));
        }

        return await query
            .OrderBy(x => x.ServiceTypeID)
            .ProjectTo<ServiceTypeViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
