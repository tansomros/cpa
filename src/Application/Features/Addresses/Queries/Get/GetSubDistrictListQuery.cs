using Cpa.Application.Features.Addresses.ViewModel;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Mappings;
using Cpa.Application.Common.Models;

namespace Cpa.Application.Features.Addresses.Queries.Get;

public record GetSubDistrictListQuery : IRequest<SubDistrictListViewModel>
{
    public required string DistrictId { get; set; }
}

public class GetSubDistrictListQueryHandler : IRequestHandler<GetSubDistrictListQuery, SubDistrictListViewModel>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _context;

    public GetSubDistrictListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SubDistrictListViewModel> Handle(GetSubDistrictListQuery request, CancellationToken cancellationToken)
    {
        var subDistricts =  await _context.SubDistricts
            .Include(c => c.District)
            .AsNoTracking()
            .Where(d => d.DistrictId==request.DistrictId)
            .OrderByDescending(x => x.Name) 
            .ToListAsync(cancellationToken);

        var subDistrictsList = _mapper.Map<List<SubDistrictViewModel>>(subDistricts);

        return new SubDistrictListViewModel()
        {
            SubDistricts = subDistrictsList
        };
    }
}
