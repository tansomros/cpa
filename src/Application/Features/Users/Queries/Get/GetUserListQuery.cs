using Cpa.Application.Common.Interfaces;
using Cpa.Application.Features.Users.ViewModel;

namespace Cpa.Application.Features.Users.Queries.Get;

public record GetUserListQuery : IRequest<UserListViewModel>
{
    public required string visitNumber { get; set; }
}

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, UserListViewModel>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _context;

    public GetUserListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserListViewModel> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {     

        var Users = await _context.Users.AsNoTracking()            
            .OrderByDescending(x => x.CreatedOn)
            .ToListAsync(cancellationToken);

        var UsersModel = _mapper.Map<List<UserViewModel>>(Users);
        return new UserListViewModel { Users = UsersModel };
    }
}
