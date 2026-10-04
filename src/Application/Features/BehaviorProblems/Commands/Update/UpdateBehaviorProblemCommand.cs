using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.BehaviorProblem;

namespace BigLion.CPA.Application.Features.BehaviorProblems.Commands.Update;

public class UpdateBehaviorProblemCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public int? ServiceUID { get; set; }
    public int? ProblemUID { get; set; }
    public string? ProblemOther { get; set; }
    public string? Interventions { get; set; }
    public string? FinalResult { get; set; }
    public string? FinalResultOther { get; set; }
    public string? FatFollow { get; set; }
    public string? TasteFallow { get; set; }
    public string? ResultBegin { get; set; }
    public string? ResultEnd { get; set; }
    public string? Remark { get; set; }
    public string? isFollow { get; set; }
    public int? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public int? MUser { get; set; }
    public DateTime? MWhen { get; set; }
    public string? ServiceTypeID { get; set; }
    public int? PatientID { get; set; }
}

public class UpdateBehaviorProblemCommandValidator : AbstractValidator<UpdateBehaviorProblemCommand>
{
    public UpdateBehaviorProblemCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateBehaviorProblemCommandHandler : IRequestHandler<UpdateBehaviorProblemCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateBehaviorProblemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateBehaviorProblemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BehaviorProblems
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("BehaviorProblem", request.UID);

        entity.ServiceUID = request.ServiceUID;
        entity.ProblemUID = request.ProblemUID;
        entity.ProblemOther = request.ProblemOther;
        entity.Interventions = request.Interventions;
        entity.FinalResult = request.FinalResult;
        entity.FinalResultOther = request.FinalResultOther;
        entity.FatFollow = request.FatFollow;
        entity.TasteFallow = request.TasteFallow;
        entity.ResultBegin = request.ResultBegin;
        entity.ResultEnd = request.ResultEnd;
        entity.Remark = request.Remark;
        entity.isFollow = request.isFollow;
        entity.CUser = request.CUser;
        entity.CWhen = request.CWhen;
        entity.MUser = request.MUser;
        entity.MWhen = request.MWhen;
        entity.ServiceTypeID = request.ServiceTypeID;
        entity.PatientID = request.PatientID;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
