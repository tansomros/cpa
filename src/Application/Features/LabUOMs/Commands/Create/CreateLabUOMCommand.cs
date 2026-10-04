using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.LabUOM;

namespace BigLion.CPA.Application.Features.LabUOMs.Commands.Create;

public class CreateLabUOMCommand : IRequest<int>
{
    public string? Descriptions { get; set; }
}

public class CreateLabUOMCommandValidator : AbstractValidator<CreateLabUOMCommand>
{
    public CreateLabUOMCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateLabUOMCommandHandler : IRequestHandler<CreateLabUOMCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateLabUOMCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateLabUOMCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.Descriptions = request.Descriptions;
        await _context.LabUOMs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
