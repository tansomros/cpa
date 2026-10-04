using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Users.ViewModel;

namespace BigLion.CPA.Application.Features.Users.Queries.Get;

public record GetUserListQuery : IRequest<PaginatedList<UserViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string? Search { get; init; }
}

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, PaginatedList<UserViewModel>>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _context;

    public GetUserListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserViewModel>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x =>
                x.Username.Contains(term)
                || x.DisplayName.Contains(term)
                || (x.PositionName != null && x.PositionName.Contains(term))
                || (x.Email != null && x.Email.Contains(term)));
        }

        return await query
            .OrderBy(x => x.Id)
            .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
