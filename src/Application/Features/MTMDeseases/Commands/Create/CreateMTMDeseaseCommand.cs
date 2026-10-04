using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTMDesease;

namespace BigLion.CPA.Application.Features.MTMDeseases.Commands.Create;

public class CreateMTMDeseaseCommand : IRequest<int>
{
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

public class CreateMTMDeseaseCommandValidator : AbstractValidator<CreateMTMDeseaseCommand>
{
    public CreateMTMDeseaseCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateMTMDeseaseCommandHandler : IRequestHandler<CreateMTMDeseaseCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateMTMDeseaseCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMTMDeseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.MTMUID = request.MTMUID;
        entity.ServiceTypeID = request.ServiceTypeID;
        entity.DeseaseUID = request.DeseaseUID;
        entity.DeseaseOther = request.DeseaseOther;
        entity.CUser = request.CUser;
        entity.CWhen = request.CWhen;
        entity.PatientID = request.PatientID;
        entity.ICDCode = request.ICDCode;
        entity.DeseaseName = request.DeseaseName;
        await _context.MTMDeseases.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
