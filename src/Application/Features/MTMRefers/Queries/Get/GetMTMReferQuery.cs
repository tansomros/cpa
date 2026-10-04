using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.MTMRefers.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.MTMRefer;

namespace BigLion.CPA.Application.Features.MTMRefers.Queries.Get;

public class GetMTMReferQuery : IRequest<MTMReferViewModel>
{
    public int UID { get; set; }
}

public class GetMTMReferQueryHandler : IRequestHandler<GetMTMReferQuery, MTMReferViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMReferQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MTMReferViewModel> Handle(GetMTMReferQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMRefers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMRefer", request.UID);

        return _mapper.Map<MTMReferViewModel>(entity);
    }
}
