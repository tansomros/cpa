using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.UserRoles;

namespace BigLion.CPA.Application.Features.UserRoleAssignments.Commands.Create;

public class CreateUserRolesCommand : IRequest<Unit>
{
    public int RoleID { get; set; }
    public int UserID { get; set; }
    public int? isActive { get; set; }
    public string? UpdBy { get; set; }
    public DateTime? UpdDate { get; set; }
}

public class CreateUserRolesCommandValidator : AbstractValidator<CreateUserRolesCommand>
{
    public CreateUserRolesCommandValidator()
    {
        RuleFor(x => x.RoleID).GreaterThan(0);
        RuleFor(x => x.UserID).GreaterThan(0);
    }
}

public class CreateUserRolesCommandHandler : IRequestHandler<CreateUserRolesCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public CreateUserRolesCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.RoleID = request.RoleID;
        entity.UserID = request.UserID;
        entity.isActive = request.isActive;
        entity.UpdBy = request.UpdBy;
        entity.UpdDate = request.UpdDate;
        await _context.UserRoles.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
