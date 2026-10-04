using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.LabUOMs.Commands.Delete;

public class DeleteLabUOMCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteLabUOMCommandHandler : IRequestHandler<DeleteLabUOMCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteLabUOMCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteLabUOMCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.LabUOMs
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("LabUOM", request.UID);

        _context.LabUOMs.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
