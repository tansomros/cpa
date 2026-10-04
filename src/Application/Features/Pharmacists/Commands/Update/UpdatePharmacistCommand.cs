using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.Pharmacist;

namespace BigLion.CPA.Application.Features.Pharmacists.Commands.Update;

public class UpdatePharmacistCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string? Name { get; set; }
    public string? LicenseNo { get; set; }
    public string? WorkTime { get; set; }
    public string? WorkType { get; set; }
    public string? PositionName { get; set; }
    public int? PharmacyId { get; set; }
}

public class UpdatePharmacistCommandValidator : AbstractValidator<UpdatePharmacistCommand>
{
    public UpdatePharmacistCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdatePharmacistCommandHandler : IRequestHandler<UpdatePharmacistCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdatePharmacistCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePharmacistCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Pharmacists
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeleteFlag != true, cancellationToken)
            ?? throw new NotFoundException("Pharmacist", request.Id);

        entity.IsActive = request.IsActive;
        entity.Name = request.Name;
        entity.LicenseNo = request.LicenseNo;
        entity.WorkTime = request.WorkTime;
        entity.WorkType = request.WorkType;
        entity.PositionName = request.PositionName;
        entity.PharmacyId = request.PharmacyId;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
