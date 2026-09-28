using BigLion.Cpa.Application.Common.Interfaces;

namespace BigLion.Cpa.Application.Features.BoundedContext.Examples.Queries.GetExamples;

public record GetExamplesQuery : IRequest<ReturnTypePlaceholder>
{
    // TODO: request properties (filters, paging, search)
}

public class GetExamplesQueryHandler : IRequestHandler<GetExamplesQuery, ReturnTypePlaceholder>
{
    private readonly ICpaDbContext _context;
    private readonly IMapper _mapper;

    public GetExamplesQueryHandler(ICpaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReturnTypePlaceholder> Handle(GetExamplesQuery request, CancellationToken cancellationToken)
    {
        // TODO: query via _context, project to the view model, return.
        throw new NotImplementedException();
    }
}
