#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceValues.Commands.Delete;

public class DeleteReferenceValueCommandValidator : AbstractValidator<DeleteReferenceValueCommand>
{
    public DeleteReferenceValueCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
