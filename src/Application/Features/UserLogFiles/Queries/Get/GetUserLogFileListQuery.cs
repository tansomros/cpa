using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.UserLogFiles.ViewModels;

namespace BigLion.CPA.Application.Features.UserLogFiles.Queries.Get;

public class GetUserLogFileListQuery : IRequest<PaginatedList<UserLogFileViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class GetUserLogFileListQueryValidator : AbstractValidator<GetUserLogFileListQuery>
{
    public GetUserLogFileListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetUserLogFileListQueryHandler : IRequestHandler<GetUserLogFileListQuery, PaginatedList<UserLogFileViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetUserLogFileListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserLogFileViewModel>> Handle(GetUserLogFileListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.UserLogFiles.AsNoTracking();

        return await query
            .OrderBy(x => x.LogID)
            .ProjectTo<UserLogFileViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
