namespace BigLion.CPA.Application.Features.RunningConfigs.Commands.Delete;

public class DeleteRunningConfigCommandValidator : AbstractValidator<DeleteRunningConfigCommand>
{
    public DeleteRunningConfigCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}
