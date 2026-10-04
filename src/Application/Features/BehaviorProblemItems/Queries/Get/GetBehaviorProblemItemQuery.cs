using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.BehaviorProblemItems.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.BehaviorProblemItem;

namespace BigLion.CPA.Application.Features.BehaviorProblemItems.Queries.Get;

public class GetBehaviorProblemItemQuery : IRequest<BehaviorProblemItemViewModel>
{
    public int UID { get; set; }
}

public class GetBehaviorProblemItemQueryHandler : IRequestHandler<GetBehaviorProblemItemQuery, BehaviorProblemItemViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetBehaviorProblemItemQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BehaviorProblemItemViewModel> Handle(GetBehaviorProblemItemQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.BehaviorProblemItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("BehaviorProblemItem", request.UID);

        return _mapper.Map<BehaviorProblemItemViewModel>(entity);
    }
}
