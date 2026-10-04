using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.PharmacyGroup;

namespace BigLion.CPA.Application.Features.PharmacyGroups.Commands.Create;

public class CreatePharmacyGroupCommand : IRequest<int>
{
    public bool IsActive { get; set; } = true;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Sort { get; set; }
}

public class CreatePharmacyGroupCommandValidator : AbstractValidator<CreatePharmacyGroupCommand>
{
    public CreatePharmacyGroupCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreatePharmacyGroupCommandHandler : IRequestHandler<CreatePharmacyGroupCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreatePharmacyGroupCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePharmacyGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(request.Code, request.Name, request.Sort);
        entity.IsActive = request.IsActive;
        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Sort = request.Sort;
        await _context.PharmacyGroups.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
