using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.PaymentMethods.ViewModels;

namespace BigLion.CPA.Application.Features.PaymentMethods.Queries.Get;

public class GetPaymentMethodListQuery : IRequest<PaginatedList<PaymentMethodViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetPaymentMethodListQueryValidator : AbstractValidator<GetPaymentMethodListQuery>
{
    public GetPaymentMethodListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetPaymentMethodListQueryHandler : IRequestHandler<GetPaymentMethodListQuery, PaginatedList<PaymentMethodViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPaymentMethodListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PaymentMethodViewModel>> Handle(GetPaymentMethodListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.PaymentMethods.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => (x.PaymentName != null && x.PaymentName.Contains(term)));
        }

        return await query
            .OrderBy(x => x.PaymentID)
            .ProjectTo<PaymentMethodViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
