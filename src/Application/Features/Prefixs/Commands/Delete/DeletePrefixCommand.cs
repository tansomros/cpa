using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Prefixs.Commands.Delete;

public class DeletePrefixCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeletePrefixCommandHandler : IRequestHandler<DeletePrefixCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeletePrefixCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePrefixCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Prefixs
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Prefix", request.Id);

        _context.Prefixs.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
