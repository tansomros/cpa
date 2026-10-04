namespace BigLion.CPA.Application.Features.Districts.Commands.Delete;

public class DeleteDistrictCommandValidator : AbstractValidator<DeleteDistrictCommand>
{
    public DeleteDistrictCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
