using Cpa.Application.Common.Mappings;
using Cpa.Application.Common.Models;
using MediatR;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Features.Patients.ViewModels;

namespace Cpa.Application.Features.Patients.Queries.Get;

public class GetPatientListQuery : IRequest<PaginatedList<PatientViewModel>>
{
    /// <summary>
    /// หน้าของข้อมูลเป็นเลขจำนวนเต็ม
    /// </summary>
    public int Page { get; init; } = 1;

    /// <summary>
    /// จำนวนของข้อมูลที่ต้องการเป็นเลขจำนวนเต็ม
    /// </summary>
    public int Limit { get; init; } = 10;
}
public class GetPatientListQueryHandler : IRequestHandler<GetPatientListQuery, PaginatedList<PatientViewModel>>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _context;

    public GetPatientListQueryHandler(IMapper mapper, ICpaDatabaseContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaginatedList<PatientViewModel>> Handle(GetPatientListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Patients
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedOn)
            .ProjectTo<PatientViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit);
    }
}
