namespace BigLion.CPA.Application.Features.UserLogFiles.Commands.Delete;

public class DeleteUserLogFileCommandValidator : AbstractValidator<DeleteUserLogFileCommand>
{
    public DeleteUserLogFileCommandValidator()
    {
        RuleFor(x => x.LogID).GreaterThan(0);
    }
}
