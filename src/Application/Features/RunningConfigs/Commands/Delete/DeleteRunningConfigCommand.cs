using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.RunningConfigs.Commands.Delete;

public class DeleteRunningConfigCommand : IRequest<Unit>
{
    public string Code { get; set; } = string.Empty;
}

public class DeleteRunningConfigCommandHandler : IRequestHandler<DeleteRunningConfigCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteRunningConfigCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRunningConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.RunningConfigs
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("RunningConfig", request.Code);

        _context.RunningConfigs.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
