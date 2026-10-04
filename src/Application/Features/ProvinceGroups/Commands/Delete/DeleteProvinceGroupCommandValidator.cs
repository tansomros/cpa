namespace BigLion.CPA.Application.Features.ProvinceGroups.Commands.Delete;

public class DeleteProvinceGroupCommandValidator : AbstractValidator<DeleteProvinceGroupCommand>
{
    public DeleteProvinceGroupCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
