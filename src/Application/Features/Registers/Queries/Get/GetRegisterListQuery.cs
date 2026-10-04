using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Registers.ViewModels;

namespace BigLion.CPA.Application.Features.Registers.Queries.Get;

public class GetRegisterListQuery : IRequest<PaginatedList<RegisterViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetRegisterListQueryValidator : AbstractValidator<GetRegisterListQuery>
{
    public GetRegisterListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetRegisterListQueryHandler : IRequestHandler<GetRegisterListQuery, PaginatedList<RegisterViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetRegisterListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RegisterViewModel>> Handle(GetRegisterListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Registers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.LocationName != null && x.LocationName.Contains(term)));
        }

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<RegisterViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
