using Cpa.Application.Common.Interfaces;
using Cpa.Application.Features.Users.ViewModel;

namespace Cpa.Application.Features.Users.Queries.Get;

public record GetUserLoginQuery : IRequest<UserListViewModel>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class GetUserListWithClassQueryHandler : IRequestHandler<GetUserLoginQuery, UserListViewModel>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _context;

    public GetUserListWithClassQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserListViewModel> Handle(GetUserLoginQuery request, CancellationToken cancellationToken)
    {
        var Users = await _context.Users
            .AsNoTracking()            
            .Where(l => l.Username == request.Username && l.PasswordHash== request.Password)
            .ToListAsync(cancellationToken);
                
        var UserList = _mapper.Map<List<UserViewModel>>(Users);

        return new UserListViewModel() { Users = UserList };
    }
}
