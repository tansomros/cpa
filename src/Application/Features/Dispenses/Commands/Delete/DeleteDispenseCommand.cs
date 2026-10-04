using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Dispenses.Commands.Delete;

public class DeleteDispenseCommand : IRequest<Unit>
{
    public long UID { get; set; }
}

public class DeleteDispenseCommandHandler : IRequestHandler<DeleteDispenseCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteDispenseCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDispenseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Dispenses
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("Dispense", request.UID);

        _context.Dispenses.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
