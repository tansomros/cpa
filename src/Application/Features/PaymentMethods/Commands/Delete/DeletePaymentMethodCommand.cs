using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.PaymentMethods.Commands.Delete;

public class DeletePaymentMethodCommand : IRequest<Unit>
{
    public int PaymentID { get; set; }
}

public class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeletePaymentMethodCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PaymentMethods
            .FirstOrDefaultAsync(x => x.PaymentID == request.PaymentID, cancellationToken)
            ?? throw new NotFoundException("PaymentMethod", request.PaymentID);

        _context.PaymentMethods.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
