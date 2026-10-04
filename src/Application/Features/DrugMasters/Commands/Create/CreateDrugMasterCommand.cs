using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.DrugMaster;

namespace BigLion.CPA.Application.Features.DrugMasters.Commands.Create;

public class CreateDrugMasterCommand : IRequest<int>
{
    public string? TMTID { get; set; }
    public string? Name { get; set; }
    public string? AliasName { get; set; }
    public string? Manufacturer { get; set; }
    public string? FSN { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }
    public int? OUID { get; set; }
}

public class CreateDrugMasterCommandValidator : AbstractValidator<CreateDrugMasterCommand>
{
    public CreateDrugMasterCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateDrugMasterCommandHandler : IRequestHandler<CreateDrugMasterCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateDrugMasterCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDrugMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.TMTID = request.TMTID;
        entity.Name = request.Name;
        entity.AliasName = request.AliasName;
        entity.Manufacturer = request.Manufacturer;
        entity.FSN = request.FSN;
        entity.Sort = request.Sort;
        entity.StatusFlag = request.StatusFlag;
        entity.OUID = request.OUID;
        await _context.DrugMasters.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
