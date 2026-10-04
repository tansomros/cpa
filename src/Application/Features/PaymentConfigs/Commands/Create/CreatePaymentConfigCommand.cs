using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.PaymentConfig;

namespace BigLion.CPA.Application.Features.PaymentConfigs.Commands.Create;

public class CreatePaymentConfigCommand : IRequest<int>
{
    public string? ProvinceID { get; set; }
    public int? PaymentID { get; set; }
    public long? EffectiveTo { get; set; }
    public string? StatusFlag { get; set; }
    public int? ProjectID { get; set; }
}

public class CreatePaymentConfigCommandValidator : AbstractValidator<CreatePaymentConfigCommand>
{
    public CreatePaymentConfigCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreatePaymentConfigCommandHandler : IRequestHandler<CreatePaymentConfigCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreatePaymentConfigCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePaymentConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.ProvinceID = request.ProvinceID;
        entity.PaymentID = request.PaymentID;
        entity.EffectiveTo = request.EffectiveTo;
        entity.StatusFlag = request.StatusFlag;
        entity.ProjectID = request.ProjectID;
        await _context.PaymentConfigs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.itemID;
    }
}
