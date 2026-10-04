using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.BehaviorProblemItem;

namespace BigLion.CPA.Application.Features.BehaviorProblemItems.Commands.Update;

public class UpdateBehaviorProblemItemCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public string? Descriptions { get; set; }
    public string? StatusFlag { get; set; }
    public int? Sort { get; set; }
}

public class UpdateBehaviorProblemItemCommandValidator : AbstractValidator<UpdateBehaviorProblemItemCommand>
{
    public UpdateBehaviorProblemItemCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateBehaviorProblemItemCommandHandler : IRequestHandler<UpdateBehaviorProblemItemCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateBehaviorProblemItemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateBehaviorProblemItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BehaviorProblemItems
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("BehaviorProblemItem", request.UID);

        entity.Descriptions = request.Descriptions;
        entity.StatusFlag = request.StatusFlag;
        entity.Sort = request.Sort;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
