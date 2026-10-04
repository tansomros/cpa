namespace BigLion.CPA.Application.Features.Prefixs.Commands.Delete;

public class DeletePrefixCommandValidator : AbstractValidator<DeletePrefixCommand>
{
    public DeletePrefixCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
