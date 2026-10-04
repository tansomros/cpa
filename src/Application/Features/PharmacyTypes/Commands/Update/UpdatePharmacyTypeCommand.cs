using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.PharmacyType;

namespace BigLion.CPA.Application.Features.PharmacyTypes.Commands.Update;

public class UpdatePharmacyTypeCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Sort { get; set; }
}

public class UpdatePharmacyTypeCommandValidator : AbstractValidator<UpdatePharmacyTypeCommand>
{
    public UpdatePharmacyTypeCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdatePharmacyTypeCommandHandler : IRequestHandler<UpdatePharmacyTypeCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdatePharmacyTypeCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePharmacyTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PharmacyTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeleteFlag != true, cancellationToken)
            ?? throw new NotFoundException("PharmacyType", request.Id);

        entity.IsActive = request.IsActive;
        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Sort = request.Sort;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
