using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.DrugProblemGroup;

namespace BigLion.CPA.Application.Features.DrugProblemGroups.Commands.Create;

public class CreateDrugProblemGroupCommand : IRequest<string>
{
    public string Code { get; set; } = string.Empty;
    public string? Descriptions { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }
}

public class CreateDrugProblemGroupCommandValidator : AbstractValidator<CreateDrugProblemGroupCommand>
{
    public CreateDrugProblemGroupCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}

public class CreateDrugProblemGroupCommandHandler : IRequestHandler<CreateDrugProblemGroupCommand, string>
{
    private readonly ICpaDatabaseContext _context;

    public CreateDrugProblemGroupCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateDrugProblemGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.Code = request.Code;
        entity.Descriptions = request.Descriptions;
        entity.Sort = request.Sort;
        entity.StatusFlag = request.StatusFlag;
        await _context.DrugProblemGroups.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Code;
    }
}
