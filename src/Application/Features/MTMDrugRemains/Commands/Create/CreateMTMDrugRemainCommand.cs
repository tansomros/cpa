using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTMDrugRemain;

namespace BigLion.CPA.Application.Features.MTMDrugRemains.Commands.Create;

public class CreateMTMDrugRemainCommand : IRequest<int>
{
    public int? MTMUID { get; set; }
    public string? ServiceTypeID { get; set; }
    public int? DrugUID { get; set; }
    public double? QTY { get; set; }
    public string? UOM { get; set; }
    public string? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public string? MUser { get; set; }
    public DateTime? MWhen { get; set; }
    public int? PatientID { get; set; }
    public string? RemainFrom { get; set; }
    public int? ReasonUID { get; set; }
    public string? ReasonRemark { get; set; }
    public string? TMTID { get; set; }
}

public class CreateMTMDrugRemainCommandValidator : AbstractValidator<CreateMTMDrugRemainCommand>
{
    public CreateMTMDrugRemainCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateMTMDrugRemainCommandHandler : IRequestHandler<CreateMTMDrugRemainCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateMTMDrugRemainCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMTMDrugRemainCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.MTMUID = request.MTMUID;
        entity.ServiceTypeID = request.ServiceTypeID;
        entity.DrugUID = request.DrugUID;
        entity.QTY = request.QTY;
        entity.UOM = request.UOM;
        entity.CUser = request.CUser;
        entity.CWhen = request.CWhen;
        entity.MUser = request.MUser;
        entity.MWhen = request.MWhen;
        entity.PatientID = request.PatientID;
        entity.RemainFrom = request.RemainFrom;
        entity.ReasonUID = request.ReasonUID;
        entity.ReasonRemark = request.ReasonRemark;
        entity.TMTID = request.TMTID;
        await _context.MTMDrugRemains.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
