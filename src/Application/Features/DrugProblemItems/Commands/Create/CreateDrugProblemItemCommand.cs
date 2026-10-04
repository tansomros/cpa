using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.DrugProblemItem;

namespace BigLion.CPA.Application.Features.DrugProblemItems.Commands.Create;

public class CreateDrugProblemItemCommand : IRequest<string>
{
    public string Code { get; set; } = string.Empty;
    public string? Descriptions { get; set; }
    public string? DrugProblemGroupUID { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }
}

public class CreateDrugProblemItemCommandValidator : AbstractValidator<CreateDrugProblemItemCommand>
{
    public CreateDrugProblemItemCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}

public class CreateDrugProblemItemCommandHandler : IRequestHandler<CreateDrugProblemItemCommand, string>
{
    private readonly ICpaDatabaseContext _context;

    public CreateDrugProblemItemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateDrugProblemItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.Code = request.Code;
        entity.Descriptions = request.Descriptions;
        entity.DrugProblemGroupUID = request.DrugProblemGroupUID;
        entity.Sort = request.Sort;
        entity.StatusFlag = request.StatusFlag;
        await _context.DrugProblemItems.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Code;
    }
}
