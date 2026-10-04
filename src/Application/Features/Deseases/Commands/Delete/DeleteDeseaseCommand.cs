using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Deseases.Commands.Delete;

public class DeleteDeseaseCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteDeseaseCommandHandler : IRequestHandler<DeleteDeseaseCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteDeseaseCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDeseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Deseases
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("Desease", request.UID);

        _context.Deseases.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
