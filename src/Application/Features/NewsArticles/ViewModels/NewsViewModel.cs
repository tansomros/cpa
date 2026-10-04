using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.News;

namespace BigLion.CPA.Application.Features.NewsArticles.ViewModels;

public class NewsViewModel : IMapFrom<Entity>
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

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, NewsViewModel>();
    }
}
