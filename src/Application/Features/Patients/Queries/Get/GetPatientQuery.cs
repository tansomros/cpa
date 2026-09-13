using Cpa.Application.Features.Patients.ViewModels;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Exceptions;
using Cpa.Domain.Entities;

namespace Cpa.Application.Features.Patients.Queries.Get;

public class GetPatientQuery : IRequest<PatientViewModel>
{
    public int Id { get; set; }
}

public class GetPatientQueryHandler : IRequestHandler<GetPatientQuery, PatientViewModel>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _checkupContext;
    public GetPatientQueryHandler(ICpaDatabaseContext checkupContext, IMapper mapper)
    {
        _checkupContext = checkupContext;
        _mapper = mapper;
    }

    public async Task<PatientViewModel> Handle(GetPatientQuery request, CancellationToken cancellationToken)
    {
        var patient = await _checkupContext.Patients.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Patient), request.Id);
        return _mapper.Map<PatientViewModel>(patient);
       
    }
}
