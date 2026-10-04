using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Hospitals.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Hospital;

namespace BigLion.CPA.Application.Features.Hospitals.Queries.Get;

public class GetHospitalQuery : IRequest<HospitalViewModel>
{
    public int HospitalUID { get; set; }
}

public class GetHospitalQueryHandler : IRequestHandler<GetHospitalQuery, HospitalViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetHospitalQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<HospitalViewModel> Handle(GetHospitalQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Hospitals
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.HospitalUID == request.HospitalUID, cancellationToken)
            ?? throw new NotFoundException("Hospital", request.HospitalUID);

        return _mapper.Map<HospitalViewModel>(entity);
    }
}
