using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.LabItems.Commands.Delete;

public class DeleteLabItemCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteLabItemCommandHandler : IRequestHandler<DeleteLabItemCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteLabItemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteLabItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.LabItems
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("LabItem", request.UID);

        _context.LabItems.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
