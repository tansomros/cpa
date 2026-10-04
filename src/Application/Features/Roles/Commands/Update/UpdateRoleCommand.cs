using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.Role;

namespace BigLion.CPA.Application.Features.Roles.Commands.Update;

public class UpdateRoleCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Sort { get; set; }
}

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateRoleCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Role", request.Id);

        entity.Name = request.Name;
        entity.IsActive = request.IsActive;
        entity.Sort = request.Sort;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
