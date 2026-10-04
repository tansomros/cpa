using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.UserRoleAssignments.ViewModels;

namespace BigLion.CPA.Application.Features.UserRoleAssignments.Queries.Get;

public class GetUserRolesListQuery : IRequest<PaginatedList<UserRolesViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public int? RoleID { get; init; }
    public int? UserID { get; init; }
}

public class GetUserRolesListQueryValidator : AbstractValidator<GetUserRolesListQuery>
{
    public GetUserRolesListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetUserRolesListQueryHandler : IRequestHandler<GetUserRolesListQuery, PaginatedList<UserRolesViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetUserRolesListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserRolesViewModel>> Handle(GetUserRolesListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.UserRoles.AsNoTracking();
        if (request.RoleID.HasValue)
        {
            query = query.Where(x => x.RoleID == request.RoleID);
        }

        if (request.UserID.HasValue)
        {
            query = query.Where(x => x.UserID == request.UserID);
        }

        return await query
            .OrderBy(x => x.RoleID)
            .ThenBy(x => x.UserID)
            .ProjectTo<UserRolesViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
