using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.PaymentConfigs.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.PaymentConfig;

namespace BigLion.CPA.Application.Features.PaymentConfigs.Queries.Get;

public class GetPaymentConfigQuery : IRequest<PaymentConfigViewModel>
{
    public int itemID { get; set; }
}

public class GetPaymentConfigQueryHandler : IRequestHandler<GetPaymentConfigQuery, PaymentConfigViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPaymentConfigQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaymentConfigViewModel> Handle(GetPaymentConfigQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.PaymentConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.itemID == request.itemID, cancellationToken)
            ?? throw new NotFoundException("PaymentConfig", request.itemID);

        return _mapper.Map<PaymentConfigViewModel>(entity);
    }
}
