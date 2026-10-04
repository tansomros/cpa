using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.PaymentMethods.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.PaymentMethod;

namespace BigLion.CPA.Application.Features.PaymentMethods.Queries.Get;

public class GetPaymentMethodQuery : IRequest<PaymentMethodViewModel>
{
    public int PaymentID { get; set; }
}

public class GetPaymentMethodQueryHandler : IRequestHandler<GetPaymentMethodQuery, PaymentMethodViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPaymentMethodQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaymentMethodViewModel> Handle(GetPaymentMethodQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.PaymentMethods
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PaymentID == request.PaymentID, cancellationToken)
            ?? throw new NotFoundException("PaymentMethod", request.PaymentID);

        return _mapper.Map<PaymentMethodViewModel>(entity);
    }
}
