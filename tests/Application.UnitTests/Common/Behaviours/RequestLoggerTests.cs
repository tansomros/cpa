using Moq;
using NUnit.Framework;
using SUTH.HealthCheckup.Application.Common.Interfaces;

namespace SUTH.HealthCheckup.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ICurrentUserService> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _user = new Mock<ICurrentUserService>();
        _identityService = new Mock<IIdentityService>();
    }

}
