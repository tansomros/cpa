using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.News;

namespace BigLion.CPA.Application.Features.NewsArticles.Commands.Update;

public class UpdateNewsCommand : IRequest<Unit>
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

public class UpdateNewsCommandValidator : AbstractValidator<UpdateNewsCommand>
{
    public UpdateNewsCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateNewsCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.News
            .FirstOrDefaultAsync(x => x.NewsID == request.NewsID, cancellationToken)
            ?? throw new NotFoundException("News", request.NewsID);

        entity.NewsDate = request.NewsDate;
        entity.NewsOrder = request.NewsOrder;
        entity.Title = request.Title;
        entity.FirstPage = request.FirstPage;
        entity.NewsType = request.NewsType;
        entity.LinkPath = request.LinkPath;
        entity.isPublic = request.isPublic;
        entity.ContentNews = request.ContentNews;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
