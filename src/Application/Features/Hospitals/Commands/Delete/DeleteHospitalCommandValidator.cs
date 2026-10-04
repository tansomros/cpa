namespace BigLion.CPA.Application.Features.Hospitals.Commands.Delete;

public class DeleteHospitalCommandValidator : AbstractValidator<DeleteHospitalCommand>
{
    public DeleteHospitalCommandValidator()
    {
        RuleFor(x => x.HospitalUID).GreaterThan(0);
    }
}
