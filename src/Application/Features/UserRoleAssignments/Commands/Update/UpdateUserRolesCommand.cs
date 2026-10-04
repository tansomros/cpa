using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.UserRoles;

namespace BigLion.CPA.Application.Features.UserRoleAssignments.Commands.Update;

public class UpdateUserRolesCommand : IRequest<Unit>
{
    public int RoleID { get; set; }
    public int UserID { get; set; }
    public int? isActive { get; set; }
    public string? UpdBy { get; set; }
    public DateTime? UpdDate { get; set; }
}

public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateUserRolesCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserRoles.FirstOrDefaultAsync(
            x => x.RoleID == request.RoleID && x.UserID == request.UserID, cancellationToken)
            ?? throw new NotFoundException("UserRoles", $"{request.RoleID}/{request.UserID}");
        entity.isActive = request.isActive;
        entity.UpdBy = request.UpdBy;
        entity.UpdDate = request.UpdDate;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
