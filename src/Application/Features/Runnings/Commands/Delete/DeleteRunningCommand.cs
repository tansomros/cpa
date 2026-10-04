using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Runnings.Commands.Delete;

public class DeleteRunningCommand : IRequest<Unit>
{
    public string Code { get; set; } = string.Empty;
}

public class DeleteRunningCommandHandler : IRequestHandler<DeleteRunningCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteRunningCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRunningCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Runnings
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("Running", request.Code);

        _context.Runnings.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
