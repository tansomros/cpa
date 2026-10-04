using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.NewsArticles.Commands.Delete;

public class DeleteNewsCommand : IRequest<Unit>
{
    public int NewsID { get; set; }
}

public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteNewsCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.News
            .FirstOrDefaultAsync(x => x.NewsID == request.NewsID, cancellationToken)
            ?? throw new NotFoundException("News", request.NewsID);

        _context.News.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
