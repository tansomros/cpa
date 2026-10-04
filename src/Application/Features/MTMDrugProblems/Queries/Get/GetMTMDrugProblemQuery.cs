using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.MTMDrugProblems.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.MTMDrugProblem;

namespace BigLion.CPA.Application.Features.MTMDrugProblems.Queries.Get;

public class GetMTMDrugProblemQuery : IRequest<MTMDrugProblemViewModel>
{
    public int UID { get; set; }
}

public class GetMTMDrugProblemQueryHandler : IRequestHandler<GetMTMDrugProblemQuery, MTMDrugProblemViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMDrugProblemQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MTMDrugProblemViewModel> Handle(GetMTMDrugProblemQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMDrugProblems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMDrugProblem", request.UID);

        return _mapper.Map<MTMDrugProblemViewModel>(entity);
    }
}
