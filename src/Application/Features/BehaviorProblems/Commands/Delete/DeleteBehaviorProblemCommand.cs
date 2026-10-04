using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.BehaviorProblems.Commands.Delete;

public class DeleteBehaviorProblemCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteBehaviorProblemCommandHandler : IRequestHandler<DeleteBehaviorProblemCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteBehaviorProblemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteBehaviorProblemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BehaviorProblems
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("BehaviorProblem", request.UID);

        _context.BehaviorProblems.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
