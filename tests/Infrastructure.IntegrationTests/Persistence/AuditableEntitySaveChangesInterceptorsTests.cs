using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BigLion.CPA.Domain.Entities;

namespace BigLion.Infrastructure.IntegrationTests.Persistence;

public class AuditableEntitySaveChangesInterceptorsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldSetCreatedPropertiesWhenAddingNewEntity()
    {
        // Arrange
        var context = Testing.CreateContext();
        var fixedTime = new DateTimeOffset(2025, 1, 1, 10, 0, 0, TimeSpan.Zero);
        Testing.DateTimeMock.Setup(m => m.Now).Returns(fixedTime);

        var pharmacyType = new PharmacyType("T50", "Type 50", 1);

        // Act
        context.PharmacyTypes.Add(pharmacyType);
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.PharmacyTypes.FirstAsync(p => p.Code == "T50");
        saved.CreatedOn.Should().Be(fixedTime);
        saved.LastModified.Should().Be(fixedTime);
        saved.IsActive.Should().BeTrue();
        saved.DeleteFlag.Should().BeFalse();
    }

    [Test]
    public async Task ShouldSetLastModifiedPropertiesWhenUpdatingEntity()
    {
        // Arrange
        var context = Testing.CreateContext();
        var createdTime = new DateTimeOffset(2025, 1, 1, 10, 0, 0, TimeSpan.Zero);
        var updatedTime = new DateTimeOffset(2025, 1, 2, 10, 0, 0, TimeSpan.Zero);
        
        Testing.DateTimeMock.Setup(m => m.Now).Returns(createdTime);

        var pharmacyType = new PharmacyType("T51", "Type 51", 1);
        context.PharmacyTypes.Add(pharmacyType);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        // Act
        Testing.DateTimeMock.Setup(m => m.Now).Returns(updatedTime);

        var existing = await context.PharmacyTypes.FirstAsync(p => p.Code == "T51");
        existing.Name = "Updated Type 51";
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.PharmacyTypes.FirstAsync(p => p.Code == "T51");
        saved.CreatedOn.Should().Be(createdTime);
        saved.LastModified.Should().Be(updatedTime);
    }
}
