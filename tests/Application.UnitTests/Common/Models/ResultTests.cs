using FluentAssertions;
using NUnit.Framework;
using SUTH.HealthCheckup.Application.Common.Models;

namespace SUTH.HealthCheckup.Application.UnitTests.Common.Models;

public class ResultTests
{
    [Test]
    public void Success_ShouldCreateSuccessfulResult()
    {
        var result = Result.Success();

        result.Succeeded.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void Failure_ShouldCreateFailedResultWithErrors()
    {
        var errors = new[] { "Error 1", "Error 2" };

        var result = Result.Failure(errors);

        result.Succeeded.Should().BeFalse();
        result.Errors.Should().HaveCount(2).And.Contain(errors);
    }
}
