using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.ServiceTypes.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.ServiceType;

namespace BigLion.CPA.Application.Features.ServiceTypes.Queries.Get;

public class GetServiceTypeQuery : IRequest<ServiceTypeViewModel>
{
    public string ServiceTypeID { get; set; } = string.Empty;
}

public class GetServiceTypeQueryHandler : IRequestHandler<GetServiceTypeQuery, ServiceTypeViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetServiceTypeQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceTypeViewModel> Handle(GetServiceTypeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.ServiceTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ServiceTypeID == request.ServiceTypeID, cancellationToken)
            ?? throw new NotFoundException("ServiceType", request.ServiceTypeID);

        return _mapper.Map<ServiceTypeViewModel>(entity);
    }
}
