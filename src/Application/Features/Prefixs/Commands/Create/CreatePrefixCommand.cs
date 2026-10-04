using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Prefix;

namespace BigLion.CPA.Application.Features.Prefixs.Commands.Create;

public class CreatePrefixCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
}

public class CreatePrefixCommandValidator : AbstractValidator<CreatePrefixCommand>
{
    public CreatePrefixCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreatePrefixCommandHandler : IRequestHandler<CreatePrefixCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreatePrefixCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePrefixCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(request.Name);
        await _context.Prefixs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
