using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.DrugProblemGroup;

namespace BigLion.CPA.Application.Features.DrugProblemGroups.Commands.Update;

public class UpdateDrugProblemGroupCommand : IRequest<Unit>
{
    public string Code { get; set; } = string.Empty;
    public string? Descriptions { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }
}

public class UpdateDrugProblemGroupCommandValidator : AbstractValidator<UpdateDrugProblemGroupCommand>
{
    public UpdateDrugProblemGroupCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}

public class UpdateDrugProblemGroupCommandHandler : IRequestHandler<UpdateDrugProblemGroupCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateDrugProblemGroupCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDrugProblemGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DrugProblemGroups
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("DrugProblemGroup", request.Code);

        entity.Descriptions = request.Descriptions;
        entity.Sort = request.Sort;
        entity.StatusFlag = request.StatusFlag;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
