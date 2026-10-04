using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.UserRoleAssignments.ViewModels;

namespace BigLion.CPA.Application.Features.UserRoleAssignments.Queries.Get;

public class GetUserRolesQuery : IRequest<UserRolesViewModel>
{
    public int RoleID { get; set; }
    public int UserID { get; set; }
}

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, UserRolesViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetUserRolesQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserRolesViewModel> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(
            x => x.RoleID == request.RoleID && x.UserID == request.UserID, cancellationToken)
            ?? throw new NotFoundException("UserRoles", $"{request.RoleID}/{request.UserID}");

        return _mapper.Map<UserRolesViewModel>(entity);
    }
}
