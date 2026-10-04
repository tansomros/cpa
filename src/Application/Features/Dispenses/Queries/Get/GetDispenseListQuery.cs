using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Dispenses.ViewModels;

namespace BigLion.CPA.Application.Features.Dispenses.Queries.Get;

public class GetDispenseListQuery : IRequest<PaginatedList<DispenseViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class GetDispenseListQueryValidator : AbstractValidator<GetDispenseListQuery>
{
    public GetDispenseListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetDispenseListQueryHandler : IRequestHandler<GetDispenseListQuery, PaginatedList<DispenseViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDispenseListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DispenseViewModel>> Handle(GetDispenseListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Dispenses.AsNoTracking();

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<DispenseViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
