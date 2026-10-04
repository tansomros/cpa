using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.BehaviorProblemItem;

namespace BigLion.CPA.Application.Features.BehaviorProblemItems.Commands.Create;

public class CreateBehaviorProblemItemCommand : IRequest<int>
{
    public string? Descriptions { get; set; }
    public string? StatusFlag { get; set; }
    public int? Sort { get; set; }
}

public class CreateBehaviorProblemItemCommandValidator : AbstractValidator<CreateBehaviorProblemItemCommand>
{
    public CreateBehaviorProblemItemCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateBehaviorProblemItemCommandHandler : IRequestHandler<CreateBehaviorProblemItemCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateBehaviorProblemItemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateBehaviorProblemItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.Descriptions = request.Descriptions;
        entity.StatusFlag = request.StatusFlag;
        entity.Sort = request.Sort;
        await _context.BehaviorProblemItems.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
