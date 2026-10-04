using BigLion.CPA.Domain.Entities;
using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features.Pharmacies;

internal static class PharmacyTestData
{
    public const string ProvinceId = "10";
    public const string DistrictId = "1001";
    public const string SubDistrictId = "100101";
    public const string OtherProvinceId = "50";
    public const string OtherDistrictId = "5001";

    public static async Task SeedAddressesAsync()
    {
        await AddAsync(new Province("กลาง", ProvinceId, "กรุงเทพมหานคร", "Bangkok"));
        await AddAsync(new Province("เหนือ", OtherProvinceId, "เชียงใหม่", "Chiang Mai"));
        await AddAsync(new District(ProvinceId, DistrictId, "พระนคร", "Phra Nakhon"));
        await AddAsync(new District(OtherProvinceId, OtherDistrictId, "เมืองเชียงใหม่", "Mueang Chiang Mai"));
        await AddAsync(new SubDistrict(ProvinceId, DistrictId, SubDistrictId, "พระบรมมหาราชวัง", "Phra Borom Maha Ratchawang", "10200"));
    }

    public static async Task<int> CreatePharmacyAsync(string code = "PHA-001", string name = "ร้านยาชุมชน")
    {
        var pharmacy = new Pharmacy(code, name);
        await AddAsync(pharmacy);

        return pharmacy.Id;
    }
}
