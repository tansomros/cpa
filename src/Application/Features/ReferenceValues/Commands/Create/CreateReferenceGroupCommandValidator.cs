#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceValues.Commands.Create;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class CreateReferenceValueCommandValidator : AbstractValidator<CreateReferenceValueCommand>
{
    public CreateReferenceValueCommandValidator()
    {
        RuleFor(p => p.ValueCode).NotNull().WithMessage("ValueCode ต้องไม่เป็นค่าว่าง");
    }
}
