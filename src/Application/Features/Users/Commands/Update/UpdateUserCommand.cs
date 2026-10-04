using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Domain.Entities;
using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Application.Features.Users.Commands.Update;

public record UpdateUserCommand : IRequest<Unit>
{
    public required int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? DisplayName { get; set; }
    public string? PositionName { get; set; }
    //public DateTime? LastLog { get; set; }
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

        //RuleFor(p => p.ReportText).NotEmpty().WithMessage("ReportText เป็นค่าว่างไม่ได้");
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

    public UpdateCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var User = await _context.Users.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Users), request.Id);

        //User.Name = request.Name;
        //User.PositionName = request.PositionName;
        //User.RoleId = request.RoleId;
        //User.IsActive = request.IsActive;

        _context.Users.Update(User);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;

    }
}
