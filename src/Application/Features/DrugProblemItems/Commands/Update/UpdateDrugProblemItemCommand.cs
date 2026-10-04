using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.DrugProblemItem;

namespace BigLion.CPA.Application.Features.DrugProblemItems.Commands.Update;

public class UpdateDrugProblemItemCommand : IRequest<Unit>
{
    public string Code { get; set; } = string.Empty;
    public string? Descriptions { get; set; }
    public string? DrugProblemGroupUID { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }
}

public class UpdateDrugProblemItemCommandValidator : AbstractValidator<UpdateDrugProblemItemCommand>
{
    public UpdateDrugProblemItemCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}

public class UpdateDrugProblemItemCommandHandler : IRequestHandler<UpdateDrugProblemItemCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateDrugProblemItemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDrugProblemItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DrugProblemItems
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("DrugProblemItem", request.Code);

        entity.Descriptions = request.Descriptions;
        entity.DrugProblemGroupUID = request.DrugProblemGroupUID;
        entity.Sort = request.Sort;
        entity.StatusFlag = request.StatusFlag;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
