using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.BehaviorProblems.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.BehaviorProblem;

namespace BigLion.CPA.Application.Features.BehaviorProblems.Queries.Get;

public class GetBehaviorProblemQuery : IRequest<BehaviorProblemViewModel>
{
    public int UID { get; set; }
}

public class GetBehaviorProblemQueryHandler : IRequestHandler<GetBehaviorProblemQuery, BehaviorProblemViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetBehaviorProblemQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BehaviorProblemViewModel> Handle(GetBehaviorProblemQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.BehaviorProblems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("BehaviorProblem", request.UID);

        return _mapper.Map<BehaviorProblemViewModel>(entity);
    }
}
