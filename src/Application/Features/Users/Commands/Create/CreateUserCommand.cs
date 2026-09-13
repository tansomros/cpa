using Cpa.Domain.Entities;
using Cpa.Application.Common.Interfaces;

namespace Cpa.Application.Features.Users.Commands.Create;

public record CreateUserCommand : IRequest<int>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string DisplayName { get; set; }
    public required string PositionName { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{ 
    private readonly ICpaDatabaseContext _context;

    public CreateUserCommandValidator(ICpaDatabaseContext context)
    {
        _context = context;     

        RuleFor(p => p.Username).NotEmpty().WithMessage("Username เป็นค่าว่างไม่ได้");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Password เป็นค่าว่างไม่ได้");

        RuleFor(p => p.RoleId)
            .NotEmpty().WithMessage("RoleId ต้องไม่เป็นค่าว่าง")
            .NotNull().WithMessage("RoleId ต้องไม่เป็นค่า NULL");

        //RuleFor(p => p.ReportText).NotEmpty().WithMessage("ReportText เป็นค่าว่างไม่ได้");
    }      
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
