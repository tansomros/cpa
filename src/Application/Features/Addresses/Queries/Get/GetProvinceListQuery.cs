using Cpa.Application.Features.Addresses.ViewModel;
using Cpa.Application.Common.Interfaces;

namespace Cpa.Application.Features.Addresses.Queries.Get;

public record GetProvinceListQuery : IRequest<ProvinceListViewModel>
{
  
}

public class GetProvinceListQueryHandler : IRequestHandler<GetProvinceListQuery,ProvinceListViewModel>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _context;

    public GetProvinceListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProvinceListViewModel> Handle(GetProvinceListQuery request, CancellationToken cancellationToken)
    {
        var provinces =  await _context.Provinces
            .AsNoTracking()
            .OrderByDescending(x => x.Name) 
            .ToListAsync(cancellationToken);

        var provincesList = _mapper.Map<List<ProvinceViewModel>>(provinces);

        return new ProvinceListViewModel()
        {
            Provinces = provincesList
        };
    }
}
