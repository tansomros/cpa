#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceGroups.Commands.Create;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class CreateReferenceGroupCommandValidator : AbstractValidator<CreateReferenceGroupCommand>
{
    public CreateReferenceGroupCommandValidator()
    {
        RuleFor(p => p.Code).NotEmpty().WithMessage("Code ต้องไม่เป็นค่าว่าง");
        RuleFor(p => p.Descriptions).NotNull().WithMessage("Description ต้องไม่เป็นค่าว่าง");
    }
}
