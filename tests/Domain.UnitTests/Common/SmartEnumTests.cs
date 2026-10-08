using BigLion.CPA.Domain.Common;
using FluentAssertions;
using NUnit.Framework;

namespace BigLion.CPA.Domain.UnitTests.Common;

/// <summary>
/// Covers the SmartEnum base class with a test-only enum, so the lookup mechanism
/// stays tested while no production SmartEnum is registered yet.
/// </summary>
public class SmartEnumTests
{
    private sealed class SampleStatus : SmartEnum<SampleStatus>
    {
        public static readonly SampleStatus Second = new("second", "Second item", 2);
        public static readonly SampleStatus First = new("first", "First item", 1);

        private SampleStatus(string value, string name, int sort) : base(value, name, sort) { }
    }

    [Test]
    public void All_ShouldBeOrderedBySort()
    {
        SampleStatus.All.Should().ContainInOrder(SampleStatus.First, SampleStatus.Second);
    }

    [Test]
    public void FromValue_ShouldRoundTripCaseInsensitive()
    {
        SampleStatus.FromValue("FIRST").Should().BeSameAs(SampleStatus.First);
    }

    [Test]
    public void FromValue_InvalidValue_ShouldThrow()
    {
        FluentActions.Invoking(() => SampleStatus.FromValue("missing"))
            .Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void TryFromValue_InvalidValue_ShouldReturnFalse()
    {
        SampleStatus.TryFromValue("missing", out var result).Should().BeFalse();
        result.Should().BeNull();
    }

    [Test]
    public void GetDisplayName_WithoutResource_ShouldFallBackToName()
    {
        SampleStatus.First.GetDisplayName().Should().Be("First item");
        SampleStatus.First.GetDisplayName("en").Should().Be("First item");
    }
}