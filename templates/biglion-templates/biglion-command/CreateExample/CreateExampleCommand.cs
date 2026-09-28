using BigLion.Cpa.Application.Common.Interfaces;

namespace BigLion.Cpa.Application.Features.BoundedContext.Examples.Commands.CreateExample;

public record CreateExampleCommand : IRequest<ReturnTypePlaceholder>
{
    // TODO: request properties
}

public class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
{
    public CreateExampleCommandValidator()
    {
        // TODO: validation rules — see .ai/coding-rules.md for message conventions
    }
}

public class CreateExampleCommandHandler : IRequestHandler<CreateExampleCommand, ReturnTypePlaceholder>
{
    private readonly ICpaDbContext _context;

    public CreateExampleCommandHandler(ICpaDbContext context)
    {
        _context = context;
    }

    public async Task<ReturnTypePlaceholder> Handle(CreateExampleCommand request, CancellationToken cancellationToken)
    {
        // TODO: construct the entity through its guarded constructor (never via property
        // assignment after construction — see .ai/coding-rules.md), add it, save, return.
        throw new NotImplementedException();
    }
}
