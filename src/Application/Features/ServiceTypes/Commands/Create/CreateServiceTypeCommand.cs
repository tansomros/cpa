using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.ServiceType;

namespace BigLion.CPA.Application.Features.ServiceTypes.Commands.Create;

public class CreateServiceTypeCommand : IRequest<string>
{
    public string ServiceTypeID { get; set; } = string.Empty;
    public string? ServiceName { get; set; }
    public string? Descriptions { get; set; }
    public string? Status { get; set; }
    public int? ProjectID { get; set; }
}

public class CreateServiceTypeCommandValidator : AbstractValidator<CreateServiceTypeCommand>
{
    public CreateServiceTypeCommandValidator()
    {
        RuleFor(x => x.ServiceTypeID).NotEmpty();
    }
}

public class CreateServiceTypeCommandHandler : IRequestHandler<CreateServiceTypeCommand, string>
{
    private readonly ICpaDatabaseContext _context;

    public CreateServiceTypeCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateServiceTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.ServiceTypeID = request.ServiceTypeID;
        entity.ServiceName = request.ServiceName;
        entity.Descriptions = request.Descriptions;
        entity.Status = request.Status;
        entity.ProjectID = request.ProjectID;
        await _context.ServiceTypes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ServiceTypeID;
    }
}
