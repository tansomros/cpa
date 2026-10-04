using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using MediatR;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Features.Patients.ViewModels;

namespace BigLion.CPA.Application.Features.Patients.Queries.Get;

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

    public string? Search { get; init; }
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
        var query = _context.Patients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x =>
                x.FirstName.Contains(term)
                || x.LastName.Contains(term)
                || x.Prefix.Contains(term)
                || (x.HospitalNumber != null && x.HospitalNumber.Contains(term))
                || (x.NationId != null && x.NationId.Contains(term))
                || (x.TelephoneNumber != null && x.TelephoneNumber.Contains(term)));
        }

        return await query
            .OrderByDescending(x => x.Id)
            .ProjectTo<PatientViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit);
    }
}
