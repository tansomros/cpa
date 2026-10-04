using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Banks.Commands.Delete;

public class DeleteBankCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeleteBankCommandHandler : IRequestHandler<DeleteBankCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteBankCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteBankCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Banks
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Bank", request.Id);

        _context.Banks.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
