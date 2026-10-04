using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Identity.Interfaces;
using BigLion.CPA.Domain.Entities;

namespace BigLion.CPA.Application.Features.Users.Commands.Create;

public record CreateUserCommand : IRequest<int>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string DisplayName { get; set; }
    public required string PositionName { get; set; }
    public string? Email { get; set; }
    public int? PharmacyId { get; set; }
    public required int RoleId { get; set; }
    public bool IsActive { get; set; }
}


public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(ICpaDatabaseContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var User = new User(
            request.Username,
            request.Password,
            request.DisplayName,
            request.PositionName,
            request.Email,
            request.PharmacyId,
            request.RoleId
            )
        {
            IsActive = request.IsActive,
        };

        User.ChangePassword(_passwordHasher.Hash(User, request.Password));

        await _context.Users.AddAsync(User, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        if (!request.IsActive)
        {
            User.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
        }

        return User.Id;
    }
}
