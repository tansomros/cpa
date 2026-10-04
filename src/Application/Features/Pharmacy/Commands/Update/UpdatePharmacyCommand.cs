using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Security;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Pharmacy.Commands.Update;

[RequirePermission(Permissions.Pharmacies.Update)]
public record UpdatePharmacyCommand : IRequest<Unit>
{
    public required int Id { get; init; }
    public required uint RowVersion { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? LicenseNo { get; init; }
    public string? NhsoCode { get; init; }
    public string? Name2 { get; init; }
    public int? PharmacyGroupId { get; init; }
    public int? PharmacyTypeId { get; init; }
    public string? PharmacyTypeOther { get; init; }
    public string? AddressNo { get; init; }
    public string? ProvinceId { get; init; }
    public string? DistrictId { get; init; }
    public string? SubDistrictId { get; init; }
    public string? ZipCode { get; init; }
    public string? Fda_Province { get; init; }
    public string? Office_Tel { get; init; }
    public string? Office_Fax { get; init; }
    public string? Office_Mail { get; init; }
    public string? LineID { get; init; }
    public string? Co_Name { get; init; }
    public string? Co_Mail { get; init; }
    public string? Co_Tel { get; init; }
    public string? RegisYear { get; init; }
    public string? Lat { get; init; }
    public string? Lng { get; init; }
    public bool IsActive { get; init; }
}

public class UpdatePharmacyCommandHandler : IRequestHandler<UpdatePharmacyCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdatePharmacyCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePharmacyCommand request, CancellationToken cancellationToken)
    {
        var pharmacy = await _context.Pharmacy
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Pharmacy), request.Id);

        _context.Entry(pharmacy).Property<uint>("xmin").OriginalValue = request.RowVersion;
        pharmacy.Update(
            request.Code.Trim(),
            request.Name.Trim(),
            request.LicenseNo,
            request.NhsoCode,
            request.Name2,
            request.PharmacyGroupId,
            request.PharmacyTypeId,
            request.PharmacyTypeOther,
            request.AddressNo,
            request.ProvinceId,
            request.DistrictId,
            request.SubDistrictId,
            request.ZipCode,
            request.Fda_Province,
            request.Office_Tel,
            request.Office_Fax,
            request.Office_Mail,
            request.LineID,
            request.Co_Name,
            request.Co_Mail,
            request.Co_Tel,
            request.RegisYear,
            request.Lat,
            request.Lng,
            request.IsActive);

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
