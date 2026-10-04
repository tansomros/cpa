using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.MTMDesease;

namespace BigLion.CPA.Application.Features.MTMDeseases.Commands.Update;

public class UpdateMTMDeseaseCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public int? MTMUID { get; set; }
    public string? ServiceTypeID { get; set; }
    public int? DeseaseUID { get; set; }
    public string? DeseaseOther { get; set; }
    public int? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public int? PatientID { get; set; }
    public string? ICDCode { get; set; }
    public string? DeseaseName { get; set; }
}

public class UpdateMTMDeseaseCommandValidator : AbstractValidator<UpdateMTMDeseaseCommand>
{
    public UpdateMTMDeseaseCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateMTMDeseaseCommandHandler : IRequestHandler<UpdateMTMDeseaseCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateMTMDeseaseCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateMTMDeseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMDeseases
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMDesease", request.UID);

        entity.MTMUID = request.MTMUID;
        entity.ServiceTypeID = request.ServiceTypeID;
        entity.DeseaseUID = request.DeseaseUID;
        entity.DeseaseOther = request.DeseaseOther;
        entity.CUser = request.CUser;
        entity.CWhen = request.CWhen;
        entity.PatientID = request.PatientID;
        entity.ICDCode = request.ICDCode;
        entity.DeseaseName = request.DeseaseName;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
