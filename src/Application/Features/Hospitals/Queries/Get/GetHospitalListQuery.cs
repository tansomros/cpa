using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Hospitals.ViewModels;

namespace BigLion.CPA.Application.Features.Hospitals.Queries.Get;

public class GetHospitalListQuery : IRequest<PaginatedList<HospitalViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetHospitalListQueryValidator : AbstractValidator<GetHospitalListQuery>
{
    public GetHospitalListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetHospitalListQueryHandler : IRequestHandler<GetHospitalListQuery, PaginatedList<HospitalViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetHospitalListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<HospitalViewModel>> Handle(GetHospitalListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Hospitals.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.HospitalName != null && x.HospitalName.Contains(term)));
        }

        return await query
            .OrderBy(x => x.HospitalUID)
            .ProjectTo<HospitalViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
