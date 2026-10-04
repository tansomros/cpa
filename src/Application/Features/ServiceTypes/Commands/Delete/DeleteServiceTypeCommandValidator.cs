namespace BigLion.CPA.Application.Features.ServiceTypes.Commands.Delete;

public class DeleteServiceTypeCommandValidator : AbstractValidator<DeleteServiceTypeCommand>
{
    public DeleteServiceTypeCommandValidator()
    {
        RuleFor(x => x.ServiceTypeID).NotEmpty();
    }
}
