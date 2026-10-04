using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.NewsArticles.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.News;

namespace BigLion.CPA.Application.Features.NewsArticles.Queries.Get;

public class GetNewsQuery : IRequest<NewsViewModel>
{
    public int NewsID { get; set; }
}

public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, NewsViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetNewsQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<NewsViewModel> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.News
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.NewsID == request.NewsID, cancellationToken)
            ?? throw new NotFoundException("News", request.NewsID);

        return _mapper.Map<NewsViewModel>(entity);
    }
}
