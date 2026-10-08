using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Features.Patients.Commands.Create;
using BigLion.CPA.Application.Features.Patients.Commands.Update;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;

namespace BigLion.CPA.Application.UnitTests.Features.Patients;

/// <summary>
/// BirthDate rule (PatientWriteRules): optional; when given it must be
/// on or before today and no more than 120 years before today.
/// "Today" is pinned with a fixed TimeProvider so results never depend on the run date.
/// </summary>
public class PatientBirthDateRuleTests
{
    private static readonly DateOnly Today = new(2026, 10, 8);

    public static IEnumerable<TestCaseData> ValidCases()
    {
        yield return new TestCaseData(null).SetName("BirthDate null is valid");
        yield return new TestCaseData(Today).SetName("BirthDate today is valid");
        yield return new TestCaseData(new DateOnly(1987, 3, 15)).SetName("BirthDate 1987-03-15 is valid");
        yield return new TestCaseData(Today.AddYears(-120)).SetName("BirthDate exactly 120 years ago is valid");
        yield return new TestCaseData(new DateOnly(2000, 2, 29)).SetName("BirthDate leap day 2000-02-29 is valid");
    }

    public static IEnumerable<TestCaseData> InvalidCases()
    {
        yield return new TestCaseData(Today.AddDays(1)).SetName("BirthDate tomorrow is invalid");
        yield return new TestCaseData(Today.AddYears(1)).SetName("BirthDate next year is invalid");
        yield return new TestCaseData(Today.AddYears(-120).AddDays(-1)).SetName("BirthDate 120 years and 1 day ago is invalid");
        yield return new TestCaseData(new DateOnly(1444, 3, 15)).SetName("BirthDate typed CE year read as BE (1444) is invalid");
    }

    [TestCaseSource(nameof(ValidCases))]
    public async Task Create_ShouldPass_WhenBirthDateInRange(DateOnly? birthDate)
    {
        var errors = await ValidateCreate(birthDate, Today);
        errors.Should().BeEmpty();
    }

    [TestCaseSource(nameof(InvalidCases))]
    public async Task Create_ShouldFail_WhenBirthDateOutOfRange(DateOnly? birthDate)
    {
        var errors = await ValidateCreate(birthDate, Today);
        errors.Should().ContainSingle();
    }

    [TestCaseSource(nameof(ValidCases))]
    public async Task Update_ShouldPass_WhenBirthDateInRange(DateOnly? birthDate)
    {
        var errors = await ValidateUpdate(birthDate, Today);
        errors.Should().BeEmpty();
    }

    [TestCaseSource(nameof(InvalidCases))]
    public async Task Update_ShouldFail_WhenBirthDateOutOfRange(DateOnly? birthDate)
    {
        var errors = await ValidateUpdate(birthDate, Today);
        errors.Should().ContainSingle();
    }

    [Test]
    public async Task Create_ShouldAcceptLeapDayBoundary_WhenTodayIsFeb29()
    {
        // 2028-02-29 minus 120 years = 1908-02-29 (1908 is a leap year).
        var leapToday = new DateOnly(2028, 2, 29);

        (await ValidateCreate(new DateOnly(1908, 2, 29), leapToday)).Should().BeEmpty();
        (await ValidateCreate(new DateOnly(1908, 2, 28), leapToday)).Should().ContainSingle();
    }

    [Test]
    public async Task Create_ShouldUseTimeProviderDate_NotMachineClock()
    {
        // With "today" pinned to 2000-01-01, a 2026 birth date is in the future.
        var pastToday = new DateOnly(2000, 1, 1);

        (await ValidateCreate(new DateOnly(2026, 1, 1), pastToday)).Should().ContainSingle();
        (await ValidateCreate(pastToday, pastToday)).Should().BeEmpty();
    }

    private static async Task<List<ValidationFailure>> ValidateCreate(DateOnly? birthDate, DateOnly today)
    {
        var validator = new CreatePatientCommandValidator(Mock.Of<ICpaDatabaseContext>(), new FixedTimeProvider(today));
        var command = new CreatePatientCommand { ForeName = "Test", Surname = "Patient", BirthDate = birthDate };
        var result = await validator.ValidateAsync(command);
        return BirthDateErrors(result);
    }

    private static async Task<List<ValidationFailure>> ValidateUpdate(DateOnly? birthDate, DateOnly today)
    {
        var validator = new UpdatePatientCommandValidator(Mock.Of<ICpaDatabaseContext>(), new FixedTimeProvider(today));
        var command = new UpdatePatientCommand { Id = 1, ForeName = "Test", Surname = "Patient", BirthDate = birthDate };
        var result = await validator.ValidateAsync(command);
        return BirthDateErrors(result);
    }

    private static List<ValidationFailure> BirthDateErrors(ValidationResult result) =>
        result.Errors.Where(e => e.PropertyName.EndsWith("BirthDate", StringComparison.Ordinal)).ToList();

    private sealed class FixedTimeProvider(DateOnly today) : TimeProvider
    {
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;

        public override DateTimeOffset GetUtcNow() =>
            new(today.ToDateTime(new TimeOnly(12, 0)), TimeSpan.Zero);
    }
}