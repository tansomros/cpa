using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.NewsArticles.ViewModels;

public class NewsListViewModel
{
    public ICollection<NewsViewModel> News { get; set; }

    public NewsListViewModel()
    {
        News = new HashSet<NewsViewModel>();
    }
}
