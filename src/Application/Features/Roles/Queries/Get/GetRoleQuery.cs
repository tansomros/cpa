using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Roles.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Role;

namespace BigLion.CPA.Application.Features.Roles.Queries.Get;

public class GetRoleQuery : IRequest<RoleViewModel>
{
    public int Id { get; set; }
}

public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, RoleViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetRoleQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoleViewModel> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Role", request.Id);

        return _mapper.Map<RoleViewModel>(entity);
    }
}
