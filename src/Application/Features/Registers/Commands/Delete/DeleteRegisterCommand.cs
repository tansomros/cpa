using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Registers.Commands.Delete;

public class DeleteRegisterCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteRegisterCommandHandler : IRequestHandler<DeleteRegisterCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteRegisterCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRegisterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Registers
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("Register", request.UID);

        _context.Registers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
