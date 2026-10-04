using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.LabUOM;

namespace BigLion.CPA.Application.Features.LabUOMs.Commands.Update;

public class UpdateLabUOMCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public string? Descriptions { get; set; }
}

public class UpdateLabUOMCommandValidator : AbstractValidator<UpdateLabUOMCommand>
{
    public UpdateLabUOMCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateLabUOMCommandHandler : IRequestHandler<UpdateLabUOMCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateLabUOMCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLabUOMCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.LabUOMs
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("LabUOM", request.UID);

        entity.Descriptions = request.Descriptions;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
