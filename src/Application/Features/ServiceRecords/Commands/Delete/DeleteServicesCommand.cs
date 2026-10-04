using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.ServiceRecords.Commands.Delete;

public class DeleteServicesCommand : IRequest<Unit>
{
    public long itemID { get; set; }
}

public class DeleteServicesCommandHandler : IRequestHandler<DeleteServicesCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteServicesCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteServicesCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Services
            .FirstOrDefaultAsync(x => x.itemID == request.itemID, cancellationToken)
            ?? throw new NotFoundException("Services", request.itemID);

        _context.Services.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
