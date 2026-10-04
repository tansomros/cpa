using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.PaymentConfig;

namespace BigLion.CPA.Application.Features.PaymentConfigs.Commands.Update;

public class UpdatePaymentConfigCommand : IRequest<Unit>
{
    public int itemID { get; set; }
    public string? ProvinceID { get; set; }
    public int? PaymentID { get; set; }
    public long? EffectiveTo { get; set; }
    public string? StatusFlag { get; set; }
    public int? ProjectID { get; set; }
}

public class UpdatePaymentConfigCommandValidator : AbstractValidator<UpdatePaymentConfigCommand>
{
    public UpdatePaymentConfigCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdatePaymentConfigCommandHandler : IRequestHandler<UpdatePaymentConfigCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdatePaymentConfigCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePaymentConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PaymentConfigs
            .FirstOrDefaultAsync(x => x.itemID == request.itemID, cancellationToken)
            ?? throw new NotFoundException("PaymentConfig", request.itemID);

        entity.ProvinceID = request.ProvinceID;
        entity.PaymentID = request.PaymentID;
        entity.EffectiveTo = request.EffectiveTo;
        entity.StatusFlag = request.StatusFlag;
        entity.ProjectID = request.ProjectID;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
