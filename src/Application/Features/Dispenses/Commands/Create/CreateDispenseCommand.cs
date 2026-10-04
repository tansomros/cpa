using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Dispense;

namespace BigLion.CPA.Application.Features.Dispenses.Commands.Create;

public class CreateDispenseCommand : IRequest<long>
{
    public string? RefID { get; set; }
    public DateOnly? RefillDate { get; set; }
    public int? PatientID { get; set; }
    public string? LocationID { get; set; }
    public string? Remark { get; set; }
    public int? MTMUID { get; set; }
    public int? DrugUID { get; set; }
    public string? TMTID { get; set; }
    public double? QTY { get; set; }
    public string? UOM { get; set; }
    public string? UsedRemark { get; set; }
    public string? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public string? MUser { get; set; }
    public DateTime? MWhen { get; set; }
}

public class CreateDispenseCommandValidator : AbstractValidator<CreateDispenseCommand>
{
    public CreateDispenseCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateDispenseCommandHandler : IRequestHandler<CreateDispenseCommand, long>
{
    private readonly ICpaDatabaseContext _context;

    public CreateDispenseCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateDispenseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.RefID = request.RefID;
        entity.RefillDate = request.RefillDate;
        entity.PatientID = request.PatientID;
        entity.LocationID = request.LocationID;
        entity.Remark = request.Remark;
        entity.MTMUID = request.MTMUID;
        entity.DrugUID = request.DrugUID;
        entity.TMTID = request.TMTID;
        entity.QTY = request.QTY;
        entity.UOM = request.UOM;
        entity.UsedRemark = request.UsedRemark;
        entity.CUser = request.CUser;
        entity.CWhen = request.CWhen;
        entity.MUser = request.MUser;
        entity.MWhen = request.MWhen;
        await _context.Dispenses.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
