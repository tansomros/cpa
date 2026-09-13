using FluentAssertions;
using NUnit.Framework;
using SUTH.HealthCheckup.Application.Common.Models;

namespace SUTH.HealthCheckup.Application.UnitTests.Common.Models;

public class PaginatedListTests
{
    [Test]
    public void Constructor_ShouldCalculateTotalPagesCorrectly()
    {
        var items = new List<string> { "Item 1", "Item 2" };
        var totalCount = 5;
        var pageNumber = 1;
        var pageSize = 2;

        var list = new PaginatedList<string>(items, totalCount, pageNumber, pageSize);

        list.TotalPages.Should().Be(3); // math.ceiling(5 / 2.0) = 3
        list.TotalCount.Should().Be(5);
        list.PageNumber.Should().Be(1);
        list.Items.Should().HaveCount(2);   
    }

    [Test]
    public void HasNextPage_ShouldBeTrue_WhenNotOnLastPage()
    {
        var list = new PaginatedList<string>(new List<string>(), count: 5, pageNumber: 1, pageSize: 2);
        list.HasNextPage.Should().BeTrue();
    }

    [Test]
    public void HasNextPage_ShouldBeFalse_WhenOnLastPage()
    {
        var list = new PaginatedList<string>(new List<string>(), count: 5, pageNumber: 3, pageSize: 2);
        list.HasNextPage.Should().BeFalse();
    }

    [Test]
    public void HasPreviousPage_ShouldBeTrue_WhenNotOnFirstPage()
    {
        var list = new PaginatedList<string>(new List<string>(), count: 5, pageNumber: 2, pageSize: 2);
        list.HasPreviousPage.Should().BeTrue();
    }

    [Test]
    public void HasPreviousPage_ShouldBeFalse_WhenOnFirstPage()
    {
        var list = new PaginatedList<string>(new List<string>(), count: 5, pageNumber: 1, pageSize: 2);
        list.HasPreviousPage.Should().BeFalse();
    }
}
