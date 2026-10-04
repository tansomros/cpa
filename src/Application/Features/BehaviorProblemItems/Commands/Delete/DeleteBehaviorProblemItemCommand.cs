using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.BehaviorProblemItems.Commands.Delete;

public class DeleteBehaviorProblemItemCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteBehaviorProblemItemCommandHandler : IRequestHandler<DeleteBehaviorProblemItemCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteBehaviorProblemItemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteBehaviorProblemItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BehaviorProblemItems
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("BehaviorProblemItem", request.UID);

        _context.BehaviorProblemItems.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
