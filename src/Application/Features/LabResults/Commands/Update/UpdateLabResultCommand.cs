using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.LabResult;

namespace BigLion.CPA.Application.Features.LabResults.Commands.Update;

public class UpdateLabResultCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public int? RefUID { get; set; }
    public int? ResultDate { get; set; }
    public int? PatientID { get; set; }
    public int? LabUID { get; set; }
    public string? ResultValue { get; set; }
    public string? IsNormal { get; set; }
    public string? StatusFlag { get; set; }
    public int? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public int? MUser { get; set; }
    public DateTime? MWhen { get; set; }
}

public class UpdateLabResultCommandValidator : AbstractValidator<UpdateLabResultCommand>
{
    public UpdateLabResultCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateLabResultCommandHandler : IRequestHandler<UpdateLabResultCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateLabResultCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLabResultCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.LabResults
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("LabResult", request.UID);

        entity.RefUID = request.RefUID;
        entity.ResultDate = request.ResultDate;
        entity.PatientID = request.PatientID;
        entity.LabUID = request.LabUID;
        entity.ResultValue = request.ResultValue;
        entity.IsNormal = request.IsNormal;
        entity.StatusFlag = request.StatusFlag;
        entity.CUser = request.CUser;
        entity.CWhen = request.CWhen;
        entity.MUser = request.MUser;
        entity.MWhen = request.MWhen;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
