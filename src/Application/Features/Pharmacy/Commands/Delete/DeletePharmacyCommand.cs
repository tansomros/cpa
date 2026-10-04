using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Security;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Pharmacy.Commands.Delete;

[RequirePermission(Permissions.Pharmacies.Update)]
public record DeletePharmacyCommand : IRequest<Unit>
{
    public required int Id { get; init; }
    public required uint RowVersion { get; init; }
}

public class DeletePharmacyCommandHandler : IRequestHandler<DeletePharmacyCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeletePharmacyCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePharmacyCommand request, CancellationToken cancellationToken)
    {
        var pharmacy = await _context.Pharmacy
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Pharmacy), request.Id);

        _context.Entry(pharmacy).Property<uint>("xmin").OriginalValue = request.RowVersion;
        pharmacy.Deactivate();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new Common.Exceptions.ConflictException("ข้อมูลร้านขายยาถูกแก้ไขโดยผู้ใช้อื่น กรุณาโหลดข้อมูลใหม่แล้วลองอีกครั้ง");
        }

        return Unit.Value;
    }
}
