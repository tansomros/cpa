using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.PaymentConfigs.ViewModels;

namespace BigLion.CPA.Application.Features.PaymentConfigs.Queries.Get;

public class GetPaymentConfigListQuery : IRequest<PaginatedList<PaymentConfigViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class GetPaymentConfigListQueryValidator : AbstractValidator<GetPaymentConfigListQuery>
{
    public GetPaymentConfigListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetPaymentConfigListQueryHandler : IRequestHandler<GetPaymentConfigListQuery, PaginatedList<PaymentConfigViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPaymentConfigListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PaymentConfigViewModel>> Handle(GetPaymentConfigListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.PaymentConfigs.AsNoTracking();

        return await query
            .OrderBy(x => x.itemID)
            .ProjectTo<PaymentConfigViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
