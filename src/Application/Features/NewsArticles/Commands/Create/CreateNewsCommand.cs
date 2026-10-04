using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.News;

namespace BigLion.CPA.Application.Features.NewsArticles.Commands.Create;

public class CreateNewsCommand : IRequest<int>
{
    public DateTime? NewsDate { get; set; }
    public int? NewsOrder { get; set; }
    public string? Title { get; set; }
    public int? FirstPage { get; set; }
    public string? NewsType { get; set; }
    public string? LinkPath { get; set; }
    public int? isPublic { get; set; }
    public string? ContentNews { get; set; }
}

public class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
{
    public CreateNewsCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateNewsCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.NewsDate = request.NewsDate;
        entity.NewsOrder = request.NewsOrder;
        entity.Title = request.Title;
        entity.FirstPage = request.FirstPage;
        entity.NewsType = request.NewsType;
        entity.LinkPath = request.LinkPath;
        entity.isPublic = request.isPublic;
        entity.ContentNews = request.ContentNews;
        await _context.News.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.NewsID;
    }
}
