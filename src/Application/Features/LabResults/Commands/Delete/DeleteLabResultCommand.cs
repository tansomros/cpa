using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.LabResults.Commands.Delete;

public class DeleteLabResultCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteLabResultCommandHandler : IRequestHandler<DeleteLabResultCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteLabResultCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteLabResultCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.LabResults
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("LabResult", request.UID);

        _context.LabResults.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
