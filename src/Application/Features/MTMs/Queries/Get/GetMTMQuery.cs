using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.MTMs.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.MTM;

namespace BigLion.CPA.Application.Features.MTMs.Queries.Get;

public class GetMTMQuery : IRequest<MTMViewModel>
{
    public int UID { get; set; }
}

public class GetMTMQueryHandler : IRequestHandler<GetMTMQuery, MTMViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MTMViewModel> Handle(GetMTMQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.MTMs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("MTM", request.UID);

        return _mapper.Map<MTMViewModel>(entity);
    }
}
