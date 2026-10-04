using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.MTMBehaviors.Commands.Delete;

public class DeleteMTMBehaviorCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteMTMBehaviorCommandHandler : IRequestHandler<DeleteMTMBehaviorCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteMTMBehaviorCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMTMBehaviorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMBehaviors
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMBehavior", request.UID);

        _context.MTMBehaviors.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
