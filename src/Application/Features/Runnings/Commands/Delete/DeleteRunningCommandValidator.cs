namespace BigLion.CPA.Application.Features.Runnings.Commands.Delete;

public class DeleteRunningCommandValidator : AbstractValidator<DeleteRunningCommand>
{
    public DeleteRunningCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}
