using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Security;
using PharmacyEntity = BigLion.CPA.Domain.Entities.Pharmacy;

namespace BigLion.CPA.Application.Features.Pharmacy.Commands.Create;

[RequirePermission(Permissions.Pharmacies.Create)]
public record CreatePharmacyCommand : IRequest<int>
{
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
}

public class CreatePharmacyCommandHandler : IRequestHandler<CreatePharmacyCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreatePharmacyCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePharmacyCommand request, CancellationToken cancellationToken)
    {
        var pharmacy = new PharmacyEntity(request.Code.Trim(), request.Name.Trim());
        pharmacy.AssignDetails(
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
            request.Lng);

        await _context.Pharmacy.AddAsync(pharmacy, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return pharmacy.Id;
    }
}
