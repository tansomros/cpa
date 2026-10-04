using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Security;

namespace BigLion.CPA.Application.Features.Pharmacy.Queries.Get;

[RequirePermission(Permissions.Pharmacies.View)]
public record GetPharmacyLookupsQuery : IRequest<PharmacyLookupsViewModel>
{
    public string? ProvinceId { get; init; }
    public string? DistrictId { get; init; }
}

public record PharmacyLookupOption(string Value, string Title);

public class PharmacyLookupsViewModel
{
    public List<PharmacyLookupOption> Groups { get; init; } = [];
    public List<PharmacyLookupOption> Types { get; init; } = [];
    public List<PharmacyLookupOption> Provinces { get; init; } = [];
    public List<PharmacyLookupOption> Districts { get; init; } = [];
    public List<PharmacyLookupOption> SubDistricts { get; init; } = [];
}

public class GetPharmacyLookupsQueryHandler : IRequestHandler<GetPharmacyLookupsQuery, PharmacyLookupsViewModel>
{
    private readonly ICpaDatabaseContext _context;

    public GetPharmacyLookupsQueryHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<PharmacyLookupsViewModel> Handle(GetPharmacyLookupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await _context.PharmacyGroups.AsNoTracking()
            .OrderBy(g => g.Sort)
            .Select(g => new PharmacyLookupOption(g.Id.ToString(), g.Code + " - " + g.Name))
            .ToListAsync(cancellationToken);

        var types = await _context.PharmacyTypes.AsNoTracking()
            .OrderBy(t => t.Sort)
            .Select(t => new PharmacyLookupOption(t.Id.ToString(), t.Name))
            .ToListAsync(cancellationToken);

        var provinces = await _context.Provinces.AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new PharmacyLookupOption(p.Id, p.Name))
            .ToListAsync(cancellationToken);

        var districts = string.IsNullOrWhiteSpace(request.ProvinceId)
            ? []
            : await _context.Districts.AsNoTracking()
                .Where(d => d.ProvinceId == request.ProvinceId)
                .OrderBy(d => d.Name)
                .Select(d => new PharmacyLookupOption(d.Id, d.Name))
                .ToListAsync(cancellationToken);

        var subDistricts = string.IsNullOrWhiteSpace(request.DistrictId)
            ? []
            : await _context.SubDistricts.AsNoTracking()
                .Where(s => s.DistrictId == request.DistrictId)
                .OrderBy(s => s.Name)
                .Select(s => new PharmacyLookupOption(s.SubDistrictId, s.Name + " (" + s.ZipCode + ")"))
                .ToListAsync(cancellationToken);

        return new PharmacyLookupsViewModel
        {
            Groups = groups,
            Types = types,
            Provinces = provinces,
            Districts = districts,
            SubDistricts = subDistricts,
        };
    }
}
