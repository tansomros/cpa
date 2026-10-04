using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.ServiceRecords.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Services;

namespace BigLion.CPA.Application.Features.ServiceRecords.Queries.Get;

public class GetServicesQuery : IRequest<ServicesViewModel>
{
    public long itemID { get; set; }
}

public class GetServicesQueryHandler : IRequestHandler<GetServicesQuery, ServicesViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetServicesQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServicesViewModel> Handle(GetServicesQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.itemID == request.itemID, cancellationToken)
            ?? throw new NotFoundException("Services", request.itemID);

        return _mapper.Map<ServicesViewModel>(entity);
    }
}
