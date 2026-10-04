using BigLion.CPA.Domain.Entities;
using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Application.Features.Systems.Commands;
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
            new User("host","AQAAAAEAACcQAAAAEDJirZQCGuiZ5HI0fDDQNNiCQBSAMXSpy/IHCPizqejnsuxEVe8AMswL16Uy3nDs1g==","Host","ผู้ดูแลระบบ","",null,9),//4321
            new User("admin","AQAAAAEAACcQAAAAEDJirZQCGuiZ5HI0fDDQNNiCQBSAMXSpy/IHCPizqejnsuxEVe8AMswL16Uy3nDs1g==","Administrator","Admin","",null,8), 
        };

        await _context.Users.AddRangeAsync(User, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
     
}
