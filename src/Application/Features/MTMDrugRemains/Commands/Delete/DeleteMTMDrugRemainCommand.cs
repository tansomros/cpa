using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.MTMDrugRemains.Commands.Delete;

public class DeleteMTMDrugRemainCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteMTMDrugRemainCommandHandler : IRequestHandler<DeleteMTMDrugRemainCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteMTMDrugRemainCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMTMDrugRemainCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMDrugRemains
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMDrugRemain", request.UID);

        _context.MTMDrugRemains.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
