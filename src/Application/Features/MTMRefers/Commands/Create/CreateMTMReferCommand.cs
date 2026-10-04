using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTMRefer;

namespace BigLion.CPA.Application.Features.MTMRefers.Commands.Create;

public class CreateMTMReferCommand : IRequest<int>
{
    public int? MTMUID { get; set; }
    public string? HospitalType { get; set; }
    public string? HospitalName { get; set; }
    public int? PatientID { get; set; }
}

public class CreateMTMReferCommandValidator : AbstractValidator<CreateMTMReferCommand>
{
    public CreateMTMReferCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateMTMReferCommandHandler : IRequestHandler<CreateMTMReferCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateMTMReferCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMTMReferCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.MTMUID = request.MTMUID;
        entity.HospitalType = request.HospitalType;
        entity.HospitalName = request.HospitalName;
        entity.PatientID = request.PatientID;
        await _context.MTMRefers.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
