using Cpa.Domain.Entities;
using Cpa.Application.Common.Interfaces;

namespace Cpa.Application.Features.Systems.Commands;
public class UserDataInitializerCommand : IRequest<Unit> { }
public class UserDataInitializerCommandHandler : IRequestHandler<UserDataInitializerCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UserDataInitializerCommandHandler(ICpaDatabaseContext BigLionDatabaseContext)
    {
        _context = BigLionDatabaseContext;
    }

    public async Task<Unit> Handle(UserDataInitializerCommand request, CancellationToken cancellationToken)
    {
        await SeedUsers(cancellationToken); 
        return Unit.Value;
    }

    private async Task SeedUsers(CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var User = new[]
        {           
            new User("host","AQAAAAEAACcQAAAAEDJirZQCGuiZ5HI0fDDQNNiCQBSAMXSpy/IHCPizqejnsuxEVe8AMswL16Uy3nDs1g==","Host","ผู้ดูแลระบบ","",0,9),//4321
            new User("admin","AQAAAAEAACcQAAAAEDJirZQCGuiZ5HI0fDDQNNiCQBSAMXSpy/IHCPizqejnsuxEVe8AMswL16Uy3nDs1g==","Administrator","Admin","",0,8), 
        };

        await _context.Users.AddRangeAsync(User, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
     
}
