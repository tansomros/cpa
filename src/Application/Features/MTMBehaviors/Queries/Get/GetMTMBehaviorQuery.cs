using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.MTMBehaviors.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.MTMBehavior;

namespace BigLion.CPA.Application.Features.MTMBehaviors.Queries.Get;

public class GetMTMBehaviorQuery : IRequest<MTMBehaviorViewModel>
{
    public int UID { get; set; }
}

public class GetMTMBehaviorQueryHandler : IRequestHandler<GetMTMBehaviorQuery, MTMBehaviorViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMBehaviorQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MTMBehaviorViewModel> Handle(GetMTMBehaviorQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMBehaviors
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMBehavior", request.UID);

        return _mapper.Map<MTMBehaviorViewModel>(entity);
    }
}
