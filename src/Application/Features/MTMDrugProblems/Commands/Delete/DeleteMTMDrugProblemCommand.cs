using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.MTMDrugProblems.Commands.Delete;

public class DeleteMTMDrugProblemCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteMTMDrugProblemCommandHandler : IRequestHandler<DeleteMTMDrugProblemCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteMTMDrugProblemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMTMDrugProblemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMDrugProblems
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMDrugProblem", request.UID);

        _context.MTMDrugProblems.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
