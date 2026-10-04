using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Deseases.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Desease;

namespace BigLion.CPA.Application.Features.Deseases.Queries.Get;

public class GetDeseaseQuery : IRequest<DeseaseViewModel>
{
    public int UID { get; set; }
}

public class GetDeseaseQueryHandler : IRequestHandler<GetDeseaseQuery, DeseaseViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDeseaseQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DeseaseViewModel> Handle(GetDeseaseQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Deseases
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("Desease", request.UID);

        return _mapper.Map<DeseaseViewModel>(entity);
    }
}
