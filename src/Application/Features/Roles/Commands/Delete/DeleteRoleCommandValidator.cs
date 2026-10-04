namespace BigLion.CPA.Application.Features.Roles.Commands.Delete;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
