#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceGroups.Commands.Delete;

public class DeleteReferenceGroupCommandValidator : AbstractValidator<DeleteReferenceGroupCommand>
{
    public DeleteReferenceGroupCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
