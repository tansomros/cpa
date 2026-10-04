using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Dispenses.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Dispense;

namespace BigLion.CPA.Application.Features.Dispenses.Queries.Get;

public class GetDispenseQuery : IRequest<DispenseViewModel>
{
    public long UID { get; set; }
}

public class GetDispenseQueryHandler : IRequestHandler<GetDispenseQuery, DispenseViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDispenseQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DispenseViewModel> Handle(GetDispenseQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Dispenses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("Dispense", request.UID);

        return _mapper.Map<DispenseViewModel>(entity);
    }
}
