using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.MTMRefer;

namespace BigLion.CPA.Application.Features.MTMRefers.Commands.Update;

public class UpdateMTMReferCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public int? MTMUID { get; set; }
    public string? HospitalType { get; set; }
    public string? HospitalName { get; set; }
    public int? PatientID { get; set; }
}

public class UpdateMTMReferCommandValidator : AbstractValidator<UpdateMTMReferCommand>
{
    public UpdateMTMReferCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateMTMReferCommandHandler : IRequestHandler<UpdateMTMReferCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateMTMReferCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateMTMReferCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMRefers
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMRefer", request.UID);

        entity.MTMUID = request.MTMUID;
        entity.HospitalType = request.HospitalType;
        entity.HospitalName = request.HospitalName;
        entity.PatientID = request.PatientID;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
