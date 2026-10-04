using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Domain.Entities;
using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Application.Features.Users.Commands.Delete;

public record DeleteUserCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteUserCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var User = await _context.Users.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Users), request.Id);

        _context.Users.Remove(User);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
