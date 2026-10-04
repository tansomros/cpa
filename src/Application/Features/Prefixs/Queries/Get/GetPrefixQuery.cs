using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Prefixs.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Prefix;

namespace BigLion.CPA.Application.Features.Prefixs.Queries.Get;

public class GetPrefixQuery : IRequest<PrefixViewModel>
{
    public int Id { get; set; }
}

public class GetPrefixQueryHandler : IRequestHandler<GetPrefixQuery, PrefixViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPrefixQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PrefixViewModel> Handle(GetPrefixQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Prefixs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Prefix", request.Id);

        return _mapper.Map<PrefixViewModel>(entity);
    }
}
