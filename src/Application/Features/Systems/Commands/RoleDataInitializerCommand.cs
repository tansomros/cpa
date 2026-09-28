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
            new Role(1,"ร้านยา",true,1),
            new Role(2,"สิทธิ์ดูรายงาน",true,2),          
            new Role(3,"ผู้จัดการโครงการ",true,3),
            new Role(4,"Admin",true,8),
            new Role(9,"ผู้ดูแลระบบ (Host)",true,9)
        };

        await _context.Roles.AddRangeAsync(Role, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }     
} 
