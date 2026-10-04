using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.MTMDrugProblem;

namespace BigLion.CPA.Application.Features.MTMDrugProblems.Commands.Update;

public class UpdateMTMDrugProblemCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public int? MTMUID { get; set; }
    public string? ServiceTypeID { get; set; }
    public string? ProblemGroupUID { get; set; }
    public string? ProblemUID { get; set; }
    public string? ProblemOther { get; set; }
    public int? DrugUID { get; set; }
    public string? Interventions { get; set; }
    public string? FinalResult { get; set; }
    public string? FinalResultOther { get; set; }
    public string? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public string? MUser { get; set; }
    public DateTime? MWhen { get; set; }
    public int? PatientID { get; set; }
    public string? TMTID { get; set; }
    public int? DrugUID_Old { get; set; }
}

public class UpdateMTMDrugProblemCommandValidator : AbstractValidator<UpdateMTMDrugProblemCommand>
{
    public UpdateMTMDrugProblemCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateMTMDrugProblemCommandHandler : IRequestHandler<UpdateMTMDrugProblemCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateMTMDrugProblemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateMTMDrugProblemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMDrugProblems
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMDrugProblem", request.UID);

        entity.MTMUID = request.MTMUID;
        entity.ServiceTypeID = request.ServiceTypeID;
        entity.ProblemGroupUID = request.ProblemGroupUID;
        entity.ProblemUID = request.ProblemUID;
        entity.ProblemOther = request.ProblemOther;
        entity.DrugUID = request.DrugUID;
        entity.Interventions = request.Interventions;
        entity.FinalResult = request.FinalResult;
        entity.FinalResultOther = request.FinalResultOther;
        entity.CUser = request.CUser;
        entity.CWhen = request.CWhen;
        entity.MUser = request.MUser;
        entity.MWhen = request.MWhen;
        entity.PatientID = request.PatientID;
        entity.TMTID = request.TMTID;
        entity.DrugUID_Old = request.DrugUID_Old;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
