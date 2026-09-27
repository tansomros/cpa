using Cpa.Application.Features.Patients.ViewModels;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Exceptions;
using Cpa.Domain.Entities;

namespace Cpa.Application.Features.Patients.Queries.Get;

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
