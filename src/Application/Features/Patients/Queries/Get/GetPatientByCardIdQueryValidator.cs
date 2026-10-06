namespace BigLion.CPA.Application.Features.Patients.Queries.Get;

public class GetPatientByCardIdQueryValidator : AbstractValidator<GetPatientByCardIdQuery>
{
    public GetPatientByCardIdQueryValidator()
    {
        RuleFor(x => x.CardId).NotEmpty().WithMessage("เลขบัตรต้องไม่ว่าง");
    }
}
