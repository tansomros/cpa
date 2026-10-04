using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.PaymentMethod;

namespace BigLion.CPA.Application.Features.PaymentMethods.Commands.Update;

public class UpdatePaymentMethodCommand : IRequest<Unit>
{
    public int PaymentID { get; set; }
    public string? PaymentName { get; set; }
    public double? Amount { get; set; }
    public long? EffectiveTo { get; set; }
    public string? StatusFlag { get; set; }
    public string? ServiceTypeID { get; set; }
}

public class UpdatePaymentMethodCommandValidator : AbstractValidator<UpdatePaymentMethodCommand>
{
    public UpdatePaymentMethodCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdatePaymentMethodCommandHandler : IRequestHandler<UpdatePaymentMethodCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdatePaymentMethodCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PaymentMethods
            .FirstOrDefaultAsync(x => x.PaymentID == request.PaymentID, cancellationToken)
            ?? throw new NotFoundException("PaymentMethod", request.PaymentID);

        entity.PaymentName = request.PaymentName;
        entity.Amount = request.Amount;
        entity.EffectiveTo = request.EffectiveTo;
        entity.StatusFlag = request.StatusFlag;
        entity.ServiceTypeID = request.ServiceTypeID;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
