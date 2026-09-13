using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BigLion.Domain.Entities;

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

        var province = new Province("North", "50", "Chiang Mai", "Chiang Mai");

        // Act
        context.Provinces.Add(province);
        await context.SaveChangesAsync();

        // Assert
        var savedProvince = await context.Provinces.FirstAsync(p => p.ProvinceId == "50");
        savedProvince.CreatedOn.Should().Be(fixedTime);
        savedProvince.LastModified.Should().Be(fixedTime);
        savedProvince.IsActive.Should().BeTrue();
        savedProvince.DeleteFlag.Should().BeFalse();
    }

    [Test]
    public async Task ShouldSetLastModifiedPropertiesWhenUpdatingEntity()
    {
        // Arrange
        var context = Testing.CreateContext();
        var createdTime = new DateTimeOffset(2025, 1, 1, 10, 0, 0, TimeSpan.Zero);
        var updatedTime = new DateTimeOffset(2025, 1, 2, 10, 0, 0, TimeSpan.Zero);
        
        Testing.DateTimeMock.Setup(m => m.Now).Returns(createdTime);

        var province = new Province("North", "51", "Lamphun", "Lamphun");
        context.Provinces.Add(province);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        // Act
        Testing.DateTimeMock.Setup(m => m.Now).Returns(updatedTime);

        var existingProvince = await context.Provinces.FirstAsync(p => p.ProvinceId == "51");
        existingProvince.Name = "Updated Lamphun";
        await context.SaveChangesAsync();

        // Assert
        var savedProvince = await context.Provinces.FirstAsync(p => p.ProvinceId == "51");
        savedProvince.CreatedOn.Should().Be(createdTime);
        savedProvince.LastModified.Should().Be(updatedTime);
    }
}
