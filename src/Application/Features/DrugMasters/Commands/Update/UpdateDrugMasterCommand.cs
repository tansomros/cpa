using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.DrugMaster;

namespace BigLion.CPA.Application.Features.DrugMasters.Commands.Update;

public class UpdateDrugMasterCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public string? TMTID { get; set; }
    public string? Name { get; set; }
    public string? AliasName { get; set; }
    public string? Manufacturer { get; set; }
    public string? FSN { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }
    public int? OUID { get; set; }
}

public class UpdateDrugMasterCommandValidator : AbstractValidator<UpdateDrugMasterCommand>
{
    public UpdateDrugMasterCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateDrugMasterCommandHandler : IRequestHandler<UpdateDrugMasterCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateDrugMasterCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDrugMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DrugMasters
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("DrugMaster", request.UID);

        entity.TMTID = request.TMTID;
        entity.Name = request.Name;
        entity.AliasName = request.AliasName;
        entity.Manufacturer = request.Manufacturer;
        entity.FSN = request.FSN;
        entity.Sort = request.Sort;
        entity.StatusFlag = request.StatusFlag;
        entity.OUID = request.OUID;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
