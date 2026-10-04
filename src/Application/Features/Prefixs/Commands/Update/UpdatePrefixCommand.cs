using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.Prefix;

namespace BigLion.CPA.Application.Features.Prefixs.Commands.Update;

public class UpdatePrefixCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class UpdatePrefixCommandValidator : AbstractValidator<UpdatePrefixCommand>
{
    public UpdatePrefixCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdatePrefixCommandHandler : IRequestHandler<UpdatePrefixCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdatePrefixCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePrefixCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Prefixs
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Prefix", request.Id);

        entity.Name = request.Name;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
