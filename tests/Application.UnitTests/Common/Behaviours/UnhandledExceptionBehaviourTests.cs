using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SUTH.HealthCheckup.Application.Common.Behaviours;

namespace SUTH.HealthCheckup.Application.UnitTests.Common.Behaviours;

public class UnhandledExceptionBehaviourTests
{
    [Test]
    public async Task Handle_ShouldContinue_WhenNoExceptionsAreThrown()
    {
        var loggerMock = new Mock<ILogger<TestRequest>>();
        var behaviour = new UnhandledExceptionBehaviour<TestRequest, Unit>(loggerMock.Object);
        var next = new RequestHandlerDelegate<Unit>(ct => Task.FromResult(Unit.Value));

        var result = await behaviour.Handle(new TestRequest(), next, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }

    [Test]
    public async Task Handle_ShouldLogAndRethrow_WhenExceptionIsThrown()
    {
        var loggerMock = new Mock<ILogger<TestRequest>>();
        var behaviour = new UnhandledExceptionBehaviour<TestRequest, Unit>(loggerMock.Object);
        
        var exception = new Exception("Test Exception");
        var next = new RequestHandlerDelegate<Unit>(ct => throw exception);

        var action = async () => await behaviour.Handle(new TestRequest(), next, CancellationToken.None);

        await action.Should().ThrowAsync<Exception>().WithMessage("Test Exception");

        // Verify that LogError was called
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unhandled Exception for Request TestRequest")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    public class TestRequest : IRequest<Unit> { }
}
