using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.LabResults.ViewModels;

namespace BigLion.CPA.Application.Features.LabResults.Queries.Get;

public class GetLabResultListQuery : IRequest<PaginatedList<LabResultViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class GetLabResultListQueryValidator : AbstractValidator<GetLabResultListQuery>
{
    public GetLabResultListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetLabResultListQueryHandler : IRequestHandler<GetLabResultListQuery, PaginatedList<LabResultViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetLabResultListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LabResultViewModel>> Handle(GetLabResultListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.LabResults.AsNoTracking();

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<LabResultViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
