#pragma warning disable CS0618
using BigLion.Domain.ValueObjects;
using BigLion.Domain.Entities;

using static BigLion.Application.FunctionalTests.Testing;

namespace BigLion.Application.FunctionalTests.Features._Shared;

public static class TestDataFactory
{
    private static int _idCounter = 100;

    private static int NextId() => Interlocked.Increment(ref _idCounter);

    public static async Task<int> CreateTestCompanyAsync(string name = "บริษัท เทสต์ จำกัด")
    {
        var company = new Company(name, true);
        await AddAsync(company);
        return company.Id;
    }

    public static async Task<int> CreateTestPatientAsync(int? companyId = null)
    {
        var patient = new Patient(
            $"HN{NextId():D8}",
            "นาย",
            "ทดสอบ",
            "",
            "ระบบ",
            Gender.Male,
            DateOnly.FromDateTime(DateTime.Parse("1990-01-01")))
        {            
            Nationality = "ไทย",
        };
        await AddAsync(patient);
        return patient.Id;
    }

    public static async Task<int> CreateTestCheckupTypeAsync()
    {
        var id = NextId();
        var checkupType = new CheckupType(id, $"ตรวจสุขภาพประจำปี-{id}", "Annual Health Checkup");
        await AddAsync(checkupType);
        return checkupType.Id;
    }

    public static async Task<int> CreateTestCareproviderAsync(int typeId = 1)
    {
        var id = NextId();
        var careprovider = new CareProvider($"T{id:D3}", $"แพทย์ทดสอบ-{id}", typeId)
        {
            IsActive = true,
        };
        await AddAsync(careprovider);
        return careprovider.Id;
    }

    public static async Task<int> CreateTestCheckupClassAsync()
    {
        var id = NextId();
        var cls = new CheckupClass(id, $"CLS{id:D3}", $"หมวดทดสอบ-{id}", 1);
        await AddAsync(cls);
        return cls.Id;
    }

    public static async Task<int> CreateTestCheckupGroupAsync(int checkupClassId)
    {
        var id = NextId();
        var group = new CheckupGroup(id, $"GRP{id:D3}", $"กลุ่มทดสอบ-{id}", checkupClassId, 1);
        await AddAsync(group);
        return group.Id;
    }

    public static async Task<int> CreateTestCheckupItemAsync(int checkupGroupId, string? labItemCode = null)
    {
        var id = NextId();
        var item = new CheckupItem(id, $"ITEM{id:D3}", labItemCode, $"รายการทดสอบ-{id}", "Test Item", checkupGroupId, 1, null, null, 0, true);
        await AddAsync(item);
        return item.Id;
    }

    public static async Task<int> CreateTestCheckupAsync(int patientId, int checkupTypeId)
    {
        var id = NextId();
        var checkup = new Checkup(
            id,
            $"VN{id:D8}",
            $"HN{id:D8}",
            DateOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now),
            checkupTypeId,
            "สิทธิ์ทดสอบ",
            1,
            "โปรแกรมทดสอบ")
        {
            PatientId = patientId,
        };
        await AddAsync(checkup);
        return checkup.Id;
    }

    public static async Task<int> CreateTestReferenceGroupAsync(
        string? code = null,
        string descriptions = "กลุ่มอ้างอิงทดสอบ")
    {
        var id = NextId();
        code ??= $"RG{id:D3}";
        var group = new ReferenceGroup(code, descriptions, 1);
        await AddAsync(group);
        return group.Id;
    }

    public static async Task<int> CreateTestReferenceValueAsync(
        int referenceGroupId,
        string? valueCode = null,
        string descriptions = "ค่าอ้างอิงทดสอบ")
    {
        var id = NextId();
        valueCode ??= $"RV{id:D3}";
        var value = new ReferenceValue(valueCode, descriptions, referenceGroupId, 1);
        await AddAsync(value);
        return value.Id;
    }

    public static async Task<int> CreateTestAudiogramAsync(
        int checkupId,
        int checkupItemId,
        string visitNumber = "VN00000001")
    {
        var audiogram = new Audiogram(checkupId, visitNumber, checkupItemId, "ปกติ", "ปกติ", "ปกติ", "", "");
        await AddAsync(audiogram);
        return audiogram.Id;
    }

    public static async Task<int> CreateTestProvinceAsync(
        string? provinceId = null,
        string name = "นครราชสีมา")
    {
        provinceId ??= $"{NextId()}";
        var province = new Province("ภาคตะวันออกเฉียงเหนือ", provinceId, name, "Nakhon Ratchasima");
        await AddAsync(province);
        return province.Id;
    }

    public static async Task<int> CreateTestDistrictAsync(
        string provinceId = "30",
        string? districtId = null,
        string name = "เมืองนครราชสีมา")
    {
        districtId ??= $"{NextId()}";
        var district = new District(provinceId, districtId, name, "Muang");
        await AddAsync(district);
        return district.Id;
    }

    public static async Task<int> CreateTestSubDistrictAsync(
        string provinceId = "30",
        string districtId = "3001",
        string? subDistrictId = null,
        string name = "ในเมือง")
    {
        subDistrictId ??= $"{NextId()}";
        var subDistrict = new SubDistrict(provinceId, districtId, subDistrictId, name, "Nai Muang", "30000");
        await AddAsync(subDistrict);
        return subDistrict.Id;
    }

    public static async Task<int> CreateTestHearingHertzAsync(int hertz = 500)
    {
        var hearingHertz = new HearingHertz(hertz);
        await AddAsync(hearingHertz);
        return hearingHertz.Id;
    }

    public static async Task<int> CreateTestRecommendationAsync(
        string? code = null,
        string name = "คำแนะนำทดสอบ")
    {
        code ??= $"REC{NextId():D3}";
        var recommendation = new Recommendation(
            code, name, "LAB", "A", "B", 0, 100,
            "ผลปกติ", "Normal", "ไม่มีคำแนะนำ", "No recommendation",
            null, null);
        await AddAsync(recommendation);
        return recommendation.Id;
    }

    public static async Task<int> CreateTestLabAsync(int checkupId, string visitNumber, int? checkupItemId)
    {
        var lab = new Lab(checkupId, visitNumber, checkupItemId);
        await AddAsync(lab);
        return lab.Id;
    }

    public static async Task<int> CreateTestXrayAsync(
        int checkupId,
        string visitNumber,
        int checkupItemId)
    {
        var xray = new Xray(checkupId, visitNumber, checkupItemId, "ปกติ", "Normal chest", $"ACC{NextId():D6}");
        await AddAsync(xray);
        return xray.Id;
    }

    public static async Task<int> CreateTestVisionAsync(
        int checkupId,
        string visitNumber,
        int checkupItemId)
    {
        var vision = new Vision(checkupId, visitNumber, checkupItemId, "20/20", "20/20", "ปกติ");
        await AddAsync(vision);
        return vision.Id;
    }

    public static async Task<int> CreateTestHearingAsync(int audiogramId, int hertz = 500)
    {
        var hearing = new Hearing(audiogramId, hertz, 25.0, 25.0);
        await AddAsync(hearing);
        return hearing.Id;
    }

    public static async Task<int> CreateTestLungAsync(
        int checkupId,
        string visitNumber,
        int checkupItemId)
    {
        var lung = new Lung(checkupId, visitNumber, checkupItemId, "ปกติ");
        await AddAsync(lung);
        return lung.Id;
    }

    public static async Task<int> CreateTestPhysicalExaminationAsync(
        int checkupId,
        string visitNumber,
        int checkupItemId)
    {
        var pe = new PhysicalExamination(visitNumber, checkupId, checkupItemId);
        await AddAsync(pe);
        return pe.Id;
    }

    public static async Task<int> CreateTestSpecialTestAsync(
        int checkupId,
        string visitNumber,
        int checkupItemId)
    {
        var specialTest = new SpecialTest(checkupId, visitNumber, checkupItemId);
        await AddAsync(specialTest);
        return specialTest.Id;
    }

    public static async Task<int> CreateTestReportAsync(
        int checkupId,
        string visitNumber,
        string hospitalNumber,
        int checkupTypeId)
    {
        var report = new Report(
            checkupId, visitNumber, hospitalNumber,
            DateOnly.FromDateTime(DateTime.Now), "08:00",
            checkupTypeId, "สิทธิ์ทดสอบ", 1, "โปรแกรมทดสอบ");
        await AddAsync(report);
        return report.Id;
    }

    public static async Task<int> CreateTestRecommendationTemplateAsync(string text = "คำแนะนำเทมเพลตทดสอบ")
    {
        var template = new RecommendationTemplate(text);
        await AddAsync(template);
        return template.Id;
    }

    public record CheckupPrerequisites(
        int PatientId,
        int CheckupTypeId,
        int CheckupId,
        int CheckupClassId,
        int CheckupGroupId,
        int CheckupItemId,
        string VisitNumber);

    public static async Task<CheckupPrerequisites> SeedFullCheckupPrerequisitesAsync()
    {
        var checkupTypeId = await CreateTestCheckupTypeAsync();
        var patientId = await CreateTestPatientAsync();
        var classId = await CreateTestCheckupClassAsync();
        var groupId = await CreateTestCheckupGroupAsync(classId);
        var itemId = await CreateTestCheckupItemAsync(groupId);
        var checkupId = await CreateTestCheckupAsync(patientId, checkupTypeId);

        var checkup = await FindAsync<Checkup>(checkupId);
        var visitNumber = checkup!.VisitNumber;

        return new CheckupPrerequisites(
            patientId, checkupTypeId, checkupId,
            classId, groupId, itemId, visitNumber);
    }
}
