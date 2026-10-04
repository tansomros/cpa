using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.ServiceType;

namespace BigLion.CPA.Application.Features.ServiceTypes.Commands.Update;

public class UpdateServiceTypeCommand : IRequest<Unit>
{
    public string ServiceTypeID { get; set; } = string.Empty;
    public string? ServiceName { get; set; }
    public string? Descriptions { get; set; }
    public string? Status { get; set; }
    public int? ProjectID { get; set; }
}

public class UpdateServiceTypeCommandValidator : AbstractValidator<UpdateServiceTypeCommand>
{
    public UpdateServiceTypeCommandValidator()
    {
        RuleFor(x => x.ServiceTypeID).NotEmpty();
    }
}

public class UpdateServiceTypeCommandHandler : IRequestHandler<UpdateServiceTypeCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateServiceTypeCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateServiceTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ServiceTypes
            .FirstOrDefaultAsync(x => x.ServiceTypeID == request.ServiceTypeID, cancellationToken)
            ?? throw new NotFoundException("ServiceType", request.ServiceTypeID);

        entity.ServiceName = request.ServiceName;
        entity.Descriptions = request.Descriptions;
        entity.Status = request.Status;
        entity.ProjectID = request.ProjectID;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
