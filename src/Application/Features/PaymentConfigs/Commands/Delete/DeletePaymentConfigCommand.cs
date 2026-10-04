using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.PaymentConfigs.Commands.Delete;

public class DeletePaymentConfigCommand : IRequest<Unit>
{
    public int itemID { get; set; }
}

public class DeletePaymentConfigCommandHandler : IRequestHandler<DeletePaymentConfigCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeletePaymentConfigCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePaymentConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PaymentConfigs
            .FirstOrDefaultAsync(x => x.itemID == request.itemID, cancellationToken)
            ?? throw new NotFoundException("PaymentConfig", request.itemID);

        _context.PaymentConfigs.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
