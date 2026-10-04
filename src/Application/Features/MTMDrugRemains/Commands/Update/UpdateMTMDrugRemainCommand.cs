using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.MTMDrugRemain;

namespace BigLion.CPA.Application.Features.MTMDrugRemains.Commands.Update;

public class UpdateMTMDrugRemainCommand : IRequest<Unit>
{
    public int UID { get; set; }
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

public class UpdateMTMDrugRemainCommandValidator : AbstractValidator<UpdateMTMDrugRemainCommand>
{
    public UpdateMTMDrugRemainCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateMTMDrugRemainCommandHandler : IRequestHandler<UpdateMTMDrugRemainCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateMTMDrugRemainCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateMTMDrugRemainCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMDrugRemains
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMDrugRemain", request.UID);

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
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
