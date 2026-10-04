#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceGroups.Commands.Update;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class UpdateReferenceGroupCommandValidator : AbstractValidator<UpdateReferenceGroupCommand>
{
    public UpdateReferenceGroupCommandValidator()
    {
        RuleFor(p => p.Id).NotNull().WithMessage("Id ต้องไม่เป็นค่าว่าง");
        RuleFor(p => p.Code).NotNull().WithMessage("Code ต้องไม่เป็นค่าว่าง");

    }
}
