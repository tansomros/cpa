using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Identity.Interfaces;

namespace BigLion.CPA.Application.Features.Users.Commands.Update;

public record UpdateUserCommand : IRequest<Unit>
{
    public required int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? DisplayName { get; set; }
    public string? PositionName { get; set; }
    public string? Email { get; set; }
    public int? PharmacyId { get; set; }
    public int? RoleId { get; set; }
    //public bool? DeleteFlag { get; set; }
    public bool? IsActive { get; set; }
    //public DateTimeOffset? CreatedOn { get; set; }
    //public DateTimeOffset? LastModified { get; set; }
}

public class UpdateCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private int? _id;
    private readonly ICpaDatabaseContext _context;

    public UpdateCommandValidator(ICpaDatabaseContext context)
    {
        _context = context;

        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Id ต้องไม่เป็นค่าว่าง")
            .NotNull().WithMessage("Id ต้องไม่เป็นค่า NULL")
            .MustAsync(UserExistAsync).WithMessage($"ไม่พบรายการ User หมายเลข {_id} ที่ท่านระบุ");

        RuleFor(p => p.Username)
            .NotEmpty().WithMessage("ชื่อผู้ใช้เป็นค่าว่างไม่ได้")
            .MaximumLength(50).WithMessage("ชื่อผู้ใช้ต้องไม่เกิน 50 ตัวอักษร")
            .MustAsync(BeUniqueUsername).WithMessage("ชื่อผู้ใช้นี้มีอยู่แล้ว");

        RuleFor(p => p.Password)
            .MinimumLength(4).WithMessage("รหัสผ่านต้องมีอย่างน้อย 4 ตัวอักษร")
            .When(p => !string.IsNullOrWhiteSpace(p.Password));

        RuleFor(p => p.DisplayName).NotEmpty().WithMessage("ชื่อที่แสดงเป็นค่าว่างไม่ได้");
        RuleFor(p => p.PositionName).NotEmpty().WithMessage("ตำแหน่งเป็นค่าว่างไม่ได้");
        RuleFor(p => p.RoleId).NotNull().GreaterThan(0).WithMessage("โปรดระบุบทบาท");
        RuleFor(p => p.PharmacyId)
            .GreaterThan(0).WithMessage("โปรดระบุร้านยา")
            .When(p => p.PharmacyId.HasValue);
    }

    private async Task<bool> BeUniqueUsername(UpdateUserCommand command, string username, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(x => x.Username == username && x.Id != command.Id, cancellationToken);
    }

    public async Task<bool> UserExistAsync(int UserId, CancellationToken cancellationToken)
    {
        _id = UserId;
        return await _context.Users.AsNoTracking().AnyAsync(x => x.Id == UserId, cancellationToken);
    }
       
}

public class UpdateCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateCommandHandler(ICpaDatabaseContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var User = await _context.Users.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

        User.Update(
            request.Username ?? User.Username,
            request.DisplayName ?? User.DisplayName,
            request.PositionName,
            request.Email,
            request.PharmacyId,
            request.RoleId ?? User.RoleId,
            request.IsActive ?? User.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Password))
            User.ChangePassword(_passwordHasher.Hash(User, request.Password));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;

    }
}
