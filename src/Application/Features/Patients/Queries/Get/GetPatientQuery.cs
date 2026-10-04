using BigLion.CPA.Application.Features.Patients.ViewModels;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Domain.Entities;

namespace BigLion.CPA.Application.Features.Patients.Queries.Get;

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
