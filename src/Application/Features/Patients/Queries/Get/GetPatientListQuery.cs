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
            var hasId = int.TryParse(term, out var id);
            query = query.Where(x =>
                (x.ForeName != null && x.ForeName.Contains(term))
                || (x.Surname != null && x.Surname.Contains(term))
                || (x.CardId != null && x.CardId.Contains(term))
                || (x.Telephone != null && x.Telephone.Contains(term))       
                || (hasId && x.Id == id));
        }

        return await query
            .OrderByDescending(x => x.Id)
            .ProjectTo<PatientViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit);
    }
}
