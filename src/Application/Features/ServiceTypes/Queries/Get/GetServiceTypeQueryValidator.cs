namespace BigLion.CPA.Application.Features.ServiceTypes.Queries.Get;

public class GetServiceTypeQueryValidator : AbstractValidator<GetServiceTypeQuery>
{
    public GetServiceTypeQueryValidator()
    {
        RuleFor(x => x.ServiceTypeID).NotEmpty();
    }
}
