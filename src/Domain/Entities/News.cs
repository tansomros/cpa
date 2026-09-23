namespace Cpa.Domain.Entities;

public class News
{
    public int NewsID { get; set; }

    public DateTime? NewsDate { get; set; }

    public int? NewsOrder { get; set; }

    public string? Title { get; set; }

    public int? FirstPage { get; set; }

    public string? NewsType { get; set; }

    public string? LinkPath { get; set; }

    public int? isPublic { get; set; }

    public string? ContentNews { get; set; }
}

