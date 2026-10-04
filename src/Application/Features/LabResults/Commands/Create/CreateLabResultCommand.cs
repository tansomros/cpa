using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.LabResult;

namespace BigLion.CPA.Application.Features.LabResults.Commands.Create;

public class CreateLabResultCommand : IRequest<int>
{
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

public class CreateLabResultCommandValidator : AbstractValidator<CreateLabResultCommand>
{
    public CreateLabResultCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateLabResultCommandHandler : IRequestHandler<CreateLabResultCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateLabResultCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateLabResultCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
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
        await _context.LabResults.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
