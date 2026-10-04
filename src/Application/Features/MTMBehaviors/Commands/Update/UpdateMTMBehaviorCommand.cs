using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.MTMBehavior;

namespace BigLion.CPA.Application.Features.MTMBehaviors.Commands.Update;

public class UpdateMTMBehaviorCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public int? MTMUID { get; set; }
    public int? ProblemUID { get; set; }
    public string? ProblemOther { get; set; }
    public string? Interventions { get; set; }
    public int? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public int? MUser { get; set; }
    public DateTime? MWhen { get; set; }
    public string? FinalResult { get; set; }
    public string? FinalResultOther { get; set; }
    public string? FatFollow { get; set; }
    public string? TasteFollow { get; set; }
    public string? ResultBegin { get; set; }
    public string? ResultEnd { get; set; }
    public string? Remark { get; set; }
    public string? isFollow { get; set; }
    public int? PatientID { get; set; }
}

public class UpdateMTMBehaviorCommandValidator : AbstractValidator<UpdateMTMBehaviorCommand>
{
    public UpdateMTMBehaviorCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateMTMBehaviorCommandHandler : IRequestHandler<UpdateMTMBehaviorCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateMTMBehaviorCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateMTMBehaviorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMBehaviors
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMBehavior", request.UID);

        entity.MTMUID = request.MTMUID;
        entity.ProblemUID = request.ProblemUID;
        entity.ProblemOther = request.ProblemOther;
        entity.Interventions = request.Interventions;
        entity.CUser = request.CUser;
        entity.CWhen = request.CWhen;
        entity.MUser = request.MUser;
        entity.MWhen = request.MWhen;
        entity.FinalResult = request.FinalResult;
        entity.FinalResultOther = request.FinalResultOther;
        entity.FatFollow = request.FatFollow;
        entity.TasteFollow = request.TasteFollow;
        entity.ResultBegin = request.ResultBegin;
        entity.ResultEnd = request.ResultEnd;
        entity.Remark = request.Remark;
        entity.isFollow = request.isFollow;
        entity.PatientID = request.PatientID;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
