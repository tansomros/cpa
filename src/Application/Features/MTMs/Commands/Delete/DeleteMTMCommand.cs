using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.MTMs.Commands.Delete;

public class DeleteMTMCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteMTMCommandHandler : IRequestHandler<DeleteMTMCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteMTMCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMTMCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMs
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTM", request.UID);

        _context.MTMs.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
