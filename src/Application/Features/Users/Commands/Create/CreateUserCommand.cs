using Cpa.Domain.Entities;
using Cpa.Application.Common.Interfaces;

namespace Cpa.Application.Features.Users.Commands.Create;

public record CreateUserCommand : IRequest<int>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string DisplayName { get; set; }
    public required string PositionName { get; set; }
    public string? Email { get; set; }
    public required int PharmacyId { get; set; }
    public required int RoleId { get; set; }
    public bool IsActive { get; set; }
}


public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly ICpaDatabaseContext _context;
    public CreateUserCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
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


        await _context.Users.AddAsync(User, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return User.Id;
    }
}
