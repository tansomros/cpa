#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceValues.Commands.Update;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class UpdateReferenceValueCommandValidator : AbstractValidator<UpdateReferenceValueCommand>
{
    public UpdateReferenceValueCommandValidator()
    {
        RuleFor(p => p.Id).NotNull().WithMessage("Id ต้องไม่เป็นค่าว่าง");
        RuleFor(p => p.ValueCode).NotNull().WithMessage("Value Code ต้องไม่เป็นค่าว่าง");

    }
}
