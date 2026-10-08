using BigLion.CPA.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace BigLion.CPA.Domain.UnitTests.Entity;

/// <summary>
/// Cross-field clearing rules in Patient.UpdateSmokingHistory / UpdateAlcoholHistory:
/// - SmokeYear, SmokeCigarette, CigaretteType kept only for Regular or Quit smokers;
/// - SmokingQuit kept only for Regular smokers;
/// - DrinkFrequency (drinking days per week) kept only for Occasional or Regular drinkers.
/// </summary>
public class PatientLifestyleRulesTests
{
    private static Patient NewPatient() => new("สมชาย", "ใจดี", "1103700000000");

    [TestCase("Non")]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("XYZ")]
    public void UpdateSmokingHistory_NotASmoker_ShouldClearSmokingDetails(string? smoke)
    {
        var patient = NewPatient();

        patient.UpdateSmokingHistory(smoke, 10, 5, "Manufactured", true, "หมายเหตุ");

        patient.Smoke.Should().Be(smoke);
        patient.SmokeYear.Should().BeNull();
        patient.SmokeCigarette.Should().BeNull();
        patient.CigaretteType.Should().BeNull();
        patient.SmokingQuit.Should().BeNull();
        patient.SmokingRemark.Should().Be("หมายเหตุ");
    }

    [Test]
    public void UpdateSmokingHistory_Quit_ShouldKeepDetailsButClearSmokingQuit()
    {
        var patient = NewPatient();

        patient.UpdateSmokingHistory("Quit", 10, 5, "RollYourOwn", true, null);

        patient.Smoke.Should().Be("Quit");
        patient.SmokeYear.Should().Be(10);
        patient.SmokeCigarette.Should().Be(5);
        patient.CigaretteType.Should().Be("RollYourOwn");
        patient.SmokingQuit.Should().BeNull();
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateSmokingHistory_Regular_ShouldKeepAllDetails(bool smokingQuit)
    {
        var patient = NewPatient();

        patient.UpdateSmokingHistory("Regular", 10, 5, "Electronic", smokingQuit, "สูบทุกวัน");

        patient.Smoke.Should().Be("Regular");
        patient.SmokeYear.Should().Be(10);
        patient.SmokeCigarette.Should().Be(5);
        patient.CigaretteType.Should().Be("Electronic");
        patient.SmokingQuit.Should().Be(smokingQuit);
        patient.SmokingRemark.Should().Be("สูบทุกวัน");
    }

    [Test]
    public void UpdateSmokingHistory_RegularThenQuit_ShouldDropSmokingQuit()
    {
        var patient = NewPatient();
        patient.UpdateSmokingHistory("Regular", 10, 5, "Manufactured", true, null);

        patient.UpdateSmokingHistory("Quit", 10, 5, "Manufactured", true, null);

        patient.SmokingQuit.Should().BeNull();
        patient.SmokeYear.Should().Be(10);
        patient.SmokeCigarette.Should().Be(5);
        patient.CigaretteType.Should().Be("Manufactured");
    }

    [Test]
    public void UpdateSmokingHistory_RegularThenNon_ShouldClearStoredDetails()
    {
        var patient = NewPatient();
        patient.UpdateSmokingHistory("Regular", 10, 5, "Manufactured", true, null);

        patient.UpdateSmokingHistory("Non", 10, 5, "Manufactured", true, null);

        patient.SmokeYear.Should().BeNull();
        patient.SmokeCigarette.Should().BeNull();
        patient.CigaretteType.Should().BeNull();
        patient.SmokingQuit.Should().BeNull();
    }

    [TestCase("Non")]
    [TestCase("Quit")]
    [TestCase(null)]
    public void UpdateAlcoholHistory_NotDrinking_ShouldClearFrequency(string? alcohol)
    {
        var patient = NewPatient();

        patient.UpdateAlcoholHistory(alcohol, 3);

        patient.Drinking.Should().Be(alcohol);
        patient.DrinkFrequency.Should().BeNull();
    }

    [TestCase(null)]
    [TestCase(0)]
    [TestCase(3)]
    public void UpdateAlcoholHistory_Occasional_ShouldKeepFrequency(int? frequency)
    {
        var patient = NewPatient();

        patient.UpdateAlcoholHistory("Occasional", frequency);

        patient.Drinking.Should().Be("Occasional");
        patient.DrinkFrequency.Should().Be(frequency);
    }

    [TestCase(1)]
    [TestCase(7)]
    public void UpdateAlcoholHistory_Regular_ShouldKeepFrequency(int frequency)
    {
        var patient = NewPatient();

        patient.UpdateAlcoholHistory("Regular", frequency);

        patient.Drinking.Should().Be("Regular");
        patient.DrinkFrequency.Should().Be(frequency);
    }

    [Test]
    public void UpdateAlcoholHistory_RegularThenQuit_ShouldClearStoredFrequency()
    {
        var patient = NewPatient();
        patient.UpdateAlcoholHistory("Regular", 5);

        patient.UpdateAlcoholHistory("Quit", 5);

        patient.DrinkFrequency.Should().BeNull();
    }
}