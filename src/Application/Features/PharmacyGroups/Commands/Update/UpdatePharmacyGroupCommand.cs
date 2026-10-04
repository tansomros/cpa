using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.PharmacyGroup;

namespace BigLion.CPA.Application.Features.PharmacyGroups.Commands.Update;

public class UpdatePharmacyGroupCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Sort { get; set; }
}

public class UpdatePharmacyGroupCommandValidator : AbstractValidator<UpdatePharmacyGroupCommand>
{
    public UpdatePharmacyGroupCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdatePharmacyGroupCommandHandler : IRequestHandler<UpdatePharmacyGroupCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdatePharmacyGroupCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePharmacyGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PharmacyGroups
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeleteFlag != true, cancellationToken)
            ?? throw new NotFoundException("PharmacyGroup", request.Id);

        entity.IsActive = request.IsActive;
        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Sort = request.Sort;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
