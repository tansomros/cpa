using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SUTH.HealthCheckup.Application.Common.Behaviours;
using ValidationException = SUTH.HealthCheckup.Application.Exceptions.ValidationException;

namespace SUTH.HealthCheckup.Application.UnitTests.Common.Behaviours;

public class ValidationBehaviourTests
{
    [Test]
    public async Task Handle_ShouldContinue_WhenNoValidatorsExist()
    {
        var behaviour = new ValidationBehaviour<TestRequest, Unit>(new List<IValidator<TestRequest>>());
        var next = new RequestHandlerDelegate<Unit>(ct => Task.FromResult(Unit.Value));

        var result = await behaviour.Handle(new TestRequest(), next, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }

    [Test]
    public async Task Handle_ShouldContinue_WhenValidationPasses()
    {
        var validatorMock = new Mock<IValidator<TestRequest>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new ValidationResult()); // No errors

        var behaviour = new ValidationBehaviour<TestRequest, Unit>(new List<IValidator<TestRequest>> { validatorMock.Object });
        var next = new RequestHandlerDelegate<Unit>(ct => Task.FromResult(Unit.Value));

        var result = await behaviour.Handle(new TestRequest(), next, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }

    [Test]
    public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
    {
        var validatorMock = new Mock<IValidator<TestRequest>>();
        var failures = new List<ValidationFailure> { new ValidationFailure("Property", "Error message") };
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new ValidationResult(failures));

        var behaviour = new ValidationBehaviour<TestRequest, Unit>(new List<IValidator<TestRequest>> { validatorMock.Object });
        var next = new RequestHandlerDelegate<Unit>(ct => Task.FromResult(Unit.Value));

        var action = async () => await behaviour.Handle(new TestRequest(), next, CancellationToken.None);

        await action.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.ContainsKey("Property"));
    }

    public class TestRequest : IRequest<Unit> { }
}
