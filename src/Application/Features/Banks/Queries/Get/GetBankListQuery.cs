using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Banks.ViewModels;

namespace BigLion.CPA.Application.Features.Banks.Queries.Get;

public class GetBankListQuery : IRequest<PaginatedList<BankViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetBankListQueryValidator : AbstractValidator<GetBankListQuery>
{
    public GetBankListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetBankListQueryHandler : IRequestHandler<GetBankListQuery, PaginatedList<BankViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetBankListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BankViewModel>> Handle(GetBankListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Banks.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.Name != null && x.Name.Contains(term)) || (x.Code != null && x.Code.Contains(term)));
        }

        return await query
            .OrderBy(x => x.Id)
            .ProjectTo<BankViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
