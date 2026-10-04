using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.UserLogFiles.Commands.Delete;

public class DeleteUserLogFileCommand : IRequest<Unit>
{
    public long LogID { get; set; }
}

public class DeleteUserLogFileCommandHandler : IRequestHandler<DeleteUserLogFileCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteUserLogFileCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserLogFileCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserLogFiles
            .FirstOrDefaultAsync(x => x.LogID == request.LogID, cancellationToken)
            ?? throw new NotFoundException("UserLogFile", request.LogID);

        _context.UserLogFiles.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
