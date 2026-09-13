using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SUTH.HealthCheckup.Application.Common.Behaviours;
using SUTH.HealthCheckup.Application.Common.Interfaces;
using SUTH.HealthCheckup.Application.Common.Security;
using SUTH.HealthCheckup.Application.Exceptions;

namespace SUTH.HealthCheckup.Application.UnitTests.Common.Behaviours;

public class AuthorizationBehaviourTests
{
    private Mock<ICurrentUserService> _currentUserServiceMock = null!;
    private AuthorizationBehaviour<TestRequest, Unit> _behaviour = null!;
    private AuthorizationBehaviour<TestRequestWithAuth, Unit> _authBehaviour = null!;
    private AuthorizationBehaviour<TestRequestWithRoles, Unit> _roleBehaviour = null!;
    private AuthorizationBehaviour<TestRequestWithPolicies, Unit> _policyBehaviour = null!;
    private RequestHandlerDelegate<Unit> _next = null!;

    [SetUp]
    public void Setup()
    {
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _behaviour = new AuthorizationBehaviour<TestRequest, Unit>(_currentUserServiceMock.Object);
        _authBehaviour = new AuthorizationBehaviour<TestRequestWithAuth, Unit>(_currentUserServiceMock.Object);
        _roleBehaviour = new AuthorizationBehaviour<TestRequestWithRoles, Unit>(_currentUserServiceMock.Object);
        _policyBehaviour = new AuthorizationBehaviour<TestRequestWithPolicies, Unit>(_currentUserServiceMock.Object);
        _next = ct => Task.FromResult(Unit.Value);
    }

    [Test]
    public async Task Handle_ShouldContinue_WhenNoAuthorizeAttribute()
    {
        var request = new TestRequest();
        var result = await _behaviour.Handle(request, _next, CancellationToken.None);
        result.Should().Be(Unit.Value);
    }

    [Test]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenNotAuthenticated()
    {
        var request = new TestRequestWithAuth();
        _currentUserServiceMock.Setup(x => x.Id).Returns(string.Empty);

        var action = async () => await _authBehaviour.Handle(request, _next, CancellationToken.None);

        await action.Should().ThrowAsync<CheckupUnauthorizedAccessException>();
    }

    [Test]
    public async Task Handle_ShouldContinue_WhenAuthenticatedAndNoRolesOrPoliciesRequired()
    {
        var request = new TestRequestWithAuth();
        _currentUserServiceMock.Setup(x => x.Id).Returns("UserId");

        var result = await _authBehaviour.Handle(request, _next, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }

    [Test]
    public async Task Handle_ShouldThrowForbiddenAccessException_WhenRoleIsMissing()
    {
        var request = new TestRequestWithRoles();
        _currentUserServiceMock.Setup(x => x.Id).Returns("UserId");
        _currentUserServiceMock.Setup(x => x.IsInRole("Admin")).Returns(false);

        var action = async () => await _roleBehaviour.Handle(request, _next, CancellationToken.None);

        await action.Should().ThrowAsync<CheckupForbiddenAccessException>();
    }

    [Test]
    public async Task Handle_ShouldContinue_WhenUserHasRequiredRole()
    {
        var request = new TestRequestWithRoles();
        _currentUserServiceMock.Setup(x => x.Id).Returns("UserId");
        _currentUserServiceMock.Setup(x => x.IsInRole("Admin")).Returns(true);

        var result = await _roleBehaviour.Handle(request, _next, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }

    [Test]
    public async Task Handle_ShouldThrowForbiddenAccessException_WhenPolicyIsMissing()
    {
        var request = new TestRequestWithPolicies();
        _currentUserServiceMock.Setup(x => x.Id).Returns("UserId");
        _currentUserServiceMock.Setup(x => x.IsInPolicyAsync("CanPurge")).ReturnsAsync(false);

        var action = async () => await _policyBehaviour.Handle(request, _next, CancellationToken.None);

        await action.Should().ThrowAsync<CheckupForbiddenAccessException>();
    }
    
    [Test]
    public async Task Handle_ShouldContinue_WhenUserHasRequiredPolicy()
    {
        var request = new TestRequestWithPolicies();
        _currentUserServiceMock.Setup(x => x.Id).Returns("UserId");
        _currentUserServiceMock.Setup(x => x.IsInPolicyAsync("CanPurge")).ReturnsAsync(true);

        var result = await _policyBehaviour.Handle(request, _next, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }

    public class TestRequest : IRequest<Unit> { }

    [Authorize]
    public class TestRequestWithAuth : IRequest<Unit> { }

    [Authorize(Roles = "Admin")]
    public class TestRequestWithRoles : IRequest<Unit> { }

    [Authorize(Policy = "CanPurge")]
    public class TestRequestWithPolicies : IRequest<Unit> { }
}
