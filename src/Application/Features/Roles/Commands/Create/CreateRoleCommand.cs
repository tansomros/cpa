using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Role;

namespace BigLion.CPA.Application.Features.Roles.Commands.Create;

public class CreateRoleCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int Sort { get; set; }
}

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateRoleCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(0, request.Name, request.IsActive, request.Sort);
        entity.Name = request.Name;
        entity.IsActive = request.IsActive;
        entity.Sort = request.Sort;
        await _context.Roles.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
