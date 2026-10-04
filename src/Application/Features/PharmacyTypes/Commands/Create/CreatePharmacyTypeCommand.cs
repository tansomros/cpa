using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.PharmacyType;

namespace BigLion.CPA.Application.Features.PharmacyTypes.Commands.Create;

public class CreatePharmacyTypeCommand : IRequest<int>
{
    public bool IsActive { get; set; } = true;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Sort { get; set; }
}

public class CreatePharmacyTypeCommandValidator : AbstractValidator<CreatePharmacyTypeCommand>
{
    public CreatePharmacyTypeCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreatePharmacyTypeCommandHandler : IRequestHandler<CreatePharmacyTypeCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreatePharmacyTypeCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePharmacyTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(request.Code, request.Name, request.Sort);
        entity.IsActive = request.IsActive;
        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Sort = request.Sort;
        await _context.PharmacyTypes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
