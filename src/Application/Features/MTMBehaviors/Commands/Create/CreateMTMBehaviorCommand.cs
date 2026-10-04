using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTMBehavior;

namespace BigLion.CPA.Application.Features.MTMBehaviors.Commands.Create;

public class CreateMTMBehaviorCommand : IRequest<int>
{
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

public class CreateMTMBehaviorCommandValidator : AbstractValidator<CreateMTMBehaviorCommand>
{
    public CreateMTMBehaviorCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateMTMBehaviorCommandHandler : IRequestHandler<CreateMTMBehaviorCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateMTMBehaviorCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMTMBehaviorCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
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
        await _context.MTMBehaviors.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
