using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.MTMDeseases.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.MTMDesease;

namespace BigLion.CPA.Application.Features.MTMDeseases.Queries.Get;

public class GetMTMDeseaseQuery : IRequest<MTMDeseaseViewModel>
{
    public int UID { get; set; }
}

public class GetMTMDeseaseQueryHandler : IRequestHandler<GetMTMDeseaseQuery, MTMDeseaseViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMDeseaseQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MTMDeseaseViewModel> Handle(GetMTMDeseaseQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMDeseases
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTMDesease", request.UID);

        return _mapper.Map<MTMDeseaseViewModel>(entity);
    }
}
