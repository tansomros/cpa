using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.UserRoleAssignments.Commands.Delete;

public class DeleteUserRolesCommand : IRequest<Unit>
{
    public int RoleID { get; set; }
    public int UserID { get; set; }
}

public class DeleteUserRolesCommandHandler : IRequestHandler<DeleteUserRolesCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteUserRolesCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserRolesCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserRoles.FirstOrDefaultAsync(
            x => x.RoleID == request.RoleID && x.UserID == request.UserID, cancellationToken)
            ?? throw new NotFoundException("UserRoles", $"{request.RoleID}/{request.UserID}");

        _context.UserRoles.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
