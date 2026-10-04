using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.LabItem;

namespace BigLion.CPA.Application.Features.LabItems.Commands.Update;

public class UpdateLabItemCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public string? Name { get; set; }
    public string? AliasName { get; set; }
    public int? UOMUID { get; set; }
    public string? NormalRange { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }
}

public class UpdateLabItemCommandValidator : AbstractValidator<UpdateLabItemCommand>
{
    public UpdateLabItemCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateLabItemCommandHandler : IRequestHandler<UpdateLabItemCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateLabItemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLabItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.LabItems
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("LabItem", request.UID);

        entity.Name = request.Name;
        entity.AliasName = request.AliasName;
        entity.UOMUID = request.UOMUID;
        entity.NormalRange = request.NormalRange;
        entity.Sort = request.Sort;
        entity.StatusFlag = request.StatusFlag;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
