using Cpa.Domain.Entities;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Security;

namespace Cpa.Application.Features.Systems.Commands;
[Authorize(Policy = CpaPolicies.AllowAnonymous)]
public class RoleDataInitializerCommand : IRequest<Unit> { }
public class RoleDataInitializerCommandHandler : IRequestHandler<RoleDataInitializerCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public RoleDataInitializerCommandHandler(ICpaDatabaseContext BigLionDatabaseContext)
    {
        _context = BigLionDatabaseContext;
    }

    public async Task<Unit> Handle(RoleDataInitializerCommand request, CancellationToken cancellationToken)
    {
        await SeedRoles(cancellationToken); 
        return Unit.Value;
    }

    private async Task SeedRoles(CancellationToken cancellationToken)
    {
        if (await _context.Roles.AnyAsync(cancellationToken))
        {
            return;
        }

        var Role = new[]
        {
            new Role(1,"ADMINISTRATOR"),
            new Role(2,"OFFICER"),          
            new Role(3,"MANAGER"),
            new Role(4,"USER"),
        };

        await _context.Roles.AddRangeAsync(Role, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }     
} 
