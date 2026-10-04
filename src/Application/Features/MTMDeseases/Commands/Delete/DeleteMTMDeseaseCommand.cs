using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.MTMDeseases.Commands.Delete;

public class DeleteMTMDeseaseCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteMTMDeseaseCommandHandler : IRequestHandler<DeleteMTMDeseaseCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteMTMDeseaseCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMTMDeseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMDeseases
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMDesease", request.UID);

        _context.MTMDeseases.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
