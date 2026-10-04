using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.MTMDrugRemains.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.MTMDrugRemain;

namespace BigLion.CPA.Application.Features.MTMDrugRemains.Queries.Get;

public class GetMTMDrugRemainQuery : IRequest<MTMDrugRemainViewModel>
{
    public int UID { get; set; }
}

public class GetMTMDrugRemainQueryHandler : IRequestHandler<GetMTMDrugRemainQuery, MTMDrugRemainViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMDrugRemainQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MTMDrugRemainViewModel> Handle(GetMTMDrugRemainQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMDrugRemains
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMDrugRemain", request.UID);

        return _mapper.Map<MTMDrugRemainViewModel>(entity);
    }
}
