namespace BigLion.CPA.Application.Features.Users.Queries.Get;

public class GetUserListQueryValidator : AbstractValidator<GetUserListQuery>
{
    public GetUserListQueryValidator()
    {
        RuleFor(x => x.visitNumber).NotEmpty();
    }
}
