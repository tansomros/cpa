using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.MTMRefers.Commands.Delete;

public class DeleteMTMReferCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteMTMReferCommandHandler : IRequestHandler<DeleteMTMReferCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteMTMReferCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMTMReferCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMRefers
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMRefer", request.UID);

        _context.MTMRefers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
