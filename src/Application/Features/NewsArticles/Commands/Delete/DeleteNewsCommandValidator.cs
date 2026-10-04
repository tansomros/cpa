namespace BigLion.CPA.Application.Features.NewsArticles.Commands.Delete;

public class DeleteNewsCommandValidator : AbstractValidator<DeleteNewsCommand>
{
    public DeleteNewsCommandValidator()
    {
        RuleFor(x => x.NewsID).GreaterThan(0);
    }
}
