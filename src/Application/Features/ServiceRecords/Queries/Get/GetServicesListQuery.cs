using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.ServiceRecords.ViewModels;

namespace BigLion.CPA.Application.Features.ServiceRecords.Queries.Get;

public class GetServicesListQuery : IRequest<PaginatedList<ServicesViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetServicesListQueryValidator : AbstractValidator<GetServicesListQuery>
{
    public GetServicesListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetServicesListQueryHandler : IRequestHandler<GetServicesListQuery, PaginatedList<ServicesViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetServicesListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ServicesViewModel>> Handle(GetServicesListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Services.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.HospitalName != null && x.HospitalName.Contains(term)));
        }

        return await query
            .OrderBy(x => x.itemID)
            .ProjectTo<ServicesViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
