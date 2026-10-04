using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Pharmacist;

namespace BigLion.CPA.Application.Features.Pharmacists.Commands.Create;

public class CreatePharmacistCommand : IRequest<int>
{
    public bool IsActive { get; set; } = true;
    public string? Name { get; set; }
    public string? LicenseNo { get; set; }
    public string? WorkTime { get; set; }
    public string? WorkType { get; set; }
    public string? PositionName { get; set; }
    public int? PharmacyId { get; set; }
}

public class CreatePharmacistCommandValidator : AbstractValidator<CreatePharmacistCommand>
{
    public CreatePharmacistCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreatePharmacistCommandHandler : IRequestHandler<CreatePharmacistCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreatePharmacistCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePharmacistCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.IsActive = request.IsActive;
        entity.Name = request.Name;
        entity.LicenseNo = request.LicenseNo;
        entity.WorkTime = request.WorkTime;
        entity.WorkType = request.WorkType;
        entity.PositionName = request.PositionName;
        entity.PharmacyId = request.PharmacyId;
        await _context.Pharmacists.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
