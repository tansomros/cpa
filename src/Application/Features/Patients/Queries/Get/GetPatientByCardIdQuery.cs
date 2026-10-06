using BigLion.CPA.Application.Features.Patients.ViewModels;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Domain.Entities;

namespace BigLion.CPA.Application.Features.Patients.Queries.Get;

public class GetPatientByCardIdQuery : IRequest<PatientViewModel>
{
    public required string CardId { get; set; }
}

public class GetPatientByCardIdQueryHandler : IRequestHandler<GetPatientByCardIdQuery, PatientViewModel>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _context;

    public GetPatientByCardIdQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PatientViewModel> Handle(GetPatientByCardIdQuery request, CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .Include(p => p.Province)
            .Include(p => p.District)
            .FirstOrDefaultAsync(p => p.CardId == request.CardId, cancellationToken)
            ?? throw new NotFoundException(nameof(Patient), request.CardId);

        return _mapper.Map<PatientViewModel>(patient);
    }
}
