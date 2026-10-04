namespace BigLion.CPA.Application.Features.SubDistricts.Commands.Delete;

public class DeleteSubDistrictCommandValidator : AbstractValidator<DeleteSubDistrictCommand>
{
    public DeleteSubDistrictCommandValidator()
    {
        RuleFor(x => x.SubDistrictId).NotEmpty();
    }
}
