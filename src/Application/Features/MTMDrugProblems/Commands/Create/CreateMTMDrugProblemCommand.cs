using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTMDrugProblem;

namespace BigLion.CPA.Application.Features.MTMDrugProblems.Commands.Create;

public class CreateMTMDrugProblemCommand : IRequest<int>
{
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

public class CreateMTMDrugProblemCommandValidator : AbstractValidator<CreateMTMDrugProblemCommand>
{
    public CreateMTMDrugProblemCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateMTMDrugProblemCommandHandler : IRequestHandler<CreateMTMDrugProblemCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateMTMDrugProblemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMTMDrugProblemCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
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
        await _context.MTMDrugProblems.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
