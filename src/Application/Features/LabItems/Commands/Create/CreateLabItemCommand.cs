using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.LabItem;

namespace BigLion.CPA.Application.Features.LabItems.Commands.Create;

public class CreateLabItemCommand : IRequest<int>
{
    public string? Name { get; set; }
    public string? AliasName { get; set; }
    public int? UOMUID { get; set; }
    public string? NormalRange { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }
}

public class CreateLabItemCommandValidator : AbstractValidator<CreateLabItemCommand>
{
    public CreateLabItemCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateLabItemCommandHandler : IRequestHandler<CreateLabItemCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateLabItemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateLabItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.Name = request.Name;
        entity.AliasName = request.AliasName;
        entity.UOMUID = request.UOMUID;
        entity.NormalRange = request.NormalRange;
        entity.Sort = request.Sort;
        entity.StatusFlag = request.StatusFlag;
        await _context.LabItems.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
