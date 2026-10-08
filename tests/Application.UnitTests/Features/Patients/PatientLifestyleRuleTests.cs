using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Features.Patients.Commands.Create;
using BigLion.CPA.Application.Features.Patients.Commands.Update;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;

namespace BigLion.CPA.Application.UnitTests.Features.Patients;

public enum PatientCommandKind
{
    Create,
    Update,
}

/// <summary>
/// Lifestyle rules in PatientWriteRules, run through both CreatePatientCommandValidator
/// and UpdatePatientCommandValidator:
/// - Smoke / CigaretteType / Alcohol must be an exact (case-sensitive) SmartEnum code or null;
/// - AlcoholFQ is drinking days per week: 0..7 for Occasional/Regular, at least 1 for Regular;
/// - stale values the Domain will clear (e.g. frequency for a Non drinker) are not errors.
/// </summary>
[TestFixture(PatientCommandKind.Create)]
[TestFixture(PatientCommandKind.Update)]
public class PatientLifestyleRuleTests(PatientCommandKind kind)
{
    private sealed record Lifestyle(
        string? Smoke = null,
        int? SmokeYear = null,
        int? SmokeCigarette = null,
        string? CigaretteType = null,
        bool? SmokingQuit = null,
        string? Alcohol = null,
        int? AlcoholFQ = null);

    [TestCase(null)]
    [TestCase(0)]
    public async Task RegularDrinker_WithoutAtLeastOneDayPerWeek_ShouldFail(int? frequency)
    {
        var errors = await Validate(new Lifestyle(Alcohol: "Regular", AlcoholFQ: frequency));

        errors.Should().ContainSingle()
            .Which.PropertyName.Should().EndWith("AlcoholFQ");
    }

    [TestCase("Regular", -1)]
    [TestCase("Regular", 8)]
    [TestCase("Occasional", -1)]
    [TestCase("Occasional", 8)]
    public async Task Drinker_FrequencyOutsideZeroToSeven_ShouldFail(string alcohol, int frequency)
    {
        var errors = await Validate(new Lifestyle(Alcohol: alcohol, AlcoholFQ: frequency));

        errors.Should().NotBeEmpty();
        errors.Should().OnlyContain(e => e.PropertyName.EndsWith("AlcoholFQ", StringComparison.Ordinal));
    }

    [TestCase("Occasional", null)]
    [TestCase("Occasional", 0)]
    [TestCase("Occasional", 7)]
    [TestCase("Regular", 1)]
    [TestCase("Regular", 7)]
    public async Task Drinker_FrequencyInRange_ShouldPass(string alcohol, int? frequency)
    {
        var errors = await Validate(new Lifestyle(Alcohol: alcohol, AlcoholFQ: frequency));

        errors.Should().BeEmpty();
    }

    [TestCase("Non", 8)]
    [TestCase("Non", -1)]
    [TestCase("Non", 0)]
    [TestCase("Quit", 8)]
    [TestCase(null, 8)]
    public async Task NotDrinking_StaleFrequency_ShouldPass(string? alcohol, int? frequency)
    {
        var errors = await Validate(new Lifestyle(Alcohol: alcohol, AlcoholFQ: frequency));

        errors.Should().BeEmpty();
    }

    [Test]
    public async Task NonSmoker_StaleSmokingDetails_ShouldPass()
    {
        var errors = await Validate(new Lifestyle(
            Smoke: "Non", SmokeYear: 10, SmokeCigarette: 5, CigaretteType: "Manufactured", SmokingQuit: true));

        errors.Should().BeEmpty();
    }

    [TestCase("Smoke", "regular")]
    [TestCase("Smoke", "NON")]
    [TestCase("Smoke", "XYZ")]
    [TestCase("Smoke", "")]
    [TestCase("CigaretteType", "manufactured")]
    [TestCase("CigaretteType", "ELECTRONIC")]
    [TestCase("CigaretteType", "XYZ")]
    [TestCase("CigaretteType", "")]
    [TestCase("Alcohol", "regular")]
    [TestCase("Alcohol", "OCCASIONAL")]
    [TestCase("Alcohol", "XYZ")]
    [TestCase("Alcohol", "")]
    public async Task LookupCode_UnknownOrWrongCase_ShouldFail(string field, string code)
    {
        var errors = await Validate(WithCode(field, code));

        errors.Should().ContainSingle()
            .Which.PropertyName.Should().EndWith(field);
    }

    [TestCase("Smoke", "Non")]
    [TestCase("Smoke", "Regular")]
    [TestCase("Smoke", "Quit")]
    [TestCase("Smoke", null)]
    [TestCase("CigaretteType", "Manufactured")]
    [TestCase("CigaretteType", "RollYourOwn")]
    [TestCase("CigaretteType", "Electronic")]
    [TestCase("CigaretteType", "Other")]
    [TestCase("CigaretteType", null)]
    [TestCase("Alcohol", "Non")]
    [TestCase("Alcohol", "Quit")]
    [TestCase("Alcohol", "Occasional")]
    [TestCase("Alcohol", "Regular")]
    [TestCase("Alcohol", null)]
    public async Task LookupCode_ExactCodeOrNull_ShouldPass(string field, string? code)
    {
        var errors = await Validate(WithCode(field, code));

        errors.Should().BeEmpty();
    }

    // AlcoholFQ = 3 keeps the Regular-drinker frequency rule satisfied so only the code rule is tested.
    private static Lifestyle WithCode(string field, string? code) => field switch
    {
        "Smoke" => new Lifestyle(Smoke: code, AlcoholFQ: 3),
        "CigaretteType" => new Lifestyle(CigaretteType: code, AlcoholFQ: 3),
        "Alcohol" => new Lifestyle(Alcohol: code, AlcoholFQ: 3),
        _ => throw new ArgumentOutOfRangeException(nameof(field), field, null),
    };

    private async Task<List<ValidationFailure>> Validate(Lifestyle lifestyle)
    {
        var context = Mock.Of<ICpaDatabaseContext>();
        ValidationResult result;

        if (kind == PatientCommandKind.Create)
        {
            var command = new CreatePatientCommand
            {
                ForeName = "Test",
                Surname = "Patient",
                Smoke = lifestyle.Smoke,
                SmokeYear = lifestyle.SmokeYear,
                SmokeCigarette = lifestyle.SmokeCigarette,
                CigaretteType = lifestyle.CigaretteType,
                SmokingQuit = lifestyle.SmokingQuit,
                Alcohol = lifestyle.Alcohol,
                AlcoholFQ = lifestyle.AlcoholFQ,
            };
            result = await new CreatePatientCommandValidator(context, TimeProvider.System).ValidateAsync(command);
        }
        else
        {
            var command = new UpdatePatientCommand
            {
                Id = 1,
                ForeName = "Test",
                Surname = "Patient",
                Smoke = lifestyle.Smoke,
                SmokeYear = lifestyle.SmokeYear,
                SmokeCigarette = lifestyle.SmokeCigarette,
                CigaretteType = lifestyle.CigaretteType,
                SmokingQuit = lifestyle.SmokingQuit,
                Alcohol = lifestyle.Alcohol,
                AlcoholFQ = lifestyle.AlcoholFQ,
            };
            result = await new UpdatePatientCommandValidator(context, TimeProvider.System).ValidateAsync(command);
        }

        return result.Errors;
    }
}