using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Security;
using BigLion.CPA.Application.Features.Pharmacy.ViewModels;

namespace BigLion.CPA.Application.Features.Pharmacy.Queries.Get;

[RequirePermission(Permissions.Pharmacies.View)]
public record GetPharmaciesQuery : IRequest<PaginatedList<PharmacyViewModel>>
{
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class GetPharmaciesQueryHandler : IRequestHandler<GetPharmaciesQuery, PaginatedList<PharmacyViewModel>>
{
    private readonly ICpaDatabaseContext _context;

    public GetPharmaciesQueryHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<PharmacyViewModel>> Handle(GetPharmaciesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Pharmacy
            .AsNoTracking()
            .Where(p => p.DeleteFlag == null || p.DeleteFlag == false);

        if (request.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(p =>
                p.Code.ToLower().Contains(term) ||
                p.Name.ToLower().Contains(term) ||
                (p.Name2 != null && p.Name2.ToLower().Contains(term)) ||
                (p.LicenseNo != null && p.LicenseNo.ToLower().Contains(term)) ||
                (p.NhsoCode != null && p.NhsoCode.ToLower().Contains(term)));
        }

        var ordered = query
            .OrderBy(p => p.Code)
            .ThenBy(p => p.Name);

        var count = await ordered.CountAsync(cancellationToken);
        var items = await ordered
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(p => new PharmacyViewModel
            {
                Id = p.Id,
                Code = p.Code,
                LicenseNo = p.LicenseNo,
                NhsoCode = p.NhsoCode,
                Name = p.Name,
                Name2 = p.Name2,
                PharmacyGroupId = p.PharmacyGroupId,
                PharmacyGroupName = p.PharmacyGroup != null ? p.PharmacyGroup.Name : null,
                PharmacyTypeId = p.PharmacyTypeId,
                PharmacyTypeName = p.PharmacyType != null ? p.PharmacyType.Name : null,
                PharmacyTypeOther = p.PharmacyTypeOther,
                AddressNo = p.AddressNo,
                ProvinceId = p.ProvinceId,
                ProvinceName = p.Province != null ? p.Province.Name : null,
                DistrictId = p.DistrictId,
                DistrictName = p.District != null ? p.District.Name : null,
                SubDistrictId = p.SubDistrictId,
                SubDistrictName = p.SubDistrict != null ? p.SubDistrict.Name : null,
                ZipCode = p.ZipCode,
                Fda_Province = p.Fda_Province,
                Office_Tel = p.Office_Tel,
                Office_Fax = p.Office_Fax,
                Office_Mail = p.Office_Mail,
                LineID = p.LineID,
                Co_Name = p.Co_Name,
                Co_Mail = p.Co_Mail,
                Co_Tel = p.Co_Tel,
                RegisYear = p.RegisYear,
                Lat = p.Lat,
                Lng = p.Lng,
                IsActive = p.IsActive,
                DeleteFlag = p.DeleteFlag,
                CreatedOn = p.CreatedOn,
                LastModified = p.LastModified,
                RowVersion = EF.Property<uint>(p, "xmin")
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<PharmacyViewModel>(items, count, request.Page, request.Limit);
    }
}
