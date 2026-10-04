using BigLion.CPA.Application.Features.Patients.ViewModels;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Domain.Entities;

namespace BigLion.CPA.Application.Features.Patients.Queries.Get;

public class GetPatientByNationIdQuery : IRequest<PatientViewModel>
{
    public required string NationId { get; set; }
}

public class GetPatientByNationIdQueryHandler : IRequestHandler<GetPatientByNationIdQuery, PatientViewModel>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _checkupContext;
    public GetPatientByNationIdQueryHandler(ICpaDatabaseContext checkupContext, IMapper mapper)
    {
        _checkupContext = checkupContext;
        _mapper = mapper;
    }

    public async Task<PatientViewModel> Handle(GetPatientByNationIdQuery request, CancellationToken cancellationToken)
    {
        var patient = await _checkupContext
            .Patients
            .Include(p => p.Province)
            .Include(p => p.District)
            .Include(p => p.SubDistrict)
            .FirstOrDefaultAsync(p => p.NationId == request.NationId, cancellationToken)
            ?? throw new NotFoundException(nameof(Patient), request.NationId);
        return _mapper.Map<PatientViewModel>(patient);
       
    }
}
