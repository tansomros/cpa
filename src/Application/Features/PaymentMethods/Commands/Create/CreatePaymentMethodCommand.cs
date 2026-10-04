using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.PaymentMethod;

namespace BigLion.CPA.Application.Features.PaymentMethods.Commands.Create;

public class CreatePaymentMethodCommand : IRequest<int>
{
    public string? PaymentName { get; set; }
    public double? Amount { get; set; }
    public long? EffectiveTo { get; set; }
    public string? StatusFlag { get; set; }
    public string? ServiceTypeID { get; set; }
}

public class CreatePaymentMethodCommandValidator : AbstractValidator<CreatePaymentMethodCommand>
{
    public CreatePaymentMethodCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreatePaymentMethodCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.PaymentName = request.PaymentName;
        entity.Amount = request.Amount;
        entity.EffectiveTo = request.EffectiveTo;
        entity.StatusFlag = request.StatusFlag;
        entity.ServiceTypeID = request.ServiceTypeID;
        await _context.PaymentMethods.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.PaymentID;
    }
}
