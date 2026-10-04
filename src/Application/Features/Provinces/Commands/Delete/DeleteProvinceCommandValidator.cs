namespace BigLion.CPA.Application.Features.Provinces.Commands.Delete;

public class DeleteProvinceCommandValidator : AbstractValidator<DeleteProvinceCommand>
{
    public DeleteProvinceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
